using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Sockets;
using System.Net;
using System.Text;

/// <summary>
/// Summary description for Proxy
/// </summary>
public class Proxy
{
    private Socket clientsocket;
    private byte[] buffer = new byte[4096]; // The amount of data

    public Proxy()
    {
        TcpListener serverSocket = new TcpListener(IPAddress.Parse("0.0.0.0"), 9001);
        TcpClient clientSocket = default(TcpClient);
        serverSocket.Start();
        clientSocket = serverSocket.AcceptTcpClient();
        serverSocket.Stop();
        this.clientsocket = clientSocket.Client;
    }

    private byte[] Encode(string message) // Turning the message from string to byte[] to send it through the socket.
    {
        byte[] message2 = Encoding.UTF8.GetBytes(message); // Encoding the message
        return message2; // Returns the byte array.
    }

    public string Recv(int message) // Decodes the incoming message from byte[] to string.
    {
        string message2 = Encoding.ASCII.GetString(this.buffer, 0, message); // Decoding the message. 
        return message2; // Returns the string.
    }

    public void Send(string message) // Sends the message.
    {
        clientsocket.Send(Encode(message)); // Sends an encoded message.
    }
}