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
        public const string commandDelimiter = ", ";

        /* Holds resulting binary commands which will be sorted using the time as the key */
        private Dictionary<int, byte[]> bin_command_list = new Dictionary<int, byte[]>();

        static Dictionary<string, HubCommunicator.Pose> labelToCommand = new Dictionary<string, HubCommunicator.Pose>
        {
            {"Rest", HubCommunicator.Pose.REST},
            {"Fist", HubCommunicator.Pose.FIST},
            {"Wave In", HubCommunicator.Pose.WAVE_IN},
            {"Wave Out", HubCommunicator.Pose.WAVE_OUT},
            {"Fingers Spread", HubCommunicator.Pose.FINGERS_SPREAD},
            {"Reserved 1", HubCommunicator.Pose.RESERVED1},
            {"Thumb to Pinky", HubCommunicator.Pose.THUMB_TO_PINKY},
            {"Unknown", HubCommunicator.Pose.UNKNOWN}
        };

        private HubCommunicator hubCommunicator;
        private List<HubCommunicator.Pose> poseList;
        private CommandRunner commandRunner;

        public MyoSimulatorForm(NamedPipeServerStream pipeStream)
        {
            hubCommunicator = new HubCommunicator(pipeStream);
            poseList = new List<HubCommunicator.Pose>();
            commandRunner = new CommandRunner(hubCommunicator);
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
            CommunicationBegin();
        }

        private void sendCommand(string label, Dictionary<string, HubCommunicator.Pose> labelToCommandMap)
        {
            HubCommunicator.Pose command;
            if (labelToCommandMap.TryGetValue(label, out command))
            {
                poseList.Add(command);
                commandChain.Text = string.Concat(commandChain.Text, label + commandDelimiter);
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
            MyoScriptParser parser = new MyoScriptParser("..\\..\\test_human_readable_input.txt");

            Multimap<uint, ParsedCommand> timestampToParsedCommands;

            try
            {
                timestampToParsedCommands = parser.parseScript();
            }
            catch (ArgumentException except)
            {
                MessageBox.Show(except.ToString(), string.Format("File does not exist"));
                return;
            }
            catch (FileNotFoundException except)
            {
                MessageBox.Show(except.ToString(), string.Format("File not found"));
                return;
            }

            commandRunner.runCommands(timestampToParsedCommands);
        }

        private void sendCommandButton_Click(object sender, EventArgs e)
        {
            string command = commandChain.Text;

            try
            {
                foreach (HubCommunicator.Pose pose in poseList)
                {
                    System.Console.WriteLine("Pose to send: " + pose);

                    if (!hubCommunicator.isConnected())
                    {
                        System.Console.WriteLine("Failed to connect!!");
                        return;
                    }

                    hubCommunicator.SendPose(pose);
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

        private void MyoSimulatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        private void readTestButton_Click(object sender, EventArgs e)
        {
            RecorderFileHandler fileHandler = new RecorderFileHandler("recorded_binary_test.rbm");

            Multimap<uint, RecorderFileHandler.RecordedData> timestampToData = fileHandler.readRecorderFile();
            commandRunner.runCommands(timestampToData);
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

        public void CommunicationBegin()
        {
            // Sends three messages: Paired, Connected and Arm Recognized.
            hubCommunicator.SendPaired();
            hubCommunicator.SendConnected();
            // TODO: Make Arm and XDirection configurable.
            hubCommunicator.SendArmRecognized(HubCommunicator.Arm.RIGHT, HubCommunicator.XDirection.FACING_WRIST);
        }

        public void CommunicationEnd()
        {
            hubCommunicator.SendArmLost();
            hubCommunicator.SendDisconnected();
            hubCommunicator.SendUnpaired();
        }
    }
}
