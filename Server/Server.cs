
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
        Dictionary<int, Client> users = new Dictionary<int, Client>();
        Queue<string> messages = new Queue<string>();
        public Server(Ilogger logger)
        {
            this.logger = logger;
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }
        public void Run()
        {            
                Parallel.Invoke(() =>
                {
                    while (true)
                    {
                        AcceptClient();
                    }
                },
                () =>
                {
                    while (true)
                    {
                        if (users.Count > 0)
                        {
                            for (int i = 0; i < users.Count; i++)
                            {
                                string message = users[i].Recieve();
                                lock (message)
                                {
                                    messages.Enqueue(message);
                                }
                            }
                        }
                    }

                },
                () =>
                {
                    while (true)
                    {
                        if (messages.Count > 0)
                        {
                            lock (messages)
                            {
                                Respond(messages.Dequeue());
                            }
                        }
                    }
            });            
        }
        private void AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket);
            users.Add(counter, client);
            counter++;
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}


