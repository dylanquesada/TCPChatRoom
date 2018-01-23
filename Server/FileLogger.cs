using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FileLogger : Ilogger
    {
        public FileLogger(string fileLocation)
        {

        }
        public void Log(string message)
        {
            string path = @"C:\Users\dques\Documents\Visual Studio 2015\Projects\TCPChatRoom\ChatLog.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(message);

                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(message);
                }
            }
        }

    }
}
