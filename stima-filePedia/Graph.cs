using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace stima_filePedia
{
    public delegate void FileFound(string finalPath);

    public class Graph
    {
        private string rootPath;
        
        public event FileFound FinishSearch;

        public Graph(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public int Count { get; set; }
        public string RootPath { get { return this.rootPath; } }

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
                Debug.WriteLine(temp);

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
                foreach(FileInfo file in files){
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
        }
    }
}
