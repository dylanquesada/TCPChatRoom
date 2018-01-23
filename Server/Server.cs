
﻿using System;
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
        public int counter = 0;
        public static Client client;
        TcpListener server;
        Ilogger logger;
        Dictionary<int, Client> users;
        Queue<string> messages;
        public Server(Ilogger logger)
        {
            
            users = new Dictionary<int, Client>();
            messages = new Queue<string>();
            this.logger = logger;
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }
        public void Run()
        {
            
            AcceptClient(); 
            
        }
        private void ChatClient(Client client)
        {
            while (true)
            {
                string message = client.Recieve();
                lock (message)
                {
                    messages.Enqueue(message);
                }
                if (messages.Count > 0)
                {
                    lock (messages)
                    {
                        Respond(messages.Dequeue());
                    }
                }
            }
        }
        private void AcceptClient()
        {
            while (true)
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                NetworkStream stream = clientSocket.GetStream();
                client = new Client(stream, clientSocket);
                users.Add(counter, client);
                counter++;
                Task.Run(() => ChatClient(client));
            }
        }
        private void Respond(string body)
        {
            logger.Log(body);
            foreach (KeyValuePair<int, Client> entry in users)
            {
                entry.Value.Send(body);
                
            }          
            
        }



    }
}


