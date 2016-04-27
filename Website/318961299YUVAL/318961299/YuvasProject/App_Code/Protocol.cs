using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// A static class with methods used for analyzing and creating messages.
/// </summary>
public static class Protocol
{
    public static string Cut_Len_From_Message(string Message, out int Len)
    {
        Len = int.Parse(Message.Split(':')[0]);
        return string.Join("", Message.Split(':').Skip(1));
    }

    public static string Add_Len_To_Message(string Message)
    {
        return (Message.Length - 1).ToString() + ":" + Message;
    }

    public static bool Is_Proxy_Message(string Message)
    {
        return Message.StartsWith("Rdroid PROXY\n");
    }

    public static bool IsPhoneMessage(string Message)
    {
        try
        {
            return Message.Split(':')[1].StartsWith("Rdroid CLIENT\n");
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }
    }

    /**
     * returns the id of the phone who sent the message.
     **/
    public static string Get_Sender_Id(string Message)
    {
        return Message.Split(':')[0];
    }

    public static string Get_Message_Purpose(string Message)
    {
        string[] Message_Lines = Message.Split('\n');
        return Message_Lines[1];
    }

    public static Dictionary<string, string> Get_Message_Parameters(string Message)
    {
        Dictionary<string, string> Parameter_Dict = new Dictionary<string, string>();
        string[] Parameter_Lines = Message.Split('\n').Skip(2).ToArray<string>();
        foreach (string Parameter in Parameter_Lines)
        {
            string[] Split_Parameter = Parameter.Split(new char[] { ':' }, 2);
            if (Split_Parameter.Length == 2)
            {
                Parameter_Dict.Add(Split_Parameter[0], Split_Parameter[1]);
            }
        }
        return Parameter_Dict;
    }

    public static string Create_Login_Result_Message(string Id, bool Successful)
    {
        string Result_String;
        if (Successful)
        {
            Result_String = "success";
        }
        else
        {
            Result_String = "failure";
        }
        string Message = Id + ":Rdroid SERVER\nLOGIN\n";
        Message += "result:" + Result_String + "\n";
        return Message;
    }

    public static string Create_Task_Message(string Phone_Id, string Task_Id, string Type, string[] Parameters)
    {
        string Message = Phone_Id + ":Rdroid SERVER\nTASK\n";
        Message += "id:" + Task_Id + "\n";
        Message += "type:" + Type + "\n";
        foreach (string Parameter in Parameters)
        {
            Message += "parameter:" + Parameter + "\n";
        }
        Message = Message.Substring(0, Message.LastIndexOf("\n"));
        return Message;
    }
}