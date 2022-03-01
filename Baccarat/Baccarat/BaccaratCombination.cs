using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoreLogic;

namespace Baccarat
{
    public partial class BaccaratCombination : Form
    {
        public BaccaratCombination()
        {
            InitializeComponent();            
            ArrayLength = Controls.OfType<TextBox>().Where(c => c.Name.IndexOf("txt_") == 0).Count(); //Volumn and Value

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            fileName = string.Format(FileFormatCSV, DateTime.Now);
            File.AppendAllText(string.Format("Logs\\{0}", fileName), LogTitle);
            
        }

        public int ArrayLength { get; set; } = 0;

        private int Counter = 0;

        private string fileName = null;

        private const string LogTitle = "Time,Next Value, Current Value, Volume\r\n";
        private const string FileFormatCSV = "{0:yyyyMMdd_HHmmss}.csv";

        private void btnNumber_Click(object sender, EventArgs e)
        {
            var text = (sender as Button).Text;
            var value = Convert.ToInt32(text);

            var buttonName = (sender as Button).Name;
            var textBoxName = buttonName.Substring(3, buttonName.Length - 4);

            var textBox = this.Controls.Find("txt_" + textBoxName, false).First() as TextBox;

            textBox.Text = text;
        }

        private void btnShiftLeft_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= ArrayLength - 1; i++) 
            {
                var currentTextBox = Controls.Find("txt_" + i.ToString(), false).First() as TextBox;
                var nextTextBox = Controls.Find("txt_" + (i + 1).ToString(), false).First() as TextBox;

                currentTextBox.Text = nextTextBox.Text;
            }
            Controls.Find("txt_" + ArrayLength.ToString(), false).First().Text = string.Empty;
        }

        private void btnShiftRight_Click(object sender, EventArgs e)
        {
            for (int i = ArrayLength; i >= 2; i--)
            {
                var currentTextBox = Controls.Find("txt_" + i.ToString(), false).First() as TextBox;
                var previousTextBox = Controls.Find("txt_" + (i - 1).ToString(), false).First() as TextBox;

                currentTextBox.Text = previousTextBox.Text;
            }
            txt_1.Text = string.Empty;
        }

        private void txt_1_TextChanged(object sender, EventArgs e)
        {
            var textBox = (sender as TextBox);
            textBox.ForeColor = textBox.Text == "1" ? Color.Red : textBox.Text == "0" ? Color.Blue : Color.Black;

            textBox.BackColor = textBox.Text == "" ? Color.White : Color.Gray;
        }

        private UIValidationEnum ValidateInputs()
        {
            var firstEmpty = ArrayLength;
            if ((txt_1.Text != "0") && (txt_1.Text != "1"))
            {
                return UIValidationEnum.NeedFirstValue;
            }
            for (var i = 1; i <= ArrayLength; i++)
            {
                var textbox = Controls.Find("txt_" + i.ToString(), false).First() as TextBox;  
                if ((textbox.Text != "0") && (textbox.Text != "1") && (textbox.Text != ""))
                {
                    return UIValidationEnum.WrongValue;
                }
                else 
                {
                    if (textbox.Text == "")
                        firstEmpty = i;
                    else if (i > firstEmpty)
                    {
                        return UIValidationEnum.HasEmptyInMiddle;
                    }
                }
            }

            return UIValidationEnum.Success;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            var validatorInputs = ValidateInputs();
            if (validatorInputs == UIValidationEnum.WrongValue)
            {
                MessageBox.Show("Có giá trị đầu vào sai");
                return;
            }
            else if (validatorInputs == UIValidationEnum.HasEmptyInMiddle)
            {
                MessageBox.Show("Có ô trống ở giữa 2 ô có số");
                return;
            }
            else if (validatorInputs == UIValidationEnum.NeedFirstValue)
            {
                MessageBox.Show("Xin mời cung cấp giá trị đầu tiên");
                return;
            }
            
            var inputs = new List<int>();
            for (var i = 1; i <= ArrayLength; i++)
            {
                var textbox = Controls.Find("txt_" + i.ToString(), false).First() as TextBox;
                if ((textbox.Text == "0") || (textbox.Text == "1"))
                {
                    var value = Convert.ToInt32(textbox.Text);
                    inputs.Add(value);
                }
            }

            var calculator = new BaccaratCombinationCalculator();
            
            var baccaratResult = calculator.Predict(inputs.ToArray());
            txtValue.Text = baccaratResult.Value.ToString();
            txtVolume.Text = baccaratResult.Volume.ToString();
            txtFinalVolume.Text = (baccaratResult.Volume * (int)numCoeff.Value).ToString();

            if (baccaratResult.Value == BaccratCard.Banker)
            {
                txtValue.ForeColor = Color.Red;
                txtVolume.ForeColor = Color.Red;
                txtFinalVolume.ForeColor = Color.Red;
            }
            else if (baccaratResult.Value == BaccratCard.Player)
            {
                txtValue.ForeColor = Color.Blue;
                txtVolume.ForeColor = Color.Blue;
                txtFinalVolume.ForeColor = Color.Blue;
            }
            else
            {
                txtValue.ForeColor = Color.Black;
                txtVolume.ForeColor = Color.Black;
                txtFinalVolume.ForeColor = Color.Black;
            }

            Counter++;
            lblCounter.Value = Counter;


            //Save data
            File.AppendAllText(string.Format("Logs\\{0}", fileName), 
                string.Format("{0:yyyy-MM-dd HH:mm:ss},{1},{2},{3}\r\n", DateTime.Now, baccaratResult.Value,inputs.Last(), baccaratResult.Volume));
        }

        private void txt_1_DoubleClick(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa toàn bộ số liệu trên các ô chứ?", "Quan trọng lắm, đọc kĩ nè!!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int i = 1; i <= ArrayLength; i++)
                {
                    var textbox = Controls.Find("txt_" + i.ToString(), false).First() as TextBox;
                    textbox.Text = "";
                    Counter = 0;
                    lblCounter.Value = Counter;                    
                }

                fileName = string.Format(FileFormatCSV, DateTime.Now);
                File.AppendAllText(string.Format("Logs\\{0}", fileName), LogTitle);
            }
        }

        private void lblCounter_ValueChanged(object sender, EventArgs e)
        {
            Counter = (int)lblCounter.Value;
        }

        private void btn10_MouseHover(object sender, EventArgs e)
        {
            //SavedColor = (sender as Button).BackColor;
            (sender as Button).BackColor = Color.LimeGreen;
        }
        //Color SavedColor = Color.White; 
        private void btn120_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = SystemColors.Control;
        }

        private void Baccarat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                btnCalculate_Click(null, null);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = textBox1.Text;
        }
    }
}
