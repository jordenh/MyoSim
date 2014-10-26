using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
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

            foreach (string word in words)
            {
                System.Console.WriteLine(word);
            }
        }
    }
}
