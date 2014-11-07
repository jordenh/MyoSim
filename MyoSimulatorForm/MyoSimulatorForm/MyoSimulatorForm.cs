using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyoSimGUI
{
    public partial class MyoSimulatorForm : Form
    {
        public const char commandDelimiter = ';';

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

        public MyoSimulatorForm()
        {
            InitializeComponent();
            foreach (string key in labelToCommand.Keys) 
            {
                this.gestureList.Items.Add(key);
            }
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

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
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
                    commandChain.Text = command;
                }
            }         
        }

        private void sendCommandButton_Click(object sender, EventArgs e)
        {
            string command = commandChain.Text;
            string[] words = command.Split(commandDelimiter);

            foreach (string word in words)
            {
                System.Console.WriteLine(word);
            }
        }

        private void addGestureButton_Click(object sender, EventArgs e)
        {
            sendCommand(this.gestureList.SelectedItem.ToString(), labelToCommand);
        }
    }
}
