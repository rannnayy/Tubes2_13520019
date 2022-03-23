using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.Threading;

namespace stima_filePedia
{
    public class Graph
    {
        private Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

        public Graph() { }

        public Microsoft.Msagl.Drawing.Graph GetGraph()
        {
            return graph;
        }

        public List<string> BFS(string mode,string fileSearched, string rootPath)
        {
            Queue<string> q = new Queue<string>();
            q.Enqueue(rootPath);
            List<string> results = new List<string>();
            bool found=false;

            this.graph.AddNode(rootPath);
            /*Thread.Sleep(1000);*/

            while (q.Any() && !found)
            {
                string temp = q.Dequeue();
                string[] dirsNfiles = Directory.GetFileSystemEntries(temp, "*", SearchOption.TopDirectoryOnly);
                Debug.WriteLine("temp: " + temp);

                this.graph.AddNode(temp).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                /*Thread.Sleep(1000);*/

                foreach (string currDirFile in dirsNfiles)
                {
                    Microsoft.Msagl.Drawing.Node node = this.graph.AddNode(currDirFile);
                    /*Thread.Sleep(1000);*/
                    // Mencocokkan nama File, file yang dicari ditemukan
                    if (File.Exists(currDirFile) && (Path.GetFileName(currDirFile) == fileSearched))
                    {
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
                    // Kalau file, beri warna merah
                    else
                    {
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    }
                }
            }
            Debug.WriteLine("Hasil Akhir :");
            Debug.WriteLine(String.Join(", ", results));
            /*Thread.Sleep(1000);*/

            // GRAPHING
            Microsoft.Msagl.Drawing.Node rootNode = this.graph.FindNode(rootPath);
            if (results.Count > 0) rootNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
            else rootNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;

            foreach (string result in results)
            {
                List<string> foundFilePath = new List<string>
                {
                    rootPath
                };
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

                Microsoft.Msagl.Drawing.Color blueColor = Microsoft.Msagl.Drawing.Color.Blue;
                Microsoft.Msagl.Drawing.Color redColor = Microsoft.Msagl.Drawing.Color.Red;
                if (parentNode.Attr.Color == blueColor && node.Attr.Color == blueColor)
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                } else if ((parentNode.Attr.Color == redColor || parentNode.Attr.Color == blueColor) && (node.Attr.Color == redColor))
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }
                else
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                }
            }

            return results;
        }

        public List<string> DFS(string mode,string fileSearched, string rootPath)
        {
            Stack<string> s = new Stack<string>();
            List<string> results = new List<string>();
            s.Push(rootPath);

            bool found=false;


            this.graph.AddNode(rootPath);
            while (s.Any() && !found)
            {
                string temp = s.Pop();
                Debug.WriteLine(temp);
                DirectoryInfo dir = new DirectoryInfo(temp);
                DirectoryInfo[] childDirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                // GRAPHING START
                this.graph.AddNode(temp).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                foreach (DirectoryInfo d in childDirs)
                {
                    this.graph.AddNode(d.FullName);
                }

                foreach (FileInfo file in files){
                    Microsoft.Msagl.Drawing.Node node = this.graph.AddNode(file.FullName);
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
                    } else
                    {
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    }
                }
                foreach(DirectoryInfo childDir in childDirs){
                    this.graph.AddNode(childDir.FullName);
                    s.Push(Path.Combine(temp,childDir.ToString()));
                }
            }
            Debug.WriteLine("Hasil Akhir :");
            Debug.WriteLine(String.Join(", ", results));

            // GRAPHING
            Microsoft.Msagl.Drawing.Node rootNode = this.graph.FindNode(rootPath);
            if (results.Count > 0) rootNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
            else rootNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;


            foreach (string result in results)
            {
                List<string> foundFilePath = new List<string>
                {
                    rootPath
                };
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

                Microsoft.Msagl.Drawing.Color blueColor = Microsoft.Msagl.Drawing.Color.Blue;
                Microsoft.Msagl.Drawing.Color redColor = Microsoft.Msagl.Drawing.Color.Red;
                if (parentNode.Attr.Color == blueColor && node.Attr.Color == blueColor)
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                }
                else if ((parentNode.Attr.Color == redColor || parentNode.Attr.Color == blueColor) && (node.Attr.Color == redColor))
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }
                else
                {
                    this.graph.AddEdge(parentNodeId, node.Id).Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                }
            }

            return results;
        }
    }
}
