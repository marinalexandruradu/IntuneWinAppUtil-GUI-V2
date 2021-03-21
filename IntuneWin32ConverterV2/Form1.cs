using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

namespace IntuneWin32ConverterV2
{
    public partial class Form1 : Form
    {

       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button4.Hide();
            checkBox1.Checked = true;
            button2.Hide();
            textBox2.Hide();
            label2.Hide();
            toolStripStatusLabel1.Text = "Ready to convert!";
            string appPath = Application.StartupPath;
            string IntuneFileLocation = appPath + @"\IntuneWinAppUtil.exe";
            if (!File.Exists(IntuneFileLocation))
            {
                bool Download = Program.DownloadFromGitHub();

                if (Download)
                {
                    toolStripStatusLabel1.Text = "IntuneWinAppUtil.exe succesfully downloaded from GitHub. Ready to convert!";
                    statusStrip1.BackColor = Color.Green;
                    statusStrip1.ForeColor = Color.White;
                }
                else
                {
                    toolStripStatusLabel1.Text = "Could not download IntuneWinAppUtil.exe from GitHub. Please download it manually and place it near the exe";
                    statusStrip1.BackColor = Color.Red;
                    statusStrip1.ForeColor = Color.White;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedPath = "";
            var t = new Thread((ThreadStart)(() => {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = @"C:\";
                openFileDialog1.Title = "Select Source Location";
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.CheckPathExists = true;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                selectedPath = openFileDialog1.FileName;
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            //Console.WriteLine(selectedPath);

            textBox1.Text = selectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedPath = "";
            var t = new Thread((ThreadStart)(() => {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == DialogResult.Cancel)
                    return;

                selectedPath = fbd.SelectedPath;
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            textBox2.Text = selectedPath;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.Text = System.IO.Path.GetDirectoryName(textBox1.Text);
            }
            if ((String.IsNullOrEmpty(textBox1.Text)) && (String.IsNullOrEmpty(textBox2.Text)))
            {
                toolStripStatusLabel1.Text = "Please select a source file and an output folder";
                statusStrip1.BackColor = Color.Red;
                statusStrip1.ForeColor = Color.White;
            }
            else
            {
                string appPath = Application.StartupPath;
                string dbf_File = System.IO.Path.GetFileName(textBox1.Text);
                string dbf_Path = System.IO.Path.GetDirectoryName(textBox1.Text);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = appPath + @"\IntuneWinAppUtil.exe";
                char c = (char)34;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = " -c " + c + dbf_Path + c + " -s " + c + dbf_File + c + " -o " + c + textBox2.Text + c + " -q";
              //  Console.WriteLine(startInfo.Arguments);
                try
                {
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                        toolStripStatusLabel1.Text = "Conversion succesfull";
                        statusStrip1.BackColor = Color.Green;
                        statusStrip1.ForeColor = Color.White;
                        button4.Show();
                    }
                }
                catch
                {
                    toolStripStatusLabel1.Text = "Conversion error";
                    statusStrip1.BackColor = Color.Red;
                    statusStrip1.ForeColor = Color.White;
                }
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool Download = Program.DownloadFromGitHub();

            if (Download)
            {
                toolStripStatusLabel1.Text = "IntuneWinAppUtil.exe succesfully downloaded from GitHub. Ready to convert!";
                statusStrip1.BackColor = Color.Green;
                statusStrip1.ForeColor = Color.White;
            }
            else
            {
                toolStripStatusLabel1.Text = "Could not download IntuneWinAppUtil.exe from GitHub. Please download it manually and place it near the exe";
                statusStrip1.BackColor = Color.Red;
                statusStrip1.ForeColor = Color.White;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", @textBox2.Text);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.alexandrumarin.com/");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button2.Hide();
                textBox2.Hide();
                label2.Hide();
            }
            else {
                button2.Show();
                textBox2.Show();
                label2.Show();
            }
        }
    }
}
