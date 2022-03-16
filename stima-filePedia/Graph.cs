using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stima_filePedia
{
    public delegate void FileFound(string finalPath);

    public class Graph
    {
        private string rootPath;
        private string fileSearched;
        private Queue<string> q = new Queue<string>();
        
        public event FileFound FinishSearch;

        public Graph(string rootPath, string fileSearched)
        {
            this.rootPath = rootPath;
            this.q.Enqueue(rootPath);
        }

        public Queue<string> Q { get; set; }
        public int Count { get; set; }
        public string RootPath { get { return this.rootPath; } }
        public string FileSearched { get { return this.fileSearched; } }

        private void BFS2(string fileSearched)
        {
            while(q.Any())
            {
                string temp = this.q.Dequeue();
                string[] dirsNfiles = Directory.GetFileSystemEntries(temp, "*", SearchOption.TopDirectoryOnly);

                foreach (string currDirFile in dirsNfiles)
                {
                    // Mencocokkan nama File, file yang dicari ditemukan
                    if (File.Exists(currDirFile) && (Path.GetFileName(currDirFile) == fileSearched))
                    {
                        FinishSearch(currDirFile);
                    }
                    // Kalau directory, append ke Q
                    else if (Directory.Exists(currDirFile))
                    {
                        Q.Enqueue(currDirFile);
                    }
                }
            }

            FinishSearch(null);
        }

        public void BFS()
        {
            BFS2(this.fileSearched);
        }

        public void DFS(string mode)
        {
            Stack<string> s;
            s.Push(this.rootPath);
            while(s.Any())
            {
                string temp = s.Pop();
                DirectoryInfo dir = new DirectoryInfo(temp);
                DirectoryInfo[] childDirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                foreach(FileInfo file in files){
                    if(file.name==this.fileSearched){
                        FinishSearch(Path.Combine(temp,file.name)); //auto berhenti
                    }
                }
                foreach(DirectoryInfo childDir in childDirs){
                    s.Push(Path.Combine(temp,childDir.ToString()));
                }
            }

            FinishSearch(null);
        }
    }
}
