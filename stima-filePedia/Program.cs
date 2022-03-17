using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace stima_filePedia
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            stima_filePedia.Graph graph = new stima_filePedia.Graph("D:/TLX");
            graph.BFS("all","main.cpp");
            graph.BFS("first","main.cpp");
            graph.DFS("all","main.cpp");
            graph.DFS("first","main.cpp");
        }
    }
}
