using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace Small_Project_GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = @"/C C:\Python27\python.exe C:\SmallProject\Client\SmallProjectEngine\Main.py";
            p.Start(); // Starts the python program
            TcpListener serverSocket = new TcpListener(IPAddress.Parse("0.0.0.0"), 5555);
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            clientSocket = serverSocket.AcceptTcpClient();
            serverSocket.Stop();
            Application.Run(new MainForm(clientSocket)); // Runs the form.
        }

    }
}
