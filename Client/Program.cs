﻿using System;
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
            
            Client client = new Client("127.0.0.1", 9999);
           
                        
           // client.IntroduceClient(client.name);
            
            //client.Continue();
            Parallel.Invoke(() =>
            {
            //    while (true)
            //    {
                    client.Send();
                    
                    //client.Continue();                
                    
                    //Console.ReadLine();
            //    }
            },
            () =>
            {
            //    while (true)
            //    {
                    client.Recieve();

            //    }
            });
            Console.ReadLine();
            
           
        }
    }
}