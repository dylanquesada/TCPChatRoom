﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        public string name;
        public Client(string IP, int port)
        {
            name = "Brian";
            clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
        }
        public void Continue()
        {
            stream.Flush();
        }
        public void Send()
        {
            string messageString = name + ": " + UI.GetInput(); //string += username through a dictionary
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            stream.Write(message, 0, message.Count());
        }
        public void GetName()
        {
            Console.WriteLine("Please enter your name: ");
            name =  UI.GetInput();
        }
        public void IntroduceClient(string user)
        {
            string messageString = user + " has entered chat.";
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            stream.Write(message, 0, message.Count());
        }
        public void Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
        }
    }
}
