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
        public const string stopRecordingLabel = "Stop Recording";
        public const string startRecordingLabel = "Start Recording";

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
        private Boolean currentlyRecording;
        private MyoRecorder recorder;

        public MyoSimulatorForm(NamedPipeServerStream pipeStream)
        {
            currentlyRecording = false;
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

        private void playRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Recorded Binary Midas (RBM) File";
            openFileDialog.Filter = "RBM files|*.rbm";
            openFileDialog.InitialDirectory = @"C:\";
            if (getOpenFileDialogResult(openFileDialog) == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                System.Console.WriteLine("File from Dialog: " + fileName);
                RecorderFileHandler fileHandler = new RecorderFileHandler(fileName);
                Multimap<uint, RecorderFileHandler.RecordedData> timestampToData;

                try
                {
                    timestampToData = fileHandler.readRecorderFile();
                }
                catch (FileNotFoundException except)
                {
                    MessageBox.Show(except.ToString(), string.Format("File {0} does not exist", fileName));
                    return;
                }

                commandRunner.runCommands(timestampToData);
            }
        }

        private void loadScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Script File";
            openFileDialog.Filter = "TXT files|*.txt";
            openFileDialog.InitialDirectory = @"C:\";
            if (getOpenFileDialogResult(openFileDialog) == DialogResult.OK)
            {
                scriptPath.Text = openFileDialog.FileName;
            }
        }

        private DialogResult getOpenFileDialogResult(OpenFileDialog dialog)
        {
            OpenFileDialogResult dialogResult = new OpenFileDialogResult(dialog);
            System.Threading.Thread dialogThread = new System.Threading.Thread(dialogResult.threadShowDialog);
            dialogThread.SetApartmentState(System.Threading.ApartmentState.STA);
            dialogThread.Start();
            dialogThread.Join();

            return dialogResult.getResult();
        }

        private void startStopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Add the recording feature.
            if (!currentlyRecording)
            {
                currentlyRecording = true;
                startStopRecordingToolStripMenuItem.Text = stopRecordingLabel;
                recorder = new MyoRecorder();
                recorder.Record();
                System.Console.WriteLine("Beginning recording!");
            }
            else
            {
                if (recorder != null)
                {
                    currentlyRecording = false;
                    startStopRecordingToolStripMenuItem.Text = startRecordingLabel;
                    Multimap<uint, RecorderFileHandler.RecordedData> timeToDataMap = recorder.StopRecording();

                    // Testing
                    // TODO: Allow the user to specify a file.
                    RecorderFileHandler fileHandler = new RecorderFileHandler("recorded_binary_test.rbm");
                    fileHandler.writeRecorderFile(timeToDataMap);

                    System.Console.WriteLine("Ended recording!");
                }
            }
        }

        private void runScriptButton_Click(object sender, EventArgs e)
        {
            string fileName = scriptPath.Text;

            if (fileName.Length > 0)
            {
                MyoScriptParser parser = new MyoScriptParser(fileName);

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
        }

        public class OpenFileDialogResult
        {
            private OpenFileDialog openFileDialog;
            private DialogResult result;

            public OpenFileDialogResult(OpenFileDialog dialog)
            {
                openFileDialog = dialog;
            }

            public DialogResult getResult()
            {
                return result;
            }

            public void threadShowDialog()
            {
                result = openFileDialog.ShowDialog();
            }
        }

        private void MyoSimulatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO: send a disconnect event instead once we get protocol sorted
            string dc_signal = "DCed";
            pipeStream.Write(Encoding.ASCII.GetBytes(dc_signal), 0, dc_signal.Length);
            Dispose();
        }
    }
}
