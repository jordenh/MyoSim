﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;


namespace MyoSimGUI
{


    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        public void ThreadStartServer()
        {
            // Create a name pipe
            using (NamedPipeServerStream pipeStream = new NamedPipeServerStream("BvrPipe"))
            {
                Console.WriteLine("[Server] Pipe created {0}", pipeStream.GetHashCode());

                // Wait for a connection
                pipeStream.WaitForConnection();
                Console.WriteLine("[Server] Pipe connection established");
            /*
                using (StreamReader sr = new StreamReader(pipeStream))
                {
                    string temp;
                    // We read a line from the pipe and print it together with the current time
                    while ((temp = sr.ReadLine()) != null)
                    {
                        Console.WriteLine("{0}: {1}", DateTime.Now, temp);
                    }
                }

            */
            }

       //     Console.WriteLine("Connection lost");
            
        }

        public void SendMessage()
        {
            // Create a name pipe
            using (NamedPipeServerStream pipeStream = new NamedPipeServerStream("BvrPipe"))
            {
                Console.WriteLine("[Server] Pipe created {0}", pipeStream.GetHashCode());

                // Wait for a connection
                pipeStream.WaitForConnection();
                Console.WriteLine("[Server] Pipe connection established");
/*
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    string text = "test message from C# Server!";
                    sw.Write(text);
                }
 */
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    Application.Run();//new Form1(pipeStream, sw));
                }
            }

         }

        static void Main(string[] args)
        {
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream(".", "MyPipe", PipeDirection.InOut))
            {
                pipeStream.Connect();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MyoSimulatorForm(pipeStream));

            }
         //   Program Server = new Program();
          //  Thread ServerThread = new Thread(Server.SendMessage);
         //   ServerThread.Start();

           

          
            
            

           
         //   Thread SendMessageThread = new Thread(Server.SendMessage);
         //   SendMessageThread.Start();
           

        }
    }
}
