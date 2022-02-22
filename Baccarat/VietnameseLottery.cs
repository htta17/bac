using CoreLogic.VietnameseLottery;
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
    public partial class VietnameseLottery : Form
    {
        public VietnameseLottery()
        {
            InitializeComponent();
        }
        int[] arr = new int[100];

        private void btnGo_Click(object sender, EventArgs e)
        { 
            var num1 = (int) Math.Round(txtNum1.Value);
            var num2 = (int)Math.Round(txtNum2.Value);
            var num3 = (int)Math.Round(txtNum3.Value);
            var num4 = (int)Math.Round(txtNum4.Value);


            var firstDigit1 = num1 / 10;
            var firstDigit2 = num2 / 10;
            var firstDigit3 = num3 / 10;
            var firstDigit4 = num4 / 10;

            var secondDigit1 = num1 % 10;
            var secondDigit2 = num2 % 10;
            var secondDigit3 = num3 % 10;
            var secondDigit4 = num4 % 10;

           
            var total = 0;

            var range1 = new VL_Digit_Predict(firstDigit1, firstDigit2, firstDigit3, firstDigit4);
            var range2 = new VL_Digit_Predict(secondDigit1, secondDigit2, secondDigit3, secondDigit4);
            for (int first = 0; first <= 9; first++)
                for (int second = 0; second <= 9; second++)
                {
                    var x1 = first == 0 ? range1.Range.Ret0
                        : first == 1 ? range1.Range.Ret1
                        : first == 2 ? range1.Range.Ret2
                        : first == 3 ? range1.Range.Ret3
                        : first == 4 ? range1.Range.Ret4
                        : first == 5 ? range1.Range.Ret5
                        : first == 6 ? range1.Range.Ret6
                        : first == 7 ? range1.Range.Ret7
                        : first == 8 ? range1.Range.Ret8
                        : range1.Range.Ret9;
                    var x2 = second == 0 ? range1.Range.Ret0
                        : second == 1 ? range2.Range.Ret1
                        : second == 2 ? range2.Range.Ret2
                        : second == 3 ? range2.Range.Ret3
                        : second == 4 ? range2.Range.Ret4
                        : second == 5 ? range2.Range.Ret5
                        : second == 6 ? range2.Range.Ret6
                        : second == 7 ? range2.Range.Ret7
                        : second == 8 ? range2.Range.Ret8
                        : range2.Range.Ret9;

                    arr[first * 10 + second] = x1 + x2;
                    total += x1 + x2;
                }

            for (var i = 0; i <= 99; i++)
            {
                Display(i,arr[i]);
            }
            txtTotal.Value = total;
        }

        void Display(int number, int value)
        {
            
            foreach (Control child in this.Controls)
            {
                if (child is Label && 
                    ((child.Name == "lb_" + number.ToString() && number >= 10) ||
                    (child.Name == "lb_0" + number.ToString() && number < 10))
                   )
                {
                    child.Text = value.ToString();
                }
            }


        }

        int ReserveNumber = -1;
        private void btnReload_Click(object sender, EventArgs e)
        {
            var index = (int)txtNext.Value;
            txtUnit.Value = arr[index];

            ReserveNumber = (int)txtNum1.Value;

            txtNum1.Value = txtNum2.Value;
            txtNum2.Value = txtNum3.Value;
            txtNum3.Value = txtNum4.Value;
            txtNum4.Value = txtNext.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtNext.Value = txtNum4.Value;
            txtNum4.Value = txtNum3.Value;
            txtNum3.Value = txtNum2.Value;
            txtNum2.Value = txtNum1.Value;
            txtNum1.Value = ReserveNumber;
        }
    }
}
