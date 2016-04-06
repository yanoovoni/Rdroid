using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }

    private void Proxy_Manager_Thread_Method()
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
                            Add_Phone_By_Id(Connected_Id);
                        }
                        break;
                    case "NOTIFY_SESSION_DISCONNECT":
                        string Disconnected_Id;
                        if (Protocol.Get_Message_Parameters(Message).TryGetValue("session_id", out Disconnected_Id))
                        {
                            Phone_By_Id_Dict.Remove(Disconnected_Id);
                        }
                        break;
                }
            }
            else
            {
                if (Protocol.Is_Phone_Message(Message))
                {
                    //todo do stuff with the phone message
                    string Purpose = Protocol.Get_Message_Purpose(Message);
                    if (Purpose == "LOGIN")
                    {
                        string Email;
                        string Password;
                        if (Protocol.Get_Message_Parameters(Message).TryGetValue("email", out Email) && Protocol.Get_Message_Parameters(Message).TryGetValue("password", out Password))
                        {
                            if (Email and password are correct) //todo (yuval)
                                {
                                Add_Phone_By_Email(Protocol.Get_Sender_Id(Message), Email);
                            }
                        }
                    }
                    else
                    {
                        //todo (yaniv)
                    }
                }
            }
        }
    }

    private void Add_Phone_By_Id(string Id)
    {
        Phone_By_Id_Dict.Add(Id, new Phone(Id));
    }

    private void Add_Phone_By_Email(string Id, string Email) // needs the phone to be in the Phone_By_Id_Dict.
    {
        Phone Added_Phone;
        if (Phone_By_Id_Dict.TryGetValue(Id, out Added_Phone))
        {
            Added_Phone.Set_Email(Email);
            Phone_By_Email_Dict.Add(Email, Added_Phone);
        }
    }
}