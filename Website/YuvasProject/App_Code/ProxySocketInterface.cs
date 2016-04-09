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
    private byte[] buffer = new byte[4096]; // The amount of data
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
            if (!this.clientsocket.Connected)
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
                string recieved_message = Encoding.ASCII.GetString(this.buffer, 0, 4096); // Decoding the message.
                if (!recieved_message.Equals(""))
                {
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
                clientsocket.Send(Encode(Base64Encode(this.output_queue.First()))); // Sends an encoded message.
            }
            catch (SocketException e)
            {
                Connect();
            }
            finally
            {
                this.output_queue.Dequeue();
            }
        }
    }

    private static string Base64Encode(string plainText)
    {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
    }

    private static string Base64Decode(string base64EncodedData)
    {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public String Recv()
    {
        if (this.input_queue.Count != 0)
        {
            return input_queue.Dequeue();
        }
        else
        {
            return "";
        }
    }

    public void Send(string message)
    {
        this.output_queue.Enqueue(message);
    }
}