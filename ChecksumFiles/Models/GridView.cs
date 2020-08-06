using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecksumFiles.Models
{
    public class GridView
    {
        public string NameOfFile { get; set; }
        public string ChecksumSHA512 { get; set; }
        public string FileAttributes { get; set; }
        private string fileSize;

        public string FileSize
        {
            get { return fileSize + " " + "kb"; }
            set { fileSize = value; }
        }
        public string CreationTime { get; set; }

        public GridView(string nameOfFile, string checkSum512, string fileSize,string fileAttributes, string creationTime)
        {
            NameOfFile = nameOfFile;
            ChecksumSHA512 = checkSum512;
            FileSize = fileSize;
            FileAttributes = fileAttributes;
            CreationTime = creationTime;

        }
        public GridView()
        {
        }

       
    }
}
