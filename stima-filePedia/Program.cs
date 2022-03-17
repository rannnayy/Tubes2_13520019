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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            stima_filePedia.Graph graph = new stima_filePedia.Graph("C:\\Users\\vitos\\Files\\folder-test");
            // Debug.WriteLine("\nBFS: ");
            // graph.BFS("all","prak1.sql");
            // Debug.WriteLine("\nBFS: ");
            // graph.BFS("first", "prak1.sql");
            // Debug.WriteLine("\nDFS: ");
            // graph.DFS("all", "prak1.sql");
            // Debug.WriteLine("\nDFS: ");
            // graph.DFS("first", "prak1.sql");

            
        }
    }
}
