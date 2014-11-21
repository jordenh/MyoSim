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
        public const char commandDelimiter = ';';
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

        private unsafe int Parseline(string[] lines, int* abs_time)
        {
            int len = -1;
            //int delta_time;
            //int time_dat = 0;
            //int x, y, z;
            List<parsed_command> command_list = new List<parsed_command>();


            
            //byte[] time_bytes = new bytes[1];

            //byte[] current_orient_command = new byte[44];
            //byte[] current_async_command = new byte[7];

            //IntPtr time_p = Marshal.AllocHGlobal(Marshal.SizeOf(time_dat));
            //IntPtr quat_p = Marshal.AllocHGlobal(Marshal.SizeOf(quat_dat));
            //IntPtr gyro_p = Marshal.AllocHGlobal(Marshal.SizeOf(gyro_dat));
            //IntPtr accel_p = Marshal.AllocHGlobal(Marshal.SizeOf(accel_dat));

            int i = 0;
            foreach (string line in lines)
            {
                /* Remove leading and trailing spaces */
                line.Trim();
                command_list.Add(new parsed_command(line));
                command_list.ElementAt(i).print_debug();
                ++i;
                /* Check for word for keyword */
                //switch (command.First())
                //{
                //    case MOVE_KW:
                //        /* Check for invalid inputs */
                //        if (command.Length != SIZEOF_MOVE_CMD ||
                //            !(int.TryParse(command.Last(), out delta_time)) ||
                //            delta_time < 0 ||
                //            !(float.TryParse(command[1], out x)) ||
                //            !(float.TryParse(command[2], out y)) ||
                //            !(float.TryParse(command[3], out z)))
                //        {
                //            Console.WriteLine("ERROR in move");
                //        }
                //        else
                //        {
                //            /* Calculate the change in angles per time tick */
                //            gyro_change_per_time.x = (gyro_dat.x - gyro_dat_prev.x) / delta_time;
                //            gyro_change_per_time.y = (gyro_dat.y - gyro_dat_prev.y) / delta_time;
                //            gyro_change_per_time.z = (gyro_dat.z - gyro_dat_prev.z) / delta_time;

                //            /* Set first bit to be 1 to signal orientation data */
                //            time_dat |= (1 << (sizeof(int) * 8));

                //            /* Make sure that the last data sent is a multiple of ms_btw_send */
                //            for (int t = 0; t <= (delta_time); t += ms_btw_send)
                //            {
                //                //dispX.calc_next_disp(t);
                //                //dispY.calc_next_disp(t);
                //                //dispZ.calc_next_disp(t);

                //                /* set time of command */
                //                time_dat &= 0x80;
                //                time_dat |= (t & 0x7F);
                //                /* Calculate and save quaterion data */

                //                /* Calculate and save gyro vector */
                //                gyro_dat_prev.x += gyro_change_per_time.x;
                //                gyro_dat_prev.y += gyro_change_per_time.y;
                //                gyro_dat_prev.z += gyro_change_per_time.z;

                //                /* Acceleration data is not changing in move command */
                //                /* change all the structs to byte[] */
                //                Marshal.StructureToPtr(time_dat, time_bytes, false);
                //                Marshal.StructureToPtr(quat_dat, quat_bytes, false);
                //                Marshal.StructureToPtr(gyro_dat_prev, gyro_bytes, false);
                //                Marshal.StructureToPtr(accel_dat, accel_bytes, false);

                //                /* Combine all the byte[] into one for saving into the dictionary */
                //                System.Buffer.BlockCopy(time_bytes, 0, current_orient_command, TIME_B_START, sizeof(int));
                //                /*  current_orient_command[TIME_B_START] |= (byte)((t >> 3) & 0x0000007F);
                //                  current_orient_command[TIME_B_START + 1] = (byte)((t >> 2) & 0x000000FF);
                //                  current_orient_command[TIME_B_START + 2] = (byte)((t >> 1) & 0x000000FF);
                //                  current_orient_command[TIME_B_START + 3] = (byte)(t & 0x000000FF);
                //                  current_orient_command[QUAT_B_START] += BitConverter.GetBytes(dispX.get_disp());
                //                  current_orient_command[QUAT_B_START + 1] += (byte)dispY.get_disp();
                //                  current_orient_command[3] += (byte)dispZ.get_disp();
                //                  Console.WriteLine("X " + current_orient_command[1]);
                //                  bin_command_list.Add(t, current_orient_command);
                //            */
                //            }
                //        }

                //        break;
                //    case SET_ACCEL_KW:
                //        break;
                //    case DELAY_KW:
                //        break;
                //    case ASYNC_KW:
                //        break;
                //    case EXPECT_KW:
                //        break;
                //    default:
                //        break;
                //}
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
            
            ReadInputFile(@"C:\Users\Frederick\Source\Repos\MyoSim\MyoSimulatorForm\test_human_readable_input.txt");
            //ReadInputFile(@"D:\Fred\Source\Repos\MyoSim\MyoSimulatorForm\test_human_readable_input.txt");

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
