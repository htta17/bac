using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baccarat
{
    public partial class UtilCal : Form
    {
        public UtilCal()
        {
            InitializeComponent();            
        }

        string ExportFilePath = null;
        string[] CheckArr = new string[] { "B", "P" };

        private void button1_Click(object sender, EventArgs e)
        {
            ExportFilePath = string.Format("{0:yyyyMMdd_HHmmss}.csv", DateTime.Now);

            // Read the file and display it line by line.  
            foreach (string line in System.IO.File.ReadLines(@"D:\test.csv"))
            {
                if (line.Trim() == "" || line.IndexOf("Shoe") >= 0)
                {
                    //Do nothing
                }
                else
                {
                    var list = line.Split(',');
                    if (list.Length == 9 && CheckArr.Contains(list[8]))
                    {
                        System.IO.File.AppendAllText(ExportFilePath, list[8] == "B" ? "1\r\n" : "-1\r\n");
                    }
                }
            }
        }
    }
}
