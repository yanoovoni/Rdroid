﻿using System;
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
    private int BufferSize = 8192; // The amount of data
    private Queue<char[]> input_queue = new Queue<char[]>();
    private Queue<string> output_queue = new Queue<string>();
    private Thread Start_Thread;
    private Thread Recv_Thread;
    private Thread Send_Thread;
    private Mutex Connect_Mutex = new Mutex();

    public static ProxySocketInterface Get_Instance()
    {
        return our_instance;
    }

    private ProxySocketInterface()
    {
        this.Start_Thread = new Thread(new ThreadStart(this.StartThreadMethod));
        Start_Thread.IsBackground = true;
        Start_Thread.Start();
    }

    private void StartThreadMethod()
    {
        Connect();
        RunThreads();
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

    private void RunThreads()
    {
        this.Recv_Thread = new Thread(new ThreadStart(this.RecvThreadMethod));
        Recv_Thread.IsBackground = true;
        Recv_Thread.Start();
        this.Send_Thread = new Thread(new ThreadStart(this.SendThreadMethod));
        Send_Thread.IsBackground = true;
        Send_Thread.Start();
    }

    private byte[] Encode(string message) // Turning the message from string to byte[] to send it through the socket.
    {
        byte[] encoded_message = Encoding.UTF8.GetBytes(message); // Encoding the message
        return encoded_message; // Returns the byte array.
    }

    private void RecvThreadMethod() // Decodes the incoming message from byte[] to string.
    {
        byte[] OneLetterBuffer = new byte[1];
        while (true)
        {
            try
            {
                int MessageLen = 0;
                string MessageLenString = "";
                while (!MessageLenString.EndsWith(":"))
                {
                    clientsocket.Receive(OneLetterBuffer);
                    MessageLenString += Encoding.UTF8.GetString(OneLetterBuffer, 0, OneLetterBuffer.Length).TrimEnd('\0');
                }
                char[] MessageLenFlippedCharArray = MessageLenString.Trim(':').Reverse().ToArray();
                for (int i = 0; i < MessageLenFlippedCharArray.Length; i++)
                {
                    char ShouldBeNumberChar = MessageLenFlippedCharArray[i];
                    int RecievedNumber;
                    if (int.TryParse(ShouldBeNumberChar.ToString(), out RecievedNumber))
                    {
                        MessageLen += Convert.ToInt32(RecievedNumber * Math.Pow(10, Convert.ToDouble(i)));
                    }
                    else
                    {
                        break;
                    }
                }
                string RecievedMessage = "";
                while (RecievedMessage.Length < MessageLen)
                {
                    int NextBufferSize = MessageLen - RecievedMessage.Length;
                    if (NextBufferSize > this.BufferSize)
                    {
                        NextBufferSize = this.BufferSize;
                    }
                    byte[] Buffer = new byte[NextBufferSize];
                    clientsocket.Receive(Buffer);
                    RecievedMessage += Encoding.UTF8.GetString(Buffer, 0, Buffer.Length).TrimEnd('\0');
                }
                this.input_queue.Enqueue(Base64Decode(RecievedMessage.TrimEnd('\n'))); // Returns the string.
            }
            catch (SocketException e)
            {
                Connect();
            }
        }
    }

    private void SendThreadMethod() // Sends the message.
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
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        catch (Exception e)
        {
            return plainText;
        }
    }

    private static char[] Base64Decode(string base64EncodedData)
    {
        try
        {
            byte[] data = Convert.FromBase64String(base64EncodedData);
            char[] CharData = new char[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                CharData[i] = (char)data[i];
            }
            return CharData;
        }
        catch (Exception e)
        {
            return base64EncodedData.ToCharArray();
        }
    }

    public char[] Recv()
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