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
            ComHandShake(0);
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
                   
                    // binary data for waveIn at timestamp 0
                    byte[] test_pose = new byte[14];
                    test_pose[0] = 0x0;
                    test_pose[1] = 0x0;
                    test_pose[2] = 0x0;
                    test_pose[3] = 0x0;
                  
                    for (int x = 0; x < Encoding.ASCII.GetBytes("waveIn").Length; x++)
                    {
                        test_pose[x + 4] = Encoding.ASCII.GetBytes("waveIn")[x];
                    }
                    int size = Encoding.ASCII.GetBytes("waveIn").Length;

                    //binary data for orientation at time stamp 25
                    byte[] test_orientation = new byte[14];
                    test_orientation[0] = 0x80;
                    test_orientation[1] = 0x00;
                    test_orientation[2] = 0x00;
                    test_orientation[3] = 0x19;
                    for(int x = 0; x< Encoding.ASCII.GetBytes("xx").Length; x++)
                    {

                    }
                    test_orientation[4] = (byte)10.1;
                    test_orientation[5] = (byte)10.1;
                    test_orientation[6] = (byte)10.1;
                    test_orientation[7] = (byte)10.1;
                    test_orientation[8] = (byte)25.2;
                    test_orientation[9] = (byte)25.2;
                    test_orientation[10] = (byte)25.2;
                    test_orientation[11] = (byte)50.3;
                    test_orientation[12] = (byte)50.3;
                    test_orientation[13] = (byte)50.3;


                    //ComProtocolSend(test_pose, size);
                    ComProtocolSend(test_orientation, 10);

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
        public void ComProtocolSend(byte[] temp, int data_size)
        {
            //shift 3 bytes over for the com protocol
            byte[] Data = new byte[17];
            for (int x = 0; x < temp.Length; x++)
            {
                Data[x + 3] = temp[x];
            }
            // get the info on what kind of data is sending out
            var data_type = (temp[0] >> 7 & 0xff);

            int pos = 1;
            if (data_type == 1)
            // libmyo_event_orientation 	= 6, ///< Orientation data has been received.
            {
                
                for (int y = 0; y < Encoding.ASCII.GetBytes("6").Length; y++)
                {
                    Data[pos] = Encoding.ASCII.GetBytes("6")[y];
                    pos++;
                }
            }
            else
            //    libmyo_event_pose 			= 7, ///< A change in pose has been detected. @see libmyo_pose_t.
            {
                
                for (int y = 0; y < Encoding.ASCII.GetBytes("7").Length; y++)
                {
                    Data[pos] = Encoding.ASCII.GetBytes("7")[y];
                    pos++;
                }
            }
            //get the data segment length(-4 bytes of time stamp)

            Data[0] = (byte) data_size;

            pipeStream.Write(Data, 0, Data.Length);
        }
       
       

        //This function sends the three handshake calls to the myo_hub 
        //if status = 0, start connection, if status = 1 end connection
        public void ComHandShake( int status)
        {
            /*
           		libmyo_event_paired 		= 0, ///< Successfully paired with a Myo.
                libmyo_event_unpaired 		= 1, ///< Successfully unpaired from a Myo.
                libmyo_event_connected 		= 2, ///< A Myo has successfully connected.
                libmyo_event_disconnected 	= 3, ///< A Myo has been disconnected.
                libmyo_event_arm_recognized = 4, ///< A Myo has recognized that it is now on an arm.
                libmyo_event_arm_lost		= 5, ///< A Myo has been moved or removed from the arm.
            
             */
            
            byte[] data = new byte[17];
            data[0] = (byte) 0; //size of the data is zero(only event type)

            if (status == 0)
            {
                int count = 0;
                string event_type = "0";

                
                while (count < 3)
                {
                    
                    for (int y = 0; y < Encoding.ASCII.GetBytes(event_type).Length; y++)
                    {
                        data[y+1] = Encoding.ASCII.GetBytes(event_type)[y];
                        
                    }

                    pipeStream.Write(data, 0, data.Length);
                    count++;

                    if (count == 1)
                    {
                        event_type = "2";
                    }
                    else if (count == 2)
                    {
                        event_type = "4";
                    }
                    else
                    {
                        count = 3;
                    }
                }

            }else
            {
                int count = 0;
                string event_type = "1";


                while (count < 3)
                {

                    for (int y = 0; y < Encoding.ASCII.GetBytes(event_type).Length; y++)
                    {
                        data[y + 1] = Encoding.ASCII.GetBytes(event_type)[y];

                    }

                    pipeStream.Write(data, 0, data.Length);
                    count++;

                    if (count == 1)
                    {
                        event_type = "3";
                    }
                    else if (count == 2)
                    {
                        event_type = "5";
                    }
                    else
                    {
                        count = 3;
                    }
                }
            }
          

        }
    }
}
