using ChecksumFiles.Models;
using ChecksumFiles.Static;
using ChecksumFiles.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChecksumFiles.BusinessLogic
{
  internal class FileDetailsLogic
    {
        private ChecksumCalculation calculation;

        public FileDetailsLogic(ChecksumCalculation calculation)
        {
            this.calculation = calculation;
        }
        public FileDetailsLogic()
        {

        }
        public void FileInfo(out string fileName, out string checksumSHA512, out string calculatedChecksum, out string sizeOfFile, out string fileAtribbutes, out string creationTime, string path)
        {

            FileInfo oFileInfo = new FileInfo(path);
            fileName = Path.GetFileNameWithoutExtension(path);
            if (RadioButtonStaticVariables.ChecksumType == null)
            {
                
                calculatedChecksum = calculation.CalculateMD5(path);
                checksumSHA512 = calculation.CalculateSHA512(path);
            }
            else
            {
                calculatedChecksum = calculation.CalculateSelectedAlgoritham(path);
                checksumSHA512 = calculation.CalculateSHA512(path);
            }
            sizeOfFile = oFileInfo.Length.ToString();
            fileAtribbutes = File.GetAttributes(path).ToString();
            creationTime = oFileInfo.CreationTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

        }

    }
}
