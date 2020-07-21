using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AntiBacklash
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.Text = "Z-backlashTool v1.0.0";
            InitializeComponent();
        }

        private void panel1_DragEnter_1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                AlterG(file);
            }
            DialogResult result = MessageBox.Show("Do you want to exit the application?", "All done", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void AlterG(string file)
        {
            string txt = File.ReadAllText(file);
            Regex expr = new Regex(@"(G0\s.*?\sZ)(.*)");
            Match match = expr.Match(txt);
            string str;
            match = match.NextMatch(); // first match was not needed
            while (match.Success)
            {
                str = match.Groups[2].Value;
                double oldZ = double.Parse(str, CultureInfo.InvariantCulture);
                double newZ = oldZ + ((Convert.ToDouble(numericUpDown1.Value)/10));
                string newVal = newZ.ToString(CultureInfo.InvariantCulture);
                txt = txt.Replace(match.Groups[0].Value, match.Groups[1].Value + newVal);
                match = match.NextMatch();
            }
            File.WriteAllText(file.Replace(".g", "_out.g"), txt);
        }
    }
}
