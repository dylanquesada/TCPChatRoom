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
        //public bool stop = false;
        
        public static Client client;
        public string ServerIP;
        public int port;
        
        public TcpListener server;
        Ilogger logger;
        public Dictionary<IMember, Client> users;
        Queue<string> messages;
        public Server(Ilogger logger)
        {
            ServerIP = "127.0.0.1";
            port = 9999;
            users = new Dictionary<IMember, Client>();
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
            bool FirstTime = true;
            while (!client.stop)
            {
                try
                {
                    string message = client.Recieve();
                    lock (message)
                    {
                        if (FirstTime) {
                            SetName(message, client);
                            FirstTime = false;
                        }
                        
                       
                        messages.Enqueue(message);
                    }
                    if (messages.Count > 0)
                    {
                        lock (messages)
                        {
                            logger.Log(messages.Peek());
                            if (CheckForDirectMessage(messages.Peek()))
                            {
                                string directMessage = messages.Dequeue();
                                RespondDirectMessage(directMessage);
                            }
                            else
                            {
                                Respond(messages.Dequeue());
                            }                                
                        }
                    }
                }
                catch (Exception)
                {
                    string exit = client.name + " has left.";
                    users.Remove(client);
                    Respond(exit);
                    logger.Log(exit);
                    Console.WriteLine(exit);
                    client.stop = true;
                    
                }
            }
        }
        public bool CheckForDirectMessage(string message)
        {
            bool DM = false;
            foreach (KeyValuePair<IMember, Client> entry in users)
            {
                if (message.Contains("@"))
                {
                    if (message.Substring(message.IndexOf('@') + 1, entry.Value.name.Length).Equals(entry.Value.name))
                    {
                        DM = true;
                    }
                }
            }
            return DM;
        }
        public void SetName(string message, Client client)
        {
            client.name = message.Substring(0, message.IndexOf(" "));                        
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
                users.Add(client, client);
                Task.Run(() => ChatClient(client));                
            }
        }
        private void Respond(string body)
        {            
            foreach (KeyValuePair<IMember, Client> entry in users)
            {
                entry.Value.Notify(body);                
            }          
            
        }
        private void RespondDirectMessage(string message)
        {
            foreach(KeyValuePair<IMember, Client> entry in users)
            {
                if (message.Substring(message.IndexOf('@') + 1, entry.Value.name.Length).Equals(entry.Value.name))
                { 
                    entry.Value.RelayDirectMessage(message);
                }
            }
        }


        



}
}


