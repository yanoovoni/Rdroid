using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// A static class with methods used for analyzing and creating messages.
/// </summary>
public static class Protocol
{
    public static bool Is_Proxy_Message(string Message)
    {
        return Message.StartsWith("Rdroid PROXY\n");
    }

    public static bool Is_Phone_Message(string Message)
    {
        return Message.Split(':')[1].StartsWith("Rdroid CLIENT");
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
            string[] Split_Parameter = Parameter.Split(':');
            Parameter_Dict.Add(Split_Parameter[0], Split_Parameter[1]);
        }
        return Parameter_Dict;
    }
}