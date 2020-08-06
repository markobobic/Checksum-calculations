using ChecksumFiles.Enums;
using ChecksumFiles.ExtensionMethods;
using ChecksumFiles.Static;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChecksumFiles.BusinessLogic
{
  internal class RadioButtonLogic
    {
        public void CheckedValueForExtensions()
        {
            Form form = new Form();
            DesingForm(form);
            var controls = GenerateRadioButtons(GetFileExtensionList(), FilterTypes.FileExtension);
            foreach (var singleControl in controls)
            {
                form.Controls.Add(singleControl);
            }
            form.Show();
        }

        public void CheckedValueForChecksums()
        {
            Form form = new Form();
            DesingForm(form);
            ImmutableList<string> listOfAlgorithams = ImmutableList.Create<string>("SHA1", "SHA256", "SHA384", "SHA512");
            var controls = GenerateRadioButtons(listOfAlgorithams, FilterTypes.ChecksumAlgoritham);
            foreach (var singleControl in controls)
            {
                form.Controls.Add(singleControl);
            }
            form.Show();
        }
        //getting text from embeded resource FilesExtension.Txt and populating immutableList with it
        public static ImmutableList<string> GetFileExtensionList()
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
        public bool IsRadioButtonChecked(List<RadioButton> radioButtons,FilterTypes type)
        {
            if (radioButtons.Any(x => x.Checked))
            {
                var selected = radioButtons.Where(x => x.Checked).LastOrDefault();

                if(FilterTypes.ChecksumAlgoritham == type) {
                   MessageBox.Show
                  ($"You selected {selected.Text}. Click Browse to checksum files by {selected.Text} algorithm.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   RadioButtonStaticVariables.ChecksumType = selected.Text;
                } else if(FilterTypes.FileExtension == type)
                {
                    MessageBox.Show
                ($"You selected {selected.Text}. Click Browse to checksum only files by {selected.Text} extension.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RadioButtonStaticVariables.ExtensionType = selected.Text;
                   
                }
                return true;
            }
            return false;
        }


        public void DesingForm(Form form)
        {
            form.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            form.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(210)))), ((int)(((byte)(245)))));
            form.ClientSize = new System.Drawing.Size(150, 200);
            form.Name = "Form2";
            form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            form.Text = "";
            form.ResumeLayout(false);
            form.PerformLayout();
            form.AutoScroll = true;
            form.ShowIcon = false;
            form.MinimizeBox = false;
            form.MinimizeBox = false;
           
           
        }
        public  List<Control> GenerateRadioButtons(ImmutableList<string> list, FilterTypes type)
        {
            var controls = new List<Control>();
            var location = new Point(0, 0);
            foreach (var item in list.OrderByDescending(name => name == "All").ThenBy(name => name))
            {

                RadioButton radioButton = new RadioButton();
                radioButton.Padding = new Padding(50, 5, 0, 0);
                if (FilterTypes.FileExtension==type) { 
                radioButton.Click += radioExtension_Click;
                }
                if (FilterTypes.ChecksumAlgoritham==type)
                {
                    radioButton.Click += radioChecksum_Clcik;
                }
                radioButton.Name = item;
                radioButton.Text = item;
                radioButton.Location = location;
                if (FilterTypes.FileExtension == type)
                {
                    RadioButtonStaticVariables.RadioButtonsExtensionFiles.Add(radioButton);
                }
                else if (FilterTypes.ChecksumAlgoritham == type)
                {
                    RadioButtonStaticVariables.RadioButtonsChecksumAlgoritham.Add(radioButton);
                }
                controls.Add(radioButton);
                location.Y = location.Y + radioButton.Height;

            }
            return controls;
        }

        private void radioChecksum_Clcik(object sender, EventArgs e)
        {
            IsRadioButtonChecked(RadioButtonStaticVariables.RadioButtonsChecksumAlgoritham,
             FilterTypes.ChecksumAlgoritham);
        }

        private void radioExtension_Click(Object sender, EventArgs e)
        {
            IsRadioButtonChecked(RadioButtonStaticVariables.RadioButtonsExtensionFiles,
              FilterTypes.FileExtension);
        }

    }
}
