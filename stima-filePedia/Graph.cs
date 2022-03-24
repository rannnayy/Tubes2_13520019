using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.ComponentModel;

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


        // public async List<string> BFS(string mode, string fileSearched, string rootPath)
        
        public List<string> BFS(string mode,string fileSearched, string rootPath, BackgroundWorker bgWorker)
        {
            Queue<string> q = new Queue<string>();
            q.Enqueue(rootPath);
            List<string> results = new List<string>();
            bool found=false;


            while (q.Any() && !found)
            {
                string temp = q.Dequeue();
                string[] dirsNfiles = Directory.GetFileSystemEntries(temp, "*", SearchOption.TopDirectoryOnly);

                // Jika directory kosong, tandai dengan directory tersebut dengan warna merah serta warna parent nya (jika hitam)
                if (dirsNfiles.Length == 0)
                {
                    List<string> foundFilePath = new List<string> { rootPath };
                    string[] combination = temp.Substring(rootPath.Length + 1).Split('\\');
                    for (int i = 0; i < combination.Length; i++)
                    {
                        string joined = foundFilePath[i] + "\\" + combination[i];
                        foundFilePath.Add(joined);
                        var joinedNode = this.graph.FindNode(joined);
                        if (joinedNode.Attr.Color.Equals(Microsoft.Msagl.Drawing.Color.Black))
                        {
                            joinedNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                        }
                        foreach (var edge in this.graph.Edges.ToArray())
                        {
                            if (edge.TargetNode.Id == foundFilePath[i])
                            {
                                if (edge.Attr.Color.Equals(Microsoft.Msagl.Drawing.Color.Black))
                                {
                                    edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                                }
                                break;
                            }
                        }
                    }
                }

                var parentNode = this.graph.AddNode(temp);
                var baseLabelText = parentNode.LabelText.Split('\\');
                parentNode.LabelText = baseLabelText.Last();

                bgWorker.ReportProgress(0);
                Thread.Sleep(250);

                foreach (string currDirFile in dirsNfiles)
                {
                    Microsoft.Msagl.Drawing.Node node = this.graph.AddNode(currDirFile);
                    var e = new Microsoft.Msagl.Drawing.Edge(parentNode, node, Microsoft.Msagl.Drawing.ConnectionToGraph.Connected);

                    var labelText = node.LabelText.Split('\\');
                    node.LabelText = labelText.Last();
                    bgWorker.ReportProgress(0);
                    Thread.Sleep(250);

                    // Mencocokkan nama File, file yang dicari ditemukan
                    if (File.Exists(currDirFile) && (Path.GetFileName(currDirFile) == fileSearched))
                    {
                        Debug.Write("Ketemu : ");
                        Debug.WriteLine(currDirFile);
                        results.Add(currDirFile);

                        // Mewarnai hasil ketemu dan semua parent directory nya dengan warna biru
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                        this.graph.FindNode(rootPath).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;

                        List<string> foundFilePath = new List<string>{rootPath};
                        string[] combination = currDirFile.Substring(rootPath.Length + 1).Split('\\');
                        for (int i = 0; i < combination.Length; i++)
                        {
                            string joined = foundFilePath[i] + "\\" + combination[i];
                            foundFilePath.Add(joined);
                            this.graph.FindNode(joined).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                            foreach (var edge in this.graph.Edges.ToArray())
                            {
                                if (edge.TargetNode.Id == foundFilePath[i])
                                {
                                    edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                                    break;
                                }
                            }
                        }

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
                    // Kalau file, beri warna merah untuk node file tersebut dan seluruh parent directory nya (jika berwarna hitam)
                    else
                    {
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                        e.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;

                        List<string> foundFilePath = new List<string> { rootPath };
                        string[] combination = currDirFile.Substring(rootPath.Length + 1).Split('\\');
                        for (int i = 0; i < combination.Length; i++)
                        {
                            string joined = foundFilePath[i] + "\\" + combination[i];
                            foundFilePath.Add(joined);
                            var joinedNode = this.graph.FindNode(joined);
                            if (joinedNode.Attr.Color.Equals(Microsoft.Msagl.Drawing.Color.Black))
                            {
                                joinedNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                            }
                            foreach (var edge in this.graph.Edges.ToArray())
                            {
                                if (edge.TargetNode.Id == foundFilePath[i])
                                {
                                    if (edge.Attr.Color.Equals(Microsoft.Msagl.Drawing.Color.Black))
                                    {
                                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    bgWorker.ReportProgress(0);
                }
            }


            Debug.WriteLine("Hasil Akhir :");
            Debug.WriteLine(String.Join(", ", results));

            return results;
        }

        public List<string> DFS(string mode,string fileSearched, string rootPath, BackgroundWorker bgWorker)
        {
            Stack<string> s = new Stack<string>();
            List<string> results = new List<string>();
            s.Push(rootPath);

            bool found=false;

            while (s.Any() && !found)
            {
                string temp = s.Pop();
                Debug.WriteLine(temp);
                DirectoryInfo dir = new DirectoryInfo(temp);
                DirectoryInfo[] childDirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                var parentNode = this.graph.AddNode(temp);
                var baseLabelText = temp.Split('\\');
                parentNode.LabelText = baseLabelText.Last();

                bgWorker.ReportProgress(0);
                Thread.Sleep(250);

                // Jika directory kosong, tandai dengan directory tersebut dengan warna merah serta warna parent nya (jika hitam)
                if (files.Length == 0)
                {
                    parentNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;

                    List<string> foundFilePath = new List<string> { rootPath };
                    string[] combination = temp.Substring(rootPath.Length + 1).Split('\\');
                    for (int i = 0; i < combination.Length; i++)
                    {
                        string joined = foundFilePath[i] + "\\" + combination[i];
                        foundFilePath.Add(joined);
                        var joinedNode = this.graph.FindNode(joined);
                        if (joinedNode.Attr.Color.Equals(Microsoft.Msagl.Drawing.Color.Black))
                        {
                            joinedNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                        }
                        foreach (var edge in this.graph.Edges.ToArray())
                        {
                            if (edge.TargetNode.Id == foundFilePath[i] || edge.TargetNode.Id == temp)
                            {
                                if (edge.Attr.Color.Equals(Microsoft.Msagl.Drawing.Color.Black))
                                {
                                    edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                                }
                                break;
                            }
                        }
                    }
                    bgWorker.ReportProgress(0);
                }

                foreach (FileInfo file in files){

                    Microsoft.Msagl.Drawing.Node node = this.graph.AddNode(file.FullName);
                    var e = new Microsoft.Msagl.Drawing.Edge(parentNode, node, Microsoft.Msagl.Drawing.ConnectionToGraph.Connected);

                    var labelText = node.LabelText.Split('\\');
                    node.LabelText = labelText.Last();
                    bgWorker.ReportProgress(0);
                    Thread.Sleep(250);

                    Debug.Write("Nama File :");
                    Debug.WriteLine(file);
                    if(file.Name==fileSearched){
                        string result = Path.Combine(temp,file.Name);
                        Debug.Write("Ketemu :");
                        Debug.WriteLine(result);
                        results.Add(result);

                        // Mewarnai hasil ketemu dan semua parent directory nya dengan warna biru
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                        this.graph.FindNode(rootPath).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;

                        List<string> foundFilePath = new List<string> { rootPath };
                        string[] combination = file.FullName.Substring(rootPath.Length + 1).Split('\\');
                        for (int i = 0; i < combination.Length; i++)
                        {
                            string joined = foundFilePath[i] + "\\" + combination[i];
                            foundFilePath.Add(joined);
                            this.graph.FindNode(joined).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                            foreach (var edge in this.graph.Edges.ToArray())
                            {
                                if (edge.TargetNode.Id == foundFilePath[i] || edge.TargetNode.Id == result && edge.Attr.Color == Microsoft.Msagl.Drawing.Color.Black)
                                {
                                    edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                                    break;
                                }
                            }
                        }

                        if (mode == "first")
                        {
                            found=true;
                        }
                    } else
                    {
                        // Tandai file yang bukan dicari menjadi merah, serta parent node nya
                        node.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                        e.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;

                        List<string> foundFilePath = new List<string> { rootPath };
                        string[] combination = file.FullName.Substring(rootPath.Length + 1).Split('\\');
                        for (int i = 0; i < combination.Length; i++)
                        {
                            string joined = foundFilePath[i] + "\\" + combination[i];
                            foundFilePath.Add(joined);
                            var joinedNode = this.graph.FindNode(joined);
                            if (joinedNode.Attr.Color.Equals(Microsoft.Msagl.Drawing.Color.Black))
                            {
                                joinedNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                            }
                            foreach (var edge in this.graph.Edges.ToArray())
                            {
                                if (edge.TargetNode.Id == foundFilePath[i])
                                {
                                    if (edge.Attr.Color.Equals(Microsoft.Msagl.Drawing.Color.Black))
                                    {
                                        edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                foreach(DirectoryInfo childDir in childDirs){
                    s.Push(Path.Combine(temp,childDir.ToString()));
                    Microsoft.Msagl.Drawing.Node node = this.graph.AddNode(childDir.FullName);
                    var labelText = node.LabelText.Split('\\');
                    node.LabelText = labelText.Last();
                    var e = new Microsoft.Msagl.Drawing.Edge(parentNode, node, Microsoft.Msagl.Drawing.ConnectionToGraph.Connected);
                    bgWorker.ReportProgress(0);

                }
            }
            bgWorker.ReportProgress(0);

            Debug.WriteLine("Hasil Akhir :");
            Debug.WriteLine(String.Join(", ", results));

            return results;
        }
    }
}
