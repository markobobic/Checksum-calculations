using ChecksumFiles.Enums;
using ChecksumFiles.Static;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChecksumFiles.ExtensionMethods
{
  public static class Extension
    {
        public static List<Control> GenerateRadioButtons(this ImmutableList<string> list, FilterTypes type)
        {
            var controls = new List<Control>();
            var location = new Point(0, 0);
            foreach (var item in list.OrderByDescending(name => name == "All").ThenBy(name=>name))
            {
               
                RadioButton radioButton = new RadioButton();
                radioButton.Padding = new Padding(50,5,0,0);
                radioButton.Click += radio_Click;
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

        private static void radio_Click(object sender, EventArgs e)
        {
            MessageBox.Show("cao");
        }

        public static int ToInt321(this string value)
		{
			int result;
			if (int.TryParse(value, out result))
			{
				return result;
			}

			return 0;
		}

    }
}
