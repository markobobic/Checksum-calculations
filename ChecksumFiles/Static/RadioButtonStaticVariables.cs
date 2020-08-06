using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChecksumFiles.Static
{
   internal static class RadioButtonStaticVariables
    {
        public static List<RadioButton> RadioButtonsChecksumAlgoritham { get; set; } = new List<RadioButton>();
        public static List<RadioButton> RadioButtonsExtensionFiles { get; set; } = new List<RadioButton>();

        public static string ExtensionType { get; set; }
        public static string ChecksumType { get; set; }
    }
}
