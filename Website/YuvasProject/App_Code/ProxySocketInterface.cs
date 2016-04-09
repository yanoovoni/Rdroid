using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

/// <summary>
/// Summary description for ProxySocketInterface
/// </summary>
public sealed class ProxySocketInterface
{
    private static readonly ProxySocketInterface our_instance = new ProxySocketInterface();
    private Socket clientsocket;
    private byte[] buffer = new byte[8192]; // The amount of data
    private Queue<string> input_queue = new Queue<string>();
    private Queue<string> output_queue = new Queue<string>();
    private Thread Recv_Thread;
    private Thread Send_Thread;
    private Mutex Connect_Mutex = new Mutex();

    public static ProxySocketInterface Get_Instance()
    {
        return our_instance;
    }

    private ProxySocketInterface()
    {
        Connect();
        Run_Threads();
    }

    private void Connect()
    {
        lock (this)
        {
            if (this.clientsocket == null || !this.clientsocket.Connected)
            {
                TcpListener serverSocket = new TcpListener(IPAddress.Parse("0.0.0.0"), 9001);
                TcpClient clientSocket = default(TcpClient);
                serverSocket.Start();
                clientSocket = serverSocket.AcceptTcpClient();
                serverSocket.Stop();
                this.clientsocket = clientSocket.Client;
            }
        }
    }

    private void Run_Threads()
    {
        this.Recv_Thread = new Thread(new ThreadStart(this.Recv_Thread_Method));
        Recv_Thread.Start();
        this.Send_Thread = new Thread(new ThreadStart(this.Send_Thread_Method));
        Send_Thread.Start();
    }

    private byte[] Encode(string message) // Turning the message from string to byte[] to send it through the socket.
    {
        byte[] encoded_message = Encoding.UTF8.GetBytes(message); // Encoding the message
        return encoded_message; // Returns the byte array.
    }

    private void Recv_Thread_Method() // Decodes the incoming message from byte[] to string.
    {
        while (true)
        {
            try
            {
                clientsocket.Receive(buffer);
                string recieved_message = Encoding.ASCII.GetString(buffer, 0, buffer.Length).TrimEnd('\0'); // Decoding the message.
                if (!String.IsNullOrEmpty(recieved_message))
                {
                    int Len;
                    recieved_message = Protocol.Cut_Len_From_Message(recieved_message, out Len);
                    while (recieved_message.Length < Len)
                    {
                        clientsocket.Receive(buffer);
                        recieved_message += Encoding.ASCII.GetString(buffer, 0, buffer.Length).TrimEnd('\0');
                    }
                    this.input_queue.Enqueue(Base64Decode(recieved_message)); // Returns the string.
                }
            }
            catch (SocketException e)
            {
                Connect();
            }
        }
    }

    private void Send_Thread_Method() // Sends the message.
    {
        while (true)
        {
            try
            {
                if (this.output_queue.Count > 0)
                {
                    string Message = Base64Encode(this.output_queue.First());
                    Message = Protocol.Add_Len_To_Message(Message);
                    clientsocket.Send(Encode(Message)); // Sends an encoded message.
                    this.output_queue.Dequeue();
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            catch (SocketException e)
            {
                Connect();
            }
        }
    }

    private static string Base64Encode(string plainText)
    {
        try
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        catch (Exception e)
        {
            return plainText;
        }
    }

    private static string Base64Decode(string base64EncodedData)
    {
        try
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        catch (Exception e)
        {
            return base64EncodedData;
        }
    }

    public String Recv()
    {
        while (true)
        {
            if (this.input_queue.Count != 0)
            {
                return input_queue.Dequeue();
            }
            else
            {
                Thread.Sleep(100);
            }
        }
    }

    public void Send(string message)
    {
        this.output_queue.Enqueue(message);
    }
}