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
    public partial class Form2 : Form
    {
        public Timer timer1 = new Timer();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Interval = 500;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
            progressBar1.Maximum = 5;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == progressBar1.Maximum)
            {
                timer1.Stop();

                this.Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
            else
            {
                progressBar1.Value += 1;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = progressBar1.Maximum;
        }
    }
}
