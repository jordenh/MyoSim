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

namespace MyoSimGUI
{
    public partial class MyoSimulatorForm : Form
    {
        public const char commandDelimiter = ';';
        private static int ms_btw_send = 10;
        private NamedPipeServerStream pipeStream;
        private Dictionary<int, int[]> bin_command_list = new Dictionary<int, int[]>();

        static Dictionary<string, string> labelToCommand = new Dictionary<string, string>
        {
            {"Rest", "rest"},
            {"Fist", "fist"},
            {"Wave In", "waveIn"},
            {"Wave Out", "waveOut"},
            {"Fingers Spread", "fingersSpread"},
            {"Reserved 1", "reserved1"},
            {"Thumb to Pinky", "thumbToPinky"},
            {"Unknown", "unknown"}
        };
        
        private unsafe int Parseline(string[] lines, int* current_time)
        {
            int len = -1;
            int delta_time;
            int x;
            int y;
            char command_delim = ' ';
            int[] current_orient_command = new int[11];
            byte[] current_async_command = new byte[7];


            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i].Trim();
                string[] command = lines[i].Split(command_delim);

                Console.Write(command);
                switch (command.First())
                {
                    case "move":
                        /* Check for invalid inputs */
                        if (command.Length != 4 ||
                            !(int.TryParse(command.Last(), out delta_time)) ||
                            delta_time < 0 ||
                            !(int.TryParse(command[1], out x)) ||
                            !(int.TryParse(command[2], out y)) )
                        {

                        }
                        else
                        {
                            /* Set first bit to be 1 to signal orientation data */
                            current_orient_command[0] |= ( 1 << (sizeof(int)*8) );
                            
                        }

                        break;
                    case "delay":
                        break;
                    case "async":
                        break;
                    case "expect":
                        break;
                    default:
                        break;
                }
            }
            return len;
        }

        /* 
         * Read the human readable input file and translate that into the byte format for sending
         * Parameter:   filename  - path and filename containing the input text
         * Return:      if success  - Number of bytes generated
         *              if fail     - -1
         */
        private int ReadInputFile(string filename)
        {
            int len = -1;
            int current_time = 0;
            string[] lines = System.IO.File.ReadAllLines(@filename);

            char[] delim = {'(', ')'};
            string[] temp = lines[0].Split(delim);
            for (int i = 0; i < temp.Length; ++i)
            {

                Console.WriteLine("size " + temp.Length + " " + temp[i]);
            }
            unsafe
            {
                Parseline(lines, &current_time);
            }
            
            for (int i = 0; i < lines.Length; ++i)
            {
                Console.WriteLine(lines[i]);
            }

            return len;
        }

        public MyoSimulatorForm(NamedPipeServerStream pipeStream)
        {
            this.pipeStream = pipeStream;
            InitializeComponent();
            this.sendCommandButton.Enabled = false;
            foreach (string key in labelToCommand.Keys) 
            {
                this.gestureList.Items.Add(key);
            }
        }

        public void enableSendCommand()
        {
            this.sendCommandButton.Enabled = true;
        }

        private void sendCommand(string label, Dictionary<string, string> labelToCommandMap)
        {
            string command;
            if (labelToCommandMap.TryGetValue(label, out command))
            {
                commandChain.Text = string.Concat(commandChain.Text, command + commandDelimiter);
            }
        }

        private void call_myo_sim(string command)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string filename = saveFilename.Text;
            string command = commandChain.Text;
            System.IO.StreamWriter file = null;

            try
            {
                file = new System.IO.StreamWriter(filename);
            }
            catch (ArgumentException except)
            {
                MessageBox.Show(except.ToString(), string.Format("File not loaded."));
                return;
            }

            file.WriteLine(command);
            file.Close();
        }

        private void loadFileButton_Click(object sender, EventArgs e)
        {
            string filename = saveFilename.Text;
            string command;
            System.IO.StreamReader file = null;
            
            ReadInputFile(@"C:\Users\Frederick\Source\Repos\MyoSim\MyoSimulatorForm\test_human_readable_input.txt");

            try
            {
                file = new System.IO.StreamReader(filename);
            }
            catch(ArgumentException except)
            {
               MessageBox.Show(except.ToString(), string.Format("File does not exist"));
            }
            catch (FileNotFoundException except)
            {
                MessageBox.Show(except.ToString(), string.Format("File not found"));
            }
            finally
            {
                if (file != null)
                {
                    command = file.ReadLine();
                    file.Close();
                    commandChain.Text = command;
                }
            }         
        }

        private void sendCommandButton_Click(object sender, EventArgs e)
        {
            string command = commandChain.Text;

            if (command[command.Length - 1] == commandDelimiter)
            {
                command = command.Remove(command.Length - 1);
            }

            System.Console.WriteLine("String to send: " + command);
            string[] words = command.Split(commandDelimiter);

            foreach (string word in words)
            {
                System.Console.WriteLine(word);

                if (!pipeStream.IsConnected)
                {
                    System.Console.WriteLine("Failed to connect!!");
                    return;
                }

                System.Console.WriteLine("Connected!!");

                pipeStream.Write(Encoding.ASCII.GetBytes(word), 0, word.Length);

                System.Console.WriteLine("Message Sent!!");
            }
        }

        private void addGestureButton_Click(object sender, EventArgs e)
        {
            sendCommand(this.gestureList.SelectedItem.ToString(), labelToCommand);
        }

        private void MyoSimulatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO: send a disconnect event instead once we get protocol sorted
            string dc_signal = "DCed";
            if (pipeStream.IsConnected)
            {
                pipeStream.Write(Encoding.ASCII.GetBytes(dc_signal), 0, dc_signal.Length);
            }
            Dispose();
        }
    }
}
