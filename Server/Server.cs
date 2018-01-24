
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
        public int counter;
        public static Client client;
        public string ServerIP;
        public int port;
        TcpListener server;
        Ilogger logger;
        Dictionary<int, Client> users;
        Queue<string> messages;
        public Server(Ilogger logger)
        {
            ServerIP = "192.168.0.119";
            port = 9999;
            counter = 0;
            users = new Dictionary<int, Client>();
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
                        logger.Log(messages.Peek());
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
            foreach (KeyValuePair<int, Client> entry in users)
            {
                entry.Value.Send(body);
                
            }          
            
        }



    }
}


