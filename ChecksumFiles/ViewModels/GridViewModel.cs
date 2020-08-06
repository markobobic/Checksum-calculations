using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecksumFiles.ViewModels
{
   public class GridViewModel
    {
        public string FileName { get; set; }
        public string Checksum { get; set; }
        public GridViewModel(string fileName, string checkSum)
        {
            FileName = fileName;
            Checksum = checkSum;
        }
    }
}
