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

        /*Format of a shoe in test.csv file: 
         * 
         * Shoe number 1
         * 3,2,-,3,5,-,5,8,B
         * 4,5,-,0,0,-,9,0,P
         * 8,3,0,4,9,0,1,3,B
         * ......
         * Shoe number 2 
         * 4,4,-,8,5,-,8,3,P
         * 2,9,1,5,9,-,2,4,B
         * ......
         */
        private void btnGetResult_Click(object sender, EventArgs e)
        {
            var exportFilePath = string.Format("{0:yyyyMMdd_HHmmss}.csv", DateTime.Now);
            string[] CheckArr = new string[] { "B", "P" };

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
                        System.IO.File.AppendAllText(exportFilePath, list[8] == "B" ? "1\r\n" : "-1\r\n");
                    }
                }
            }
        }
        //End of btnGetResult_Click
    }
}
