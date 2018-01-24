using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        
        static void Main(string[] args)
        {            
            Client client = new Client("192.168.0.119", 9999);                             
            client.IntroduceClient(client.name);
            Parallel.Invoke(() =>
            {
                while (true)
                {
                    client.Send();                    
                }
            },
            () =>
            {
                while (true)
                {
                    client.Recieve();
                }
            });           
           
        }
    }
}
