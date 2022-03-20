using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

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
            bgWk.DoWork += bgWorking;
            bgWk.RunWorkerCompleted += bgWorkingDone;
        }

        private void FileFound(string finalPath)
        {
            listBoxLinkPath.BeginInvoke((Action) delegate() { 
                listBoxLinkPath.Items.Add(finalPath); 
            });
        }

        private void bgWorkingDone(object sender, RunWorkerCompletedEventArgs args)
        {
            Console.WriteLine("Done");
            gViewer1.Graph = graph.GetGraph();
            stopwatch.Stop();
            labelTE.Text = stopwatch.ElapsedMilliseconds + " ms";
        }

        public void bgWorking(object sender, DoWorkEventArgs args)
        {
            //graph.BFS("all","nama file");
            Console.WriteLine(labelFolder.Text == "No Folder Chosen");
            Console.WriteLine(textBoxFileName.Text == "");
            Console.WriteLine(checkBoxFindAll.Checked);
            Console.WriteLine(radioButtonBFS.Checked);
            Console.WriteLine(radioButtonDFS.Checked);
            if (labelFolder.Text == "No Folder Chosen")
            {
                MessageBox.Show("Choose A Folder!");
            }
            if (textBoxFileName.Text == "")
            {
                MessageBox.Show("Input A Filename! (including extension)");
            }
            if (radioButtonBFS.Checked)
            {
                if (checkBoxFindAll.Checked)
                {
                    graph.BFS("all", textBoxFileName.Text, labelFolder.Text);
                }
                else
                {
                    graph.BFS("first", textBoxFileName.Text, labelFolder.Text);
                }
            }
            else if (radioButtonDFS.Checked)
            {
                if (checkBoxFindAll.Checked)
                {
                    graph.DFS("all", textBoxFileName.Text, labelFolder.Text);
                }
                else
                {
                    graph.DFS("first", textBoxFileName.Text, labelFolder.Text);
                }
            }

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
            stopwatch.Start();
            graph = new Graph();
            graph.FinishSearch += FileFound;
            bgWk.RunWorkerAsync();
        }

        private void listBoxLinkPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            string file_path = listBoxLinkPath.SelectedItem.ToString();
            Process.Start(Path.GetDirectoryName(file_path));
        }
    }
}
