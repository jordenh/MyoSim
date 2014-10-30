using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;


namespace WindowsFormsApplication1
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

                using (StreamReader sr = new StreamReader(pipeStream))
                {
                    string temp;
                    // We read a line from the pipe and print it together with the current time
                    while ((temp = sr.ReadLine()) != null)
                    {
                        Console.WriteLine("{0}: {1}", DateTime.Now, temp);
                    }
                }
            }

            Console.WriteLine("Connection lost");
        }

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
            Program Server = new Program();
            Thread ServerThread = new Thread(Server.ThreadStartServer);
            ServerThread.Start();
           

        }
    }
}
