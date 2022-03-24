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

            List<string> results = new List<string>();

            graph = new Graph();

            if (labelFolder.Text == "No Folder Chosen")
            {
                MessageBox.Show("Choose A Folder!");
            }
            if (textBoxFileName.Text == "")
            {
                MessageBox.Show("Input A Filename! (including extension)");
            }

            if (labelFolder.Text != "No Folder Chosen" && textBoxFileName.Text != "")
            {
                if (radioButtonBFS.Checked)
                {
                    stopwatch.Start();
                    if (checkBoxFindAll.Checked)
                    {
                        results = graph.BFS("all", textBoxFileName.Text, labelFolder.Text);
                    }
                    else
                    {
                        results = graph.BFS("first", textBoxFileName.Text, labelFolder.Text);
                    }
                    stopwatch.Stop();
                }
                else if (radioButtonDFS.Checked)
                {
                    stopwatch.Start();
                    if (checkBoxFindAll.Checked)
                    {
                        results = graph.DFS("all", textBoxFileName.Text, labelFolder.Text);
                    }
                    else
                    {
                        results = graph.DFS("first", textBoxFileName.Text, labelFolder.Text);
                    }
                    stopwatch.Stop();
                }

                labelTE.Text = stopwatch.ElapsedMilliseconds + " ms";

                foreach (string res in results)
                {
                    listBoxLinkPath.Items.Add(res);
                }

                gViewer1.Graph = graph.GetGraph();
            }
            
        }

        private void listBoxLinkPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            string file_path = listBoxLinkPath.SelectedItem.ToString();
            Process.Start(Path.GetDirectoryName(file_path));
        }
    }
}
