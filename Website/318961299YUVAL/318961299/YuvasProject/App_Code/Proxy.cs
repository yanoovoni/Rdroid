using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Data.OleDb;
using System.Data;

/// <summary>
/// Summary description for PhoneManager
/// </summary>
public sealed class Proxy
{
    private static readonly Proxy our_instance = new Proxy();
    private ProxySocketInterface Proxy_Socket;
    private Dictionary<string, Phone> Phone_By_Id_Dict;
    private Dictionary<string, Phone> Phone_By_Email_Dict;
    private Queue<string> Input_Queue;
    protected OleDbConnection myConnection;
    private Thread Proxy_Manager_Thread;

    public static Proxy Get_Instance()
    {
        return our_instance;
    }

    private Proxy()
    {
        Proxy_Socket = ProxySocketInterface.Get_Instance();
        Phone_By_Id_Dict = new Dictionary<string, Phone>();
        Phone_By_Email_Dict = new Dictionary<string, Phone>();
        Input_Queue = new Queue<string>();
        string connectionString = Connect.getConnectionString();
        myConnection = new OleDbConnection(connectionString);
        this.Proxy_Manager_Thread = new Thread(new ThreadStart(this.ProxyManagerThreadMethod));
        Proxy_Manager_Thread.IsBackground = true;
        Proxy_Manager_Thread.Start();
    }

    private void ProxyManagerThreadMethod()
    {
        while (true)
        {
            string Message = Proxy_Socket.Recv();
            if (Protocol.Is_Proxy_Message(Message))
            {
                string Purpose = Protocol.Get_Message_Purpose(Message);
                switch (Purpose)
                {
                    case "NOTIFY_SESSION_ID":
                        string Connected_Id;
                        if (Protocol.Get_Message_Parameters(Message).TryGetValue("session_id", out Connected_Id))
                        {
                            AddPhoneById(Connected_Id);
                        }
                        break;
                    case "NOTIFY_SESSION_DISCONNECT":
                        string Disconnected_Id;
                        if (Protocol.Get_Message_Parameters(Message).TryGetValue("session_id", out Disconnected_Id))
                        {
                            Phone Removed_Phone = GetPhoneById(Disconnected_Id);
                            if (Removed_Phone.IsLoggedIn())
                            {
                                Phone_By_Email_Dict.Remove(Removed_Phone.GetEmail());
                            }
                            Phone_By_Id_Dict.Remove(Disconnected_Id);
                        }
                        break;
                }
            }
            else
            {
                if (Protocol.IsPhoneMessage(Message))
                {
                    //todo do stuff with the phone message
                    string Phone_Id = Protocol.Get_Sender_Id(Message);
                    string Purpose = Protocol.Get_Message_Purpose(Message);
                    if (Purpose == "LOGIN")
                    {
                        string Email;
                        string Password;
                        if (Protocol.Get_Message_Parameters(Message).TryGetValue("email", out Email) && Protocol.Get_Message_Parameters(Message).TryGetValue("password", out Password))
                        {
                            if (IsValidLogin(Email, Password))
                            {
                                AddPhoneByEmail(Phone_Id, Email);
                                Proxy_Socket.Send(Protocol.Create_Login_Result_Message(Phone_Id, true));
                            }
                            else
                            {
                                Proxy_Socket.Send(Protocol.Create_Login_Result_Message(Phone_Id, false));
                            }
                        }
                    }
                    else
                    {
                        if (Purpose == "TASK_RESULTS")
                        {
                            Dictionary<string, string> Task_Parameters_Dict = Protocol.Get_Message_Parameters(Message);
                            string Task_Id;
                            string Task_Output;
                            if (Task_Parameters_Dict.TryGetValue("task_id", out Task_Id) && Task_Parameters_Dict.TryGetValue("output", out Task_Output))
                            {
                                GetPhoneById(Phone_Id).AddRecievedTask(Task_Id, Task_Output);
                            }
                        }
                    }
                }
            }
        }
    }

    private bool IsValidLogin(string Email, string Password)
    {
        OleDbDataReader reader;
        UserDetails userDetails = new UserDetails();
        userDetails.email = Email;
        userDetails.password = Password;
        OleDbCommand myCmd = new OleDbCommand("CheckIfUserExistsByEmailAndPassword", myConnection);
        myCmd.CommandType = CommandType.StoredProcedure;
        OleDbDataAdapter adapter = new OleDbDataAdapter();
        adapter.SelectCommand = myCmd;
        DataSet dataset = new DataSet();
        OleDbParameter objParam;

        objParam = myCmd.Parameters.Add("@Email", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.email;

        objParam = myCmd.Parameters.Add("@Password", OleDbType.BSTR);
        objParam.Direction = ParameterDirection.Input;
        objParam.Value = userDetails.password;
        try
        {
            myConnection.Open();
            reader = myCmd.ExecuteReader();


            while (reader.Read())
            {
                userDetails.firstName = reader["FirstName"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            myConnection.Close();
        }
        if (userDetails.firstName == null)
        {
            return false;
        }
        return true;
    }

    private void AddPhoneById(string Id)
    {
        try
        {
            Phone_By_Id_Dict.Add(Id, new Phone(Id));
        }
        catch (Exception e)
        { }
    }

    private void AddPhoneByEmail(string Id, string Email) // needs the phone to be in the Phone_By_Id_Dict.
    {
        Phone Added_Phone = GetPhoneById(Id);
        if (Added_Phone != null && GetPhoneByEmail(Email) == null)
        {
            Added_Phone.SetEmail(Email);
            try
            {
                Phone_By_Email_Dict.Add(Email, Added_Phone);
            }
            catch (Exception e)
            { }
        }
    }

    public Phone GetPhoneById(string Id)
    {
        Phone Wanted_Phone;
        if (Phone_By_Id_Dict.TryGetValue(Id, out Wanted_Phone))
        {
            return Wanted_Phone;
        }
        return null;
    }

    public Phone GetPhoneByEmail(string Email)
    {
        Phone Wanted_Phone;
        if (Phone_By_Email_Dict.TryGetValue(Email, out Wanted_Phone))
        {
            return Wanted_Phone;
        }
        return null;
    }

    public string HandleTask(string Email, string Type, string[] Parameters)
    {
        try
        {
            Phone Tasked_Phone = GetPhoneByEmail(Email);
            string Phone_Id = Tasked_Phone.GetId();
            string Task_Id = Tasked_Phone.GenerateTaskId();
            string Task_Message = Protocol.Create_Task_Message(Phone_Id, Task_Id, Type, Parameters);
            Proxy_Socket.Send(Task_Message);
            string Task_Output = null;
            bool Got_Output = false;
            while (!Got_Output)
            {
                Task_Output = Tasked_Phone.GetRecievedTaskOutput(Task_Id);
                if (Task_Output != null)
                {
                    Got_Output = true;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            return Task_Output;
        }
        catch
        {
            return "";
        }
    }
}