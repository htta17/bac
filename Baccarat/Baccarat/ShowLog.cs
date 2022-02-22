using CoreLogic;
using System;
using System.Collections.Generic;
using Midas.Utils;
using System.Windows.Forms;
using System.Drawing;

namespace Midas.Baccarat
{
    public partial class ShowLog : Form
    {
        public ShowLog(int N, List<BaccaratPredict> baccaratPredicts)
        {
            InitializeComponent();

            this.Text = $"Hiển thị {N} bước gần nhất";
            label1.Text = $"Dưới đây là {N} bước gần nhất";

            for (int i = 0; i < baccaratPredicts.Count; i++)
            {
                richTextBox1.AppendText(
                    $"{baccaratPredicts[i].Volume} ", 
                    baccaratPredicts[i].Value == BaccratCard.Banker ? Color.Red : Color.Blue
                    );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
