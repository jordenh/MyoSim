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
        private NamedPipeServerStream pipeStream;

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

        public MyoSimulatorForm(NamedPipeServerStream pipeStream)
        {
            this.pipeStream = pipeStream;
            InitializeComponent();
            ComHandShake();
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

            try
            {
                file = new System.IO.StreamReader(filename);
            }
            catch (ArgumentException except)
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
            try
            {

                foreach (string word in words)
                {
                    System.Console.WriteLine(word);

                    if (!pipeStream.IsConnected)
                    {
                        System.Console.WriteLine("Failed to connect!!");
                        return;
                    }

                    System.Console.WriteLine("Connected!!");

                    //pipeStream.Write(Encoding.ASCII.GetBytes(word), 0, word.Length);
                    ComProtocolSend(Encoding.ASCII.GetBytes(word));


                    System.Console.WriteLine("Message Sent!!");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), string.Format("Named pipe server disconnected, please save your commands and start the GUI and the server"));
                this.sendCommandButton.Enabled = false;

            }

        }


        private void addGestureButton_Click(object sender, EventArgs e)
        {
            sendCommand(this.gestureList.SelectedItem.ToString(), labelToCommand);
        }
        /*
                Application.ThreadException += new ThreadExceptionEventHandler(CatchNamedPipeDisconnect);
                private void CatchNamedPipeDisconnect(object sender, ThreadExceptionEventArgs t)
                {
                   this.sendCommandButton.Enabled = false;
                    MessageBox.Show(t.ToString(), string.Format("Named Pipe disconnected"));
                }
         */

        //this function restruture the data into the sending format(as per communcation protocol design) and sends it to the myo_hub via named pipe
        public void ComProtocolSend(byte[] temp)
        {
            //shift 3 bytes over for the com protocol
            byte[] Data = new byte[43];
            for (int x = 0; x < temp.Length; x++)
            {
                Data[x + 3] = temp[x];
            }
            // get the info on what kind of data is sending out
            var data_type = (temp[0] >> 7 & 0xff);

            int pos = 1;
            if (data_type == 0)
            {
                
                for (int y = 0; y < Encoding.ASCII.GetBytes("onOrientationData").Length; y++)
                {
                    Data[pos] = Encoding.ASCII.GetBytes("onOrientationData")[y];
                    pos++;
                }
            }
            else
            {
                
                for (int y = 0; y < Encoding.ASCII.GetBytes("onPose").Length; y++)
                {
                    Data[pos] = Encoding.ASCII.GetBytes("onPose")[y];
                    pos++;
                }
            }
            //get the data segment length(-4 bytes of time stamp)
            Data[0] = Encoding.ASCII.GetBytes((temp.Length-4).ToString())[0];

            pipeStream.Write(Data, 0, Data.Length);
        }
       
       

        //This function sends the three handshake calls to the myo_hub 
        public void ComHandShake()
        {
            String OnAttach = "onAttach";
            String OnConnect = "onConnect";
            String OnArmSync = "onArmSync";

            byte[] data = new byte[43];
           

            data[0] = Encoding.ASCII.GetBytes("0")[0];
     
            int count = 0;

            String StringData = OnAttach;

            while (count < 3)
            {
                int pos = 1;
                for (int y = 0; y < Encoding.ASCII.GetBytes(StringData).Length; y++)
                {
                    data[pos] = Encoding.ASCII.GetBytes(StringData)[y];
                    pos++;
                }

                pipeStream.Write(data, 0, data.Length);
                count++;
                
                if(count == 1)
                {
                    StringData = OnConnect;
                }
                else if(count == 2)
                {
                    StringData = OnArmSync;
                }
                else
                {
                    count = 3;
                }
            }

                                

        }
    }
}
