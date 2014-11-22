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
using MyoSimGUI.ParsedCommands;

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
            MyoScriptParser parser = new MyoScriptParser("..\\..\\test_human_readable_input.txt");

            Multimap<uint, ParsedCommand> timestampToParsedCommands;

            try
            {
                timestampToParsedCommands = parser.parseScript();
            }
            catch(ArgumentException except)
            {
                MessageBox.Show(except.ToString(), string.Format("File does not exist"));
                return;
            }
            catch (FileNotFoundException except)
            {
                MessageBox.Show(except.ToString(), string.Format("File not found"));
                return;
            }   
    
            foreach (KeyValuePair<uint, List<ParsedCommand>> entry in timestampToParsedCommands.getUnderlyingDict())
            {
                uint time = entry.Key;
                List<ParsedCommand> commands = entry.Value;

                foreach (ParsedCommand cmd in commands)
                {
                    string output = String.Format("Time {0}: {1}", time, cmd);
                    System.Console.WriteLine(output);
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

        private void readTestButton_Click(object sender, EventArgs e)
        {
            RecorderFileHandler fileHandler = new RecorderFileHandler("recorded_binary_test.rbm");

            Multimap<uint, RecorderFileHandler.RecordedData> timestampToData = fileHandler.readRecorderFile();

            foreach (KeyValuePair<uint, List<RecorderFileHandler.RecordedData>> entry in timestampToData.getUnderlyingDict())
            {
                uint time = entry.Key;
                RecorderFileHandler.RecordedData command = entry.Value[0];

                if (command.type == RecorderFileHandler.RecordedDataType.ASYNC)
                {
                    System.Console.WriteLine(String.Format("Time {0}: Async Command =  {1}", time, command.asyncCommand));
                }
                else
                {
                    System.Console.WriteLine(String.Format("Time {0}: Orientation Quat = {1}, Gyro Dat = {2}, Accel Data = {3}", time,
                        command.orientationQuat, command.gyroDat, command.accelDat));
                }
            }
        }

        private void writeTestButton_Click(object sender, EventArgs e)
        {
            RecorderFileHandler fileHandler = new RecorderFileHandler("recorded_binary_test.rbm");

            RecorderFileHandler.RecordedData fistGesture = new RecorderFileHandler.RecordedData(ParsedCommand.AsyncCommandCode.FIST);
            RecorderFileHandler.RecordedData orientation1 = new RecorderFileHandler.RecordedData(new ParsedCommand.Quaternion(1, 2, 3, 4), 
                new ParsedCommand.vector3(0.5f, 0.5f, 0.5f), new ParsedCommand.vector3(0.2f, 0.6f, 0.8f));
            RecorderFileHandler.RecordedData fingersSpreadGesture = new RecorderFileHandler.RecordedData(ParsedCommand.AsyncCommandCode.FINGERS_SPREAD); 

            Multimap<uint, RecorderFileHandler.RecordedData> timestampToData = new Multimap<uint, RecorderFileHandler.RecordedData>();
            timestampToData.Add(0, fistGesture);
            timestampToData.Add(10, orientation1);
            timestampToData.Add(20, fingersSpreadGesture);

            fileHandler.writeRecorderFile(timestampToData);
        }
    }
}
