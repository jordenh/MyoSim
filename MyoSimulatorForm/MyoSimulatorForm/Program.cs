using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.ComponentModel;


namespace MyoSimGUI
{
    class Program
    {
        private static MyoSimulatorForm form;

        private static void getConnection(object sender, DoWorkEventArgs e)
        {
            NamedPipeServerStream pipeStream = (NamedPipeServerStream) e.Argument;
            // Wait for a connection
            pipeStream.WaitForConnection();
            Console.WriteLine("[Server] Pipe connection established");
        }

        private static void foundConnection(object sender, RunWorkerCompletedEventArgs e)
        {
            form.enableSendCommand();
        }

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (NamedPipeServerStream pipeStream = new NamedPipeServerStream("BvrPipe", PipeDirection.InOut))
            {
                Console.WriteLine("[Server] Pipe created {0}", pipeStream.GetHashCode());
                form = new MyoSimulatorForm(pipeStream);
                BackgroundWorker pipeWorker = new BackgroundWorker();
                pipeWorker.DoWork += new DoWorkEventHandler(getConnection);
                pipeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(foundConnection);
                pipeWorker.RunWorkerAsync(pipeStream);
                Application.Run(form);
            }
        }
    }
}
