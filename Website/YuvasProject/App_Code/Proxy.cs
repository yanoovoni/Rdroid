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
    private Dictionary<string, Phone> Phone_Dict;
    private Queue<string> Input_Queue;

    public static Proxy Get_Instance()
    {
        return our_instance;
    }

    private Proxy()
    {
        Proxy_Socket = ProxySocketInterface.Get_Instance();
        Phone_Dict = new Dictionary<string, Phone>();
        Input_Queue = new Queue<string>();
    }

    private void Proxy_Manager_Thread_Method()
    {
        while (true)
        {
            string Message = Proxy_Socket.Recv();
            if (Protocol.Is_Proxy_Message(Message))
            {
                string Id;
                string Purpose = Protocol.Get_Message_Purpose(Message);
                switch (Purpose)
                {
                    case "NOTIFY_SESSION_ID":

                        if (Protocol.Get_Message_Parameters(Message).TryGetValue("session_id", out Id))
                        {
                            Add_Phone(Id);
                        }
                        break;
                    case "NOTIFY_SESSION_DISCONNECT":
                        if (Protocol.Get_Message_Parameters(Message).TryGetValue("session_id", out Id))
                        {
                            Phone_Dict.Remove(Id);
                        }
                        break;
                }
            }
            else
            {
                if (Protocol.Is_Phone_Message(Message))
                {
                    //todo do stuff with the phone message
                }
            }
        }
    }

    private void Add_Phone(string Id)
    {
        Phone_Dict.Add(Id, new Phone(Id));
    }
}