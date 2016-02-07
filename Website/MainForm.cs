using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace Small_Project_GUI
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        private Socket clientsocket;
        private byte[] buffer = new byte[4096]; // The amount of data

        public MainForm(TcpClient clientsocket)
        {
            this.clientsocket = clientsocket.Client;
            InitializeComponent();
        }

        public byte[] Encode(string message) // Turning the message from string to byte[] to send it through the socket.
        {
            byte[] message2 = Encoding.UTF8.GetBytes(message); // Encoding the message
            return message2; // Returns the byte array.
        }
        public string Decode(int message) // Decodes the incoming message from byte[] to string.
        {
            string message2 = Encoding.ASCII.GetString(this.buffer, 0, message); // Decoding the message. 
            return message2; // Returns the string.
        }
        public void Send(string message) // Sends the message.
        {
            clientsocket.Send(Encode(message)); // Sends an encoded message.
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            this.Send(this.CommandTextBox.Text);
            this.OutputBox.Text = this.Decode(this.clientsocket.Receive(buffer));
        }
    }
}
