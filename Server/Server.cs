﻿
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
        public bool stop = false;
        public bool NotFirstTime;
        public static Client client;
        public string ServerIP;
        public int port;
        
        public TcpListener server;
        Ilogger logger;
        public Dictionary<Client, Client> users;
        Queue<string> messages;
        public Server(Ilogger logger)
        {
            ServerIP = "192.168.0.119";
            port = 9999;
            users = new Dictionary<Client, Client>();
            messages = new Queue<string>();
            this.logger = logger;
            server = new TcpListener(IPAddress.Parse(ServerIP), port);
            server.Start();
            
        }
        public void Run()
        {       
            AcceptClient();            
        }
        private void ChatClient(Client client)
        {
            NotFirstTime = false;
            while (!stop)
            {
                try
                {
                    string message = client.Recieve();
                    lock (message)
                    {
                        if (NotFirstTime) {
                            string Name = message.Substring(0, message.IndexOf(":"));
                            client.name = Name;
                        }
                        
                        NotFirstTime = true;
                        messages.Enqueue(message);
                    }
                    if (messages.Count > 0)
                    {
                        lock (messages)
                        {
                            logger.Log(messages.Peek());
                            Respond(messages.Dequeue());
                        }
                    }
                }
                catch (Exception)
                {
                    string exit = "Left SEND";
                    Console.WriteLine(exit);
                    stop = true;
                    client.Exit();
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
                client.Join(client);
                users.Add(client, client);
                Task.Run(() => ChatClient(client));                
            }
        }
        private void Respond(string body)
        {            
            foreach (KeyValuePair<Client, Client> entry in users)
            {
                entry.Value.Send(body);                
            }          
            
        }



    }
}


