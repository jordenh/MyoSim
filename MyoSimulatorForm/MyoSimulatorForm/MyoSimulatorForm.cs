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
using System.Runtime.InteropServices;

namespace MyoSimGUI
{
    public partial class MyoSimulatorForm : Form
    {
        /* static constants */
        private const int SIZEOF_MOVE_CMD = 5;
        private const int TIME_B_START = 0;
        private const int QUAT_B_START = 4;
        private const int GYRO_B_START = 19;
        private const int ACCL_B_START = 31;

        /* Keywords in our language. */
        private const string MOVE_KW = "move";
        private const string SET_ACCEL_KW = "set_accel";
        private const string DELAY_KW = "delay";
        private const string ASYNC_KW = "async";
        private const string EXPECT_KW = "expect";


        public const char commandDelimiter = ';';
        private static int ms_btw_send = 10;
        private NamedPipeServerStream pipeStream;

        /* Holds resulting binary commands which will be sorted using the time as the key */
        private Dictionary<int, byte[]> bin_command_list = new Dictionary<int, byte[]>();

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
        
        public struct Quaternion
        {
            public float x;
            public float y;
            public float z;
            public float w;
        }

        public struct vector3
        {
            public float x;
            public float y;
            public float z;
        }

        private unsafe int Parseline(string[] lines, int* abs_time)
        {
            int len = -1;
            int delta_time;
            int time_dat;
            Quaternion quat_dat;
            vector3 gyro_dat;
            vector3 gyro_dat_prev;
            vector3 accel_dat;

            char command_delim = ' ';
            //byte[] current_orient_command = new byte[44];
            byte[] current_async_command = new byte[7];

            IntPtr time_bytes = Marshal.AllocHGlobal(Marshal.SizeOf(time_dat));
            IntPtr quat_bytes = Marshal.AllocHGlobal(Marshal.SizeOf(quat_dat));
            IntPtr gyro_bytes = Marshal.AllocHGlobal(Marshal.SizeOf(gyro_dat));
            IntPtr accel_bytes = Marshal.AllocHGlobal(Marshal.SizeOf(accel_dat));

            for (int i = 0; i < lines.Length; ++i)
            {
                /* Remove leading and trailing spaces */
                lines[i].Trim();
                string[] command = lines[i].Split(command_delim);

                for (int j = 0; j < command.Length; ++j)
                {

                    Console.WriteLine(command[j]);
                }

                /* Check for word for keyword */
                switch (command.First())
                {
                    case MOVE_KW:
                        /* Check for invalid inputs */
                        if (command.Length != SIZEOF_MOVE_CMD ||
                            !(int.TryParse(command.Last(), out delta_time)) ||
                            delta_time < 0 ||
                            !(float.TryParse(command[1], out gyro_dat.x)) ||
                            !(float.TryParse(command[2], out gyro_dat.y)) ||
                            !(float.TryParse(command[3], out gyro_dat.z)) )
                        {
                            Console.WriteLine("ERROR in move");
                        }
                        else
                        {
                            displacement_variables dispX = new displacement_variables(gyro_dat.x, gyro_dat_prev.x);
                            displacement_variables dispY = new displacement_variables(gyro_dat.y, gyro_dat_prev.y);
                            displacement_variables dispZ = new displacement_variables(gyro_dat.z, gyro_dat_prev.z);

                            /* Set first bit to be 1 to signal orientation data */
                            time_dat |= ( 1 << (sizeof(int)*8) );

                            /* Make sure that the last data sent is a multiple of ms_btw_send */
                            for (int t = 0; t <= (delta_time + ms_btw_send - (delta_time % ms_btw_send)); t += ms_btw_send)
                            {
                                dispX.calc_next_disp(t);
                                dispY.calc_next_disp(t);
                                dispZ.calc_next_disp(t);

                                /* set time of command */
                                time_dat |= (t & 0x7F);
                                /* Calculate and save quaterion data */
                                
                                /* Calculate and save gyro vector */
                                gyro_dat.x += dispX.get_disp();
                                gyro_dat.y += dispY.get_disp();
                                gyro_dat.z += dispZ.get_disp();
                                /* Get accel data */

                              /*  current_orient_command[TIME_B_START] |= (byte)((t >> 3) & 0x0000007F);
                                current_orient_command[TIME_B_START + 1] = (byte)((t >> 2) & 0x000000FF);
                                current_orient_command[TIME_B_START + 2] = (byte)((t >> 1) & 0x000000FF);
                                current_orient_command[TIME_B_START + 3] = (byte)(t & 0x000000FF);
                                
                                current_orient_command[QUAT_B_START] += BitConverter.GetBytes(dispX.get_disp());
                                current_orient_command[QUAT_B_START + 1] += (byte)dispY.get_disp();
                                current_orient_command[3] += (byte)dispZ.get_disp();
                                Console.WriteLine("X " + current_orient_command[1]);
                                bin_command_list.Add(t, current_orient_command);
                          */
                            }
                        }

                        break;
                    case SET_ACCEL_KW:
                        break;
                    case DELAY_KW:
                        break;
                    case ASYNC_KW:
                        break;
                    case EXPECT_KW:
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

            
            unsafe
            {
                Parseline(lines, &current_time);
            }
            
           /* for (int i = 0; i < lines.Length; ++i)
            {
                Console.WriteLine(lines[i]);
            }
            */
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

        private void callMyoSim(string command)
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
            
            //ReadInputFile(@"C:\Users\Frederick\Source\Repos\MyoSim\MyoSimulatorForm\test_human_readable_input.txt");
            ReadInputFile(@"D:\Fred\Source\Repos\MyoSim\MyoSimulatorForm\test_human_readable_input.txt");

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
