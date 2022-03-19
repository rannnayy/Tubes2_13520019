using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stima_filePedia
{
    public partial class Form1 : Form
    {
        private Graph graph;
        public Form1()
        {
            InitializeComponent();
            this.graph = new Graph("C:\\Users\\vitos\\Files\\folder-test");
            this.graph.FinishSearch += FileFound;
            bgWk.DoWork += bgWorking;
            bgWk.RunWorkerCompleted += bgWorkingDone;
        }

        private void FileFound(string finalPath)
        {

        }

        private void bgWorkingDone(object sender, RunWorkerCompletedEventArgs args)
        {
            Console.WriteLine("Done");
        }

        public void bgWorking(object sender, DoWorkEventArgs args)
        {
            graph.BFS("all","nama file");
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
            Console.WriteLine(labelFolder.Text == "No Folder Chosen");
            Console.WriteLine(textBoxFileName.Text == "");
            Console.WriteLine(checkBoxFindAll.Checked);
            Console.WriteLine(radioButtonBFS.Checked);
            Console.WriteLine(radioButtonDFS.Checked);
            if (labelFolder.Text == "No Folder Chosen")
            {

            }

            graph.BFS("all", "Forza Fix 2.1.exe");
            gViewer1.Graph = graph.GetGraph();
        }

        private void textBoxFileName_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBoxLinkPath_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxFindAll_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonBFS_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonDFS_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void gViewer1_Load(object sender, EventArgs e)
        {
        }
    }
}
