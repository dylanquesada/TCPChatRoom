
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client : IMember
    {
        NetworkStream stream;
        TcpClient client;
        public string UserId;
        public string name;
        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            UserId = "495933b6-1762-47a1-b655-483510072e73";
        }
        public void Send(string Message)
        {

            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
            
            
        }
        public string Recieve()
        {
            try
            {
                byte[] recievedMessage = new byte[256];
                stream.Read(recievedMessage, 0, recievedMessage.Length);
                string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
                Console.WriteLine(recievedMessageString);
                return recievedMessageString;
            }
            catch (Exception)
            {
                string exit = "Left Recieve";
                Console.WriteLine(exit);
                return exit;
            }

        }
        public void Exit()
        {
            //users.Remove(client);
        }
        public void Join(Client client)
        {
            //users.Add(client, client);
        }
        public void Notify()
        {

        }

    }
}
