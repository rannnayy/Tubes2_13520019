using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;

namespace stima_filePedia
{
    public delegate void FileFound(string finalPath);

    public class Graph
    {
        private string rootPath;
        private Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

        public event FileFound FinishSearch;

        public Graph(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public int Count { get; set; }
        public string RootPath { get { return this.rootPath; } }

        //create a form 
        // System.Windows.Forms.Form form = new System.Windows.Forms.Form();
        //create a viewer object 
        // Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        //create a graph object 
        

        public Microsoft.Msagl.Drawing.Graph GetGraph()
        {
            return graph;
        }

        public void BFS(string mode,string fileSearched)
        {
            Queue<string> q = new Queue<string>();
            q.Enqueue(this.rootPath);
            List<string> results = new List<string>();
            bool found=false;

            while(q.Any() && !found)
            {
                string temp = q.Dequeue();
                string[] dirsNfiles = Directory.GetFileSystemEntries(temp, "*", SearchOption.TopDirectoryOnly);
                Debug.WriteLine("temp: " + temp);

                // GRAPHING START (Add Node)
                this.graph.AddNode(rootPath);
                foreach (string dnf in dirsNfiles)
                {
                    this.graph.AddNode(dnf).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }
                // GRAPHING END

                foreach (string currDirFile in dirsNfiles)
                {
                    // Mencocokkan nama File, file yang dicari ditemukan
                    if (File.Exists(currDirFile) && (Path.GetFileName(currDirFile) == fileSearched))
                    {
                        //FinishSearch(currDirFile);
                        Debug.Write("Ketemu : ");
                        Debug.WriteLine(currDirFile);
                        results.Add(currDirFile);
                        if (mode == "first")
                        {
                            found=true;
                        }
                    }
                    // Kalau directory, append ke Q
                    else if (Directory.Exists(currDirFile))
                    {
                        q.Enqueue(currDirFile);
                    }
                }
            }
            Debug.WriteLine("Hasil Akhir :");
            Debug.WriteLine(String.Join(", ", results));
            //FinishSearch(null);

            // GRAPHING
            Microsoft.Msagl.Drawing.Node rootNode = this.graph.FindNode(rootPath);
            if (results.Count > 0) rootNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
            else rootNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;

            foreach (string result in results)
            {
                List<string> foundFilePath = new List<string>();
                foundFilePath.Add(rootPath);
                string[] combination = result.Substring(rootPath.Length + 1).Split('\\');
                for (int i = 0; i < combination.Length; i++)
                {
                    // Change the color to blue
                    string joined = foundFilePath[i] + "\\" + combination[i];
                    foundFilePath.Add(joined);
                    this.graph.FindNode(joined).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                }
            }

            Microsoft.Msagl.Drawing.Node[] nodeArray = this.graph.Nodes.ToArray().Where(n => n.Id != rootPath).ToArray();
            foreach (Microsoft.Msagl.Drawing.Node node in nodeArray)
            {

                string[] pathSplit = node.Id.Split('\\');
                string parentNodeId = string.Join("\\", pathSplit.Take(pathSplit.Length - 1));
                Debug.WriteLine("parent node id: " + parentNodeId);
                Microsoft.Msagl.Drawing.Node parentNode = this.graph.FindNode(parentNodeId);
                node.LabelText = pathSplit.Last();

                if (parentNode.Attr.Color == Microsoft.Msagl.Drawing.Color.Blue && node.Attr.Color == Microsoft.Msagl.Drawing.Color.Blue)
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                }
                else
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }
            }
            

        }

        public void DFS(string mode,string fileSearched)
        {
            Stack<string> s = new Stack<string>();
            List<string> results = new List<string>();
            s.Push(this.rootPath);

            bool found=false;
            while(s.Any() && !found)
            {
                string temp = s.Pop();
                Debug.WriteLine(temp);
                DirectoryInfo dir = new DirectoryInfo(temp);
                DirectoryInfo[] childDirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                // GRAPHING START
                this.graph.AddNode(rootPath);
                foreach (DirectoryInfo d in childDirs)
                {
                    this.graph.AddNode(d.FullName).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }
                foreach (FileInfo f in files)
                {
                    this.graph.AddNode(f.FullName).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }

                foreach (FileInfo file in files){
                    Debug.Write("Nama File :");
                    Debug.WriteLine(file);
                    if(file.Name==fileSearched){
                        string result = Path.Combine(temp,file.Name);
                        Debug.Write("Ketemu :");
                        Debug.WriteLine(result);
                        results.Add(result);
                        if (mode == "first")
                        {
                            found=true;
                        }
                        //FinishSearch(result); //auto berhenti

                    }
                }
                foreach(DirectoryInfo childDir in childDirs){
                    s.Push(Path.Combine(temp,childDir.ToString()));
                }
            }
            Debug.WriteLine("Hasil Akhir :");
            Debug.WriteLine(String.Join(", ", results));
            //FinishSearch(null);


            // GRAPHING
            Microsoft.Msagl.Drawing.Node rootNode = this.graph.FindNode(rootPath);
            if (results.Count > 0) rootNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
            else rootNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;


            foreach (string result in results)
            {
                List<string> foundFilePath = new List<string>();
                foundFilePath.Add(rootPath);
                string[] combination = result.Substring(rootPath.Length + 1).Split('\\');
                for (int i = 0; i < combination.Length; i++)
                {
                    // Change the color to blue
                    string joined = foundFilePath[i] + "\\" + combination[i];
                    foundFilePath.Add(joined);
                    this.graph.FindNode(joined).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                }
            }

            Microsoft.Msagl.Drawing.Node[] nodeArray = this.graph.Nodes.ToArray().Where(n => n.Id != rootPath).ToArray();
            foreach (Microsoft.Msagl.Drawing.Node node in nodeArray)
            {

                string[] pathSplit = node.Id.Split('\\');
                string parentNodeId = string.Join("\\", pathSplit.Take(pathSplit.Length - 1));
                Microsoft.Msagl.Drawing.Node parentNode = this.graph.FindNode(parentNodeId);
                node.LabelText = pathSplit.Last();

                if (parentNode.Attr.Color == Microsoft.Msagl.Drawing.Color.Blue && node.Attr.Color == Microsoft.Msagl.Drawing.Color.Blue)
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                }
                else
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }
            }
        }
    }
}
