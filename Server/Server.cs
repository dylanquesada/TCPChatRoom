using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static Client client;
        TcpListener server;
        Dictionary<int, string> users = new Dictionary<int, string>();
        Queue<string> messages = new Queue<string>();
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }
        public void Run()
        {

            AcceptClient();
            while (true)
            {
                string message = client.Recieve();
                messages.Enqueue(message);
                Respond(messages.Dequeue());
            }
        }
        private void AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket);
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
