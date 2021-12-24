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
    public partial class BaccaratBankerOnly : Form
    {
        public BaccaratBankerOnly()
        {
            InitializeComponent();
        }        

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (txtResult.Text == "")
            {
                MessageBox.Show("Xin mời nhập kết quả");
            }
            
            if (txtResult.Text == "1")
            {
                if (txtVolume.Value == 1)
                    txtVolume.Value = 1; 
                else
                    txtVolume.Value = txtVolume.Value - 2;
            }
            else 
            {
                txtVolume.Value = txtVolume.Value + 2;
            }

            timer1.Enabled = true;
            lblAlert.Visible = true;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            var text = (sender as Button).Text;
            txtResult.Text = text;  
        }

        private void txtResult_DoubleClick(object sender, EventArgs e)
        {
            txtResult.Text = ""; 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            lblAlert.Visible = false;
        }
    }
}
