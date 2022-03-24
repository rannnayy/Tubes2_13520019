using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace stima_filePedia
{
    public partial class Form1 : Form
    {
        private Graph graph;
        private Stopwatch stopwatch;
        private bool isRunning = false;
        List<string> results = new List<string>();

        public Form1()
        {
            InitializeComponent();
            stopwatch = new Stopwatch();

        }

        // Button Search Folder
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                labelFolder.Text = dialog.SelectedPath;
            }
            else {
                labelFolder.Text = "No Folder Chosen"; 
            }
        }

        // Button Search!
        private void button2_Click(object sender, EventArgs e)
        {
            listBoxLinkPath.Items.Clear();
            stopwatch.Reset();


            if (isRunning)
            {
                MessageBox.Show("The file searching process is currently running!");
                return;
            }

            if (labelFolder.Text == "No Folder Chosen")
            {
                MessageBox.Show("Choose A Folder!");
                return;
            }
            if (textBoxFileName.Text == "")
            {
                MessageBox.Show("Input A Filename! (including extension)");
                return;
            }

            stopwatch.Reset();
            isRunning = true;
            backgroundWorker2.RunWorkerAsync();
        }

        private void listBoxLinkPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            string file_path = listBoxLinkPath.SelectedItem.ToString();
            Process.Start(Path.GetDirectoryName(file_path));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

            graph = new Graph();

            if (radioButtonBFS.Checked)
            {
                stopwatch.Start();
                if (checkBoxFindAll.Checked)
                {
                    results = graph.BFS("all", textBoxFileName.Text, labelFolder.Text, backgroundWorker2);

                }
                else
                {
                    results = graph.BFS("first", textBoxFileName.Text, labelFolder.Text, backgroundWorker2);
                }
            }
            else if (radioButtonDFS.Checked)
            {
                stopwatch.Start();
                if (checkBoxFindAll.Checked)
                {
                    results = graph.DFS("all", textBoxFileName.Text, labelFolder.Text, backgroundWorker2);
                } 
                else
                {
                    results = graph.DFS("first", textBoxFileName.Text, labelFolder.Text, backgroundWorker2);
                }
                
            }

        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelTE.Text = stopwatch.ElapsedMilliseconds + " ms";
            gViewer1.Graph = graph.GetGraph();

        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            stopwatch.Stop();
            labelTE.Text = stopwatch.ElapsedMilliseconds + " ms";
            isRunning = false;
            foreach (string res in results)
            {
                listBoxLinkPath.Items.Add(res);
            }
        }
    }
}
