using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        NamedPipeServerStream pipeStream1;
        StreamWriter sw1;

        public Form1(NamedPipeServerStream pipeStream, StreamWriter sw)
        {
            pipeStream1 = pipeStream;
            sw1 = sw;
            InitializeComponent();
        }

        private void call_myo_sim(string command)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rest_button_Click(object sender, EventArgs e)
        {
            command_chain.Text = string.Concat(command_chain.Text, "rest;");
        }

        private void fist_button_Click(object sender, EventArgs e)
        {
            command_chain.Text = string.Concat(command_chain.Text, "fist;");
        }

        private void waveIn_button_Click(object sender, EventArgs e)
        {
            command_chain.Text = string.Concat(command_chain.Text, "waveIn;");
        }

        private void waveOut_button_Click(object sender, EventArgs e)
        {
            command_chain.Text = string.Concat(command_chain.Text, "waveOut;");
        }

        private void fingersSpread_button_Click(object sender, EventArgs e)
        {
            command_chain.Text = string.Concat(command_chain.Text, "fingersSpread;");
        }

        private void reserved1_button_Click(object sender, EventArgs e)
        {
            command_chain.Text = string.Concat(command_chain.Text, "reserved1;");
        }

        private void thumbToPinky_button_Click(object sender, EventArgs e)
        {
            command_chain.Text = string.Concat(command_chain.Text, "thumbToPinky;");
        }

        private void unknown_button_Click(object sender, EventArgs e)
        {
            command_chain.Text = string.Concat(command_chain.Text, "unknown;");
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            string filename = save_filename.Text;
            string command = command_chain.Text;

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.WriteLine(command);
            file.Close();
        }

        private void load_file_button_Click(object sender, EventArgs e)
        {
            string filename = save_filename.Text;
            string command;
            System.IO.StreamReader file = null;

            try
            {
                file = new System.IO.StreamReader(filename);
            }
            catch(ArgumentException except)
            {
               MessageBox.Show(except.ToString(), string.Format("File does not exist"));
            }
            finally
            {
                if (file != null)
                {
                    command = file.ReadLine();
                    file.Close();
                    command_chain.Text = command;
                }
            }         
        }

        private void send_command_button_Click(object sender, EventArgs e)
        {
            char deliminator = ';';
            string command = command_chain.Text;
            string[] words = command.Split(deliminator);
          

          /*  Form1 Server = new Form1(); //named pipe
            Thread MessageThread = new Thread(Server.ThreadStartServer);
            MessageThread.Start();
          */
            foreach (string word in words)
            {
                System.Console.WriteLine(word);
               
                    sw1.Write(word);
            
            }
        }

        /*
        public void ThreadStartServer()
        {
            // Create a name pipe
            using (NamedPipeServerStream pipeStream = new NamedPipeServerStream("BvrPipe"))
            {
                Console.WriteLine("[Server] Pipe created {0}", pipeStream.GetHashCode());
                // Wait for a connection
                pipeStream.WaitForConnection();
                Console.WriteLine("[Server] Pipe connection established");
            }
        }
        */
    }
}
