using ChecksumFiles.Enums;
using ChecksumFiles.Models;
using ChecksumFiles.Static;
using ChecksumFiles.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChecksumFiles
{
    public partial class Form1 : Form
    {
        List<GridView> filesAndCheckSums = new List<GridView>();
        public Form1()
        {
            InitializeComponent();
            DesignGrid();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please select a folder which you want to calculate checksum of files.";
            fbd.ShowNewFolderButton = false;
            List<GridViewModel> filesAndChecksToPopulate = new List<GridViewModel>();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string folderName = fbd.SelectedPath;
                foreach (string path in Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories))
                {
                    if (SelectedExtensionType.ExtensionType != null)
                    {
                        if (path.ToLower().EndsWith(SelectedExtensionType.ExtensionType.ToLower()))
                        {
                            string fileName, calculatedChecksum, sizeOfFile, fileAtribbutes, creationTime, checksumSHA512;
                            FileInfo(out fileName, out checksumSHA512, out calculatedChecksum,
                            out sizeOfFile, out fileAtribbutes, out creationTime, path);
                            GridView allGridViews = new GridView(fileName, calculatedChecksum,
                            sizeOfFile, fileAtribbutes, creationTime);
                            filesAndCheckSums.Add(allGridViews);
                            GridViewModel gridToPopulate = new GridViewModel(fileName, calculatedChecksum);
                            filesAndChecksToPopulate.Add(gridToPopulate);
                        }
                    }
                    else
                    {
                        string fileName, calculatedChecksum, sizeOfFile, fileAtribbutes, creationTime, checksumSHA512;
                        FileInfo(out fileName, out checksumSHA512, out calculatedChecksum,
                        out sizeOfFile, out fileAtribbutes, out creationTime, path);
                        GridView allGridViews = new GridView(fileName, calculatedChecksum,
                        sizeOfFile, fileAtribbutes, creationTime);
                        filesAndCheckSums.Add(allGridViews);
                        GridViewModel gridToPopulate = new GridViewModel(fileName, calculatedChecksum);
                        filesAndChecksToPopulate.Add(gridToPopulate);
                    }

                }
                GenerateGridView(filesAndChecksToPopulate);
            }
        }

        private void FileInfo(out string fileName,out string checksumSHA512, out string calculatedChecksum, out string sizeOfFile, out string fileAtribbutes, out string creationTime, string path)
        {
            
            FileInfo oFileInfo = new FileInfo(path);
            fileName = Path.GetFileNameWithoutExtension(path);
            if (SelectedChecksumType.ChecksumType == null)
            {
                calculatedChecksum = CalculateMD5(path);
                checksumSHA512 = CalculateSHA512(path);
            }
            else
            {
                calculatedChecksum = CalculateSelectedAlgoritham(path);
                checksumSHA512 = CalculateSHA512(path);
            }
            sizeOfFile = oFileInfo.Length.ToString();
            fileAtribbutes = File.GetAttributes(path).ToString();
            creationTime = oFileInfo.CreationTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
           
        }

        private string CalculateSelectedAlgoritham(string path)
        {
            string selectedAlgoritham = SelectedChecksumType.ChecksumType;
            switch (selectedAlgoritham)
            {
                case "SHA1":
                  return CalculateSHA1(path);
                case "SHA256":
                    return CalculateSHA256(path);
                case "SHA384":
                    return CalculateSHA384(path);
                case "SHA512":
                    return CalculateSHA512(path);
                case "MD5":
                    return CalculateMD5(path);
               
            }
            return CalculateMD5(path);
        }

        private string CalculateSHA384(string path)
        {
            using (var sha384 = SHA384.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = sha384.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private string CalculateSHA1(string path)
        {
            using (var sha1 = SHA1.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = sha1.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private string CalculateSHA256(string path)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private string CalculateSHA512(string filename)
        {
            using (var sha512 = SHA512.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = sha512.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private void CustomExtension_Click(object sender, EventArgs e)
        {
            CheckedValueForExtensions();
        }

        private void CheckedValueForExtensions()
        {
            Form form2 = new Form();
            form2.AutoScroll = true;
            var controls = GenerateRadioButtons(GetFileExtensionList(),FilterTypes.FileExtension);
            foreach (var singleControl in controls)
            {
                form2.Controls.Add(singleControl);
            }
            form2.Show();

            form2.FormClosed += new FormClosedEventHandler(FormExtensionClosed);

        }

        private void CheckedValueForChecksums()
        {
            Form form3 = new Form();
            form3.AutoScroll = true;
            ImmutableList<string> listOfAlgorithams = ImmutableList.Create<string>("SHA1", "SHA256", "SHA384", "SHA512");
            var controls = GenerateRadioButtons(listOfAlgorithams,FilterTypes.ChecksumAlgoritham);
            foreach (var singleControl in controls)
            {
                form3.Controls.Add(singleControl);
            }
            form3.Show();

            form3.FormClosed += new FormClosedEventHandler(FormChecksumsClosed);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        new void FormExtensionClosed(object sender, FormClosedEventArgs e)
        {
            if (RadioButtonsExtension.RadioButtons.Any(x => x.Checked))
            {
                var selected = RadioButtonsExtension.RadioButtons.Where(x => x.Checked).LastOrDefault();
                MessageBox.Show($"You selected {selected.Text}. Click Browse to checksum {selected.Text} files.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SelectedExtensionType.ExtensionType = selected.Text;

            }
        }

         new void FormChecksumsClosed(object sender, FormClosedEventArgs e)
        {
            if (RadioButtonsChecksum.RadioButtons.Any(x => x.Checked))
            {
                var selected = RadioButtonsChecksum.RadioButtons.Where(x => x.Checked).LastOrDefault();
                MessageBox.Show($"You selected {selected.Text}. Click Browse to checksum files by {selected.Text} algorithm.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SelectedChecksumType.ChecksumType = selected.Text;

            }
        }

        private void GenerateGridView(List<GridViewModel> filesAndCheckSums)
        {
            dataGridView1.DataSource = filesAndCheckSums;
        }
        private static ImmutableList<string> GetFileExtensionList()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("ChecksumFiles.FilesExtenions.txt"))
            using (var reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                string[] array = text.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);
                return ImmutableList<string>.Empty.AddRange(array);

            }
        }

        

        private void DesignGrid()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ReadOnly = true;
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string fileName = dataGridView1.SelectedRows[0].Cells["FileName"].Value.ToString();
            var file = filesAndCheckSums.Where(x => x.NameOfFile == fileName).FirstOrDefault();
            List<GridView> list = new List<GridView>();
            list.Add(file);
            GenerateGridViewForFileDetails(list);
        }

        private void GenerateGridViewForFileDetails(List<GridView> files)
        {
           

            dataGridView2.DataSource = files;
        }

        private List<Control> GenerateRadioButtons(ImmutableList<string> list,FilterTypes type)
        {
            var controls = new List<Control>();
            var location = new Point(0, 0);
            foreach (var item in list.OrderBy(name => name))
            {
                RadioButton radioButton = new RadioButton();
                radioButton.Name = item;
                radioButton.Text = item;
                radioButton.Location = location;
                if(FilterTypes.FileExtension == type) { 
                RadioButtonsExtension.RadioButtons.Add(radioButton);
                }
                else if(FilterTypes.ChecksumAlgoritham == type)
                {
                 RadioButtonsChecksum.RadioButtons.Add(radioButton);
                }
                controls.Add(radioButton);
                location.Y = location.Y + radioButton.Height;

            }
            return controls;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckedValueForChecksums();
        }

        

        /*
         *  public string FileName { get; set; }
        public string Checksum { get; set; }
        public string FileAtribbutes { get; set; }
        public string FileSize { get; set; }
        public string CreationTime { get; set; }
         */



    }
}


