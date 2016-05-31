using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// The Phone object represents a phone that is connected to the server.
/// </summary>
public class Phone
{
    private string Id;
    private string User_Email = "";
    private int Task_Id_Generator_Number = 0;
    private Dictionary<string, char[]> Recieved_Tasks_Dict = new Dictionary<string, char[]>();

    public Phone(string Id)
    {
        this.Id = Id;
    }

    public bool IsLoggedIn()
    {
        return User_Email != "";
    }

    public string GetId()
    {
        return Id;
    }

    public string GetEmail()
    {
        return User_Email;
    }

    public void SetEmail(string email)
    {
        User_Email = email;
    }

    public void Send(string message)
    {
        ProxySocketInterface.Get_Instance().Send(message);
    }

    public string GenerateTaskId()
    {
        Task_Id_Generator_Number++;
        return Task_Id_Generator_Number.ToString();
    }

    public void AddRecievedTask(string Task_Id, char[] Task_Output)
    {
        Recieved_Tasks_Dict.Add(Task_Id, Task_Output);
    }

    public char[] GetRecievedTaskOutput(string Task_Id)
    {
        char[] Recieved_Task;
        if (Recieved_Tasks_Dict.TryGetValue(Task_Id, out Recieved_Task))
        {
            Recieved_Tasks_Dict.Remove(Task_Id);
            return Recieved_Task;
        }
        return null;
    }
}