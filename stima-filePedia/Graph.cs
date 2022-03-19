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
            Dictionary<string, int> nameCount = new Dictionary<string, int>();

            while(q.Any() && !found)
            {
                string temp = q.Dequeue();
                string[] dirsNfiles = Directory.GetFileSystemEntries(temp, "*", SearchOption.TopDirectoryOnly);
                Debug.WriteLine("temp: " + temp);

                // GRAPHING START
                string[] subs = temp.Split('\\');
                foreach (string dir in Directory.GetDirectories(temp).Select(Path.GetFileName))
                {
                    this.graph.AddEdge(subs.Last(), dir).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }

                DirectoryInfo di = new DirectoryInfo(temp);
                foreach (FileInfo file in di.GetFiles())
                {
                    if (nameCount.ContainsKey(file.Name))
                    {
                        nameCount[file.Name] = nameCount[file.Name] + 1;
                        this.graph.AddEdge(subs.Last(), String.Format("{0} ({1})", file.Name, nameCount[file.Name])).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    } else
                    {
                        this.graph.AddEdge(subs.Last(), file.Name).Attr.Color = Microsoft.Msagl.Drawing.Color.Red; ;
                        nameCount.Add(file.Name, 1);
                    }
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

            // Graph coloring
            String basePath = rootPath.Split('\\').Last();
            int trimStart = rootPath.Substring(0, rootPath.Length - basePath.Length).Length;
            foreach (string result in results)
            {
                string[] pathList = result.Substring(trimStart).Split('\\');

                List<Microsoft.Msagl.Drawing.Edge> edgeList = new List<Microsoft.Msagl.Drawing.Edge>();
                for (int i = 0; i < pathList.Length - 1; i++)
                {
                    foreach (Microsoft.Msagl.Drawing.Edge edge in this.graph.FindNode(pathList[i]).Edges)
                    {
                        if (edge.Target.Equals(pathList[i + 1]))
                        {
                            edgeList.Add(edge);
                        }
                    }
                }

                Debug.WriteLine(edgeList);

                for (int i = 0; i < pathList.Length - 1; i++)
                {
                    // Remove existing edge to path
                    this.graph.RemoveEdge(edgeList[i]);

                    // Add colored edge and node
                    this.graph.AddEdge(pathList[i], pathList[i + 1]).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    this.graph.FindNode(pathList[i]).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    
                }
                this.graph.FindNode(pathList.Last()).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
            }
            
        }

        public void DFS(string mode,string fileSearched)
        {
            Stack<string> s = new Stack<string>();
            List<string> results = new List<string>();
            Dictionary<string, int> nameCount = new Dictionary<string, int>();
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
                string[] subs = temp.Split('\\');
                foreach (string d in Directory.GetDirectories(temp).Select(Path.GetFileName))
                {
                    this.graph.AddEdge(subs.Last(), d).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }

                DirectoryInfo di = new DirectoryInfo(temp);
                foreach (FileInfo file in di.GetFiles())
                {
                    if (nameCount.ContainsKey(file.Name))
                    {
                        nameCount[file.Name] = nameCount[file.Name] + 1;
                        this.graph.AddEdge(subs.Last(), String.Format("{0} ({1})", file.Name, nameCount[file.Name])).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    }
                    else
                    {
                        this.graph.AddEdge(subs.Last(), file.Name).Attr.Color = Microsoft.Msagl.Drawing.Color.Red; ;
                        nameCount.Add(file.Name, 1);
                    }
                }
                // GRAPHING END


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


            // Graph coloring
            String basePath = rootPath.Split('\\').Last();
            int trimStart = rootPath.Substring(0, rootPath.Length - basePath.Length).Length;
            foreach (string result in results)
            {
                string[] pathList = result.Substring(trimStart).Split('\\');

                List<Microsoft.Msagl.Drawing.Edge> edgeList = new List<Microsoft.Msagl.Drawing.Edge>();
                for (int i = 0; i < pathList.Length - 1; i++)
                {
                    foreach (Microsoft.Msagl.Drawing.Edge edge in this.graph.FindNode(pathList[i]).Edges)
                    {
                        if (edge.Target.Equals(pathList[i + 1]))
                        {
                            edgeList.Add(edge);
                        }
                    }
                }

                Debug.WriteLine(edgeList);

                for (int i = 0; i < pathList.Length - 1; i++)
                {
                    // Remove existing edge to path
                    this.graph.RemoveEdge(edgeList[i]);

                    // Add colored edge and node
                    this.graph.AddEdge(pathList[i], pathList[i + 1]).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                    this.graph.FindNode(pathList[i]).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;

                }
                this.graph.FindNode(pathList.Last()).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
            }
        }
    }
}
