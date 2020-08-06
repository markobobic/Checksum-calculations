using ChecksumFiles.BusinessLogic;
using ChecksumFiles.Models;
using ChecksumFiles.Static;
using ChecksumFiles.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaitingBox;

namespace ChecksumFiles
{
    public partial class Form1 : Form
    {
        List<GridView> filesAndCheckSums = new List<GridView>();
        private ChecksumCalculation calculationLogic;
        private RadioButtonLogic radioButtonLogic;
        private GenerateGrid gridLogic;
        private FileDetailsLogic fileDetailsLogic;

        public Form1()
        {
            InitializeComponent();
            calculationLogic = new ChecksumCalculation();
            radioButtonLogic = new RadioButtonLogic();
            gridLogic = new GenerateGrid();
            gridLogic.DesignGrid(new DataGridView[] {dataGridView1,dataGridView2 });
            fileDetailsLogic = new FileDetailsLogic(calculationLogic);
        }
        
        private async void Browse_Click(object sender, EventArgs e)

        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please select a folder which you want to calculate checksum of files.";
            fbd.ShowNewFolderButton = false;
            ShowWaitingBox waiting = new ShowWaitingBox("Processing", "Please wait for calculation..");
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                List<GridViewModel> filesAndChecksToPopulate = new List<GridViewModel>();
                string folderName = fbd.SelectedPath;
                try { 
                var directoryFiles = Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories);
                if (directoryFiles.Length > 10)
                {
                    waiting.Start();
                }
                if (directoryFiles.Length > 0)
                {
                    foreach (string path in directoryFiles)
                    {

                        if (RadioButtonStaticVariables.ExtensionType != null)
                        {
                            if (path.ToLower().EndsWith(RadioButtonStaticVariables.ExtensionType.ToLower()))
                            {
                                string fileName, calculatedChecksum, sizeOfFile, fileAtribbutes, creationTime, checksumSHA512;
                                fileDetailsLogic.FileInfo(out fileName, out checksumSHA512, out calculatedChecksum,
                                out sizeOfFile, out fileAtribbutes, out creationTime, path);
                                GridView allGridViews = new GridView(fileName, checksumSHA512,
                                sizeOfFile, fileAtribbutes, creationTime);
                                filesAndCheckSums.Add(allGridViews);
                                GridViewModel gridToPopulate = new GridViewModel(fileName, calculatedChecksum);
                                filesAndChecksToPopulate.Add(gridToPopulate);
                            }
                        }
                        if (RadioButtonStaticVariables.ExtensionType == null || RadioButtonStaticVariables.ExtensionType == "All")
                        {
                            string fileName, calculatedChecksum, sizeOfFile, fileAtribbutes, creationTime, checksumSHA512;
                            fileDetailsLogic.FileInfo(out fileName, out checksumSHA512, out calculatedChecksum,
                            out sizeOfFile, out fileAtribbutes, out creationTime, path);
                            GridView allGridViews = new GridView(fileName, checksumSHA512,
                            sizeOfFile, fileAtribbutes, creationTime);
                            filesAndCheckSums.Add(allGridViews);
                            GridViewModel gridToPopulate = new GridViewModel(fileName, calculatedChecksum);
                            filesAndChecksToPopulate.Add(gridToPopulate);
                        }

                    }
                    bool isListEmpty1 = filesAndChecksToPopulate.Any();
                    if (!isListEmpty1)
                    {
                        await Task.Run(async () => 
                        {
                            await Task.Delay(3000);
                            waiting.Stop();
                            MessageBox.Show
                        ($"There are no files with selected {RadioButtonStaticVariables.ExtensionType} extension.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        });

                        gridLogic.GenerateGridView(filesAndChecksToPopulate, dataGridView1);

                    }


                    gridLogic.GenerateGridView(filesAndChecksToPopulate, dataGridView1);
                }
                else
                {
                    var pathLastName = new DirectoryInfo(folderName).Name;
                    MessageBox.Show
                 ($"Selected folder {pathLastName} is empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridLogic.GenerateGridView(filesAndChecksToPopulate, dataGridView1);
                }
                bool isListEmpty = filesAndChecksToPopulate.Any();

                    if (directoryFiles.Length > 10 && isListEmpty == true)
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(3000);
                            waiting.Stop();
                        });
                        }
            }
                catch (Exception ex)
                {
                    if (ex is UnauthorizedAccessException) { 
                        MessageBox.Show
                        ($"Selected folder is authorized.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        MessageBox.Show
                    ($"Something went wrong.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
                 

        }
       
        private void CustomFileExtension_Click(object sender, EventArgs e)
        {
            radioButtonLogic.CheckedValueForExtensions();
        }

        private void CustomChecksumAlgoritham_Click(object sender, EventArgs e)
        {
            radioButtonLogic.CheckedValueForChecksums();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string fileName = dataGridView1.SelectedRows[0].Cells["FileName"].Value.ToString();
            var file = filesAndCheckSums.Where(x => x.NameOfFile == fileName).FirstOrDefault();
            List<GridView> list = new List<GridView>();
            list.Add(file);
            gridLogic.GenerateGridView(list, dataGridView2);
        }


    }
}


