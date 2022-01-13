using CalculationLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Midas.Baccarat
{
    public partial class BaccaratRootAlg : Form
    {
        public BaccaratRootAlg()
        {
            this.KeyPreview = true;
            InitializeComponent();
        }

         

        private void btn51_Click(object sender, EventArgs e)
        {
            var text = (sender as Button).Text;

            var buttonName = (sender as Button).Name;
            var textBoxName = buttonName.Substring(3, buttonName.Length - 4);

            var textBox = this.Controls.Find("txt_" + textBoxName, false).First() as TextBox;

            textBox.Text = text;
        }

        private void btn10_MouseHover(object sender, EventArgs e)
        {
            var button = (sender as Button);
            if (button.Text == Constants.PLAYER_VALUE)
            {
                button.BackColor = Color.FromArgb(23, 120, 200);
            }
            else
            {
                button.BackColor = Color.FromArgb(255, 128, 0);
            }
        }
        private void btn120_MouseLeave(object sender, EventArgs e)
        {
            var button = (sender as Button);
            if (button.Text == Constants.PLAYER_VALUE)
            {
                button.BackColor = Color.Blue;
            }
            else
            {
                button.BackColor = Color.Red;
            }
            button.ForeColor = Color.White;
        }

        private void BaccaratQuad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                btnCalculate_Click(null, null);
                e.SuppressKeyPress = true;
            }            
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (txt_1.Text == "")
            {
                MessageBox.Show("Vui lòng nhập giá trị");
                return;
            }
            var inputValue = txt_1.Text == Constants.BANKER_VALUE
                               ? BaccratCard.Banker : BaccratCard.Player;

            ProcessInput(inputValue);

        }

        private void ProcessInput(BaccratCard inputValue)
        {
            //
            //ToDo: Change predict
            BaccaratPredict predict = new BaccaratPredict {Value = BaccratCard.NoTrade, Volume = 0 };

            txtValue.Text = predict.Value.ToString();
            txtValue.ForeColor = predict.Value == BaccratCard.NoTrade ? Color.Black :
                                            predict.Value == BaccratCard.Banker ? Color.Red : Color.Blue;
            txtVolume.ForeColor = txtValue.ForeColor;
            txtVolume.Text = predict.Volume.ToString();

            //ToDo: Change
            lbl_ClickedReport.Text = "Đã ghi nhận " + 0 + ": " + txt_1.Text;
            txt_1.Text = "";
        }

        private void txt_1_TextChanged(object sender, EventArgs e)
        {
            var textBox = (sender as TextBox);
            textBox.ForeColor = textBox.Text == Constants.BANKER_VALUE ? Color.Red : textBox.Text == Constants.PLAYER_VALUE ? Color.Blue : Color.Black;

            textBox.BackColor = textBox.Text == "" ? Color.White : Color.FromArgb(255, 255, 192);
        }

        private void txt_8_DoubleClick(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "";
        }
    }
}
