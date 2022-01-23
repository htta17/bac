using CalculationLogic;
using Microsoft.Win32;
using Midas.Configuration;
using Midas.Utils;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Midas.Baccarat
{
    public partial class BaccaratRootAlg : Form
    {
        public BaccaratRootAlg()
        {
            this.KeyPreview = true;
            InitializeComponent();

            if (string.IsNullOrEmpty(StartApp.GlobalConnectionString))
            {
                StartApp.LoadRegistryConnectionString();
            }

            BaccaratRootCalculator = new BaccaratRootCalculator(StartApp.GlobalConnectionString);

            var _moneyCoeff = Registry.GetValue(REG_PATH, REG_MONEY_COEFF_KEY, string.Empty).ToString();
            var _displayType = Registry.GetValue(REG_PATH, REG_MONEY_DISPLAYTYPE_KEY, string.Empty).ToString();

            
            if (string.IsNullOrEmpty(_moneyCoeff))
            {
                SetCoeffDisplay();
            }
            else
            {
                MoneyCoeff = int.Parse(_moneyCoeff) / 1000;
                lbUnit.Text = int.Parse(_displayType) == 1 ? ",000" : "K";
            }
            

            if (BaccaratRootCalculator.GlobalOrder > 0)
            {
                var lastcard = BaccaratRootCalculator.ShowLastCard();
                lbl_ClickedReport.Text = "Đã ghi nhận " + BaccaratRootCalculator.GlobalOrder + ": " + lastcard.ToString();
            }
            else
            {
                lbl_ClickedReport.Text = "Bắt đầu chơi";
            }            
        }

        private BaccaratRootCalculator BaccaratRootCalculator { get; set; }

        //Hệ số của unit 
        private int MoneyCoeff { get; set; }
        const string REG_PATH = "HKEY_CURRENT_USER\\MidasSoft";
        const string REG_MONEY_COEFF_KEY = "MoneyCoeff";
        const string REG_MONEY_DISPLAYTYPE_KEY = "DisplayType";

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
            if (e.Shift && e.Control && e.KeyCode == Keys.K)
            {
                SetCoeffDisplay();
            }
        }

        private void SetCoeffDisplay()
        {
            var setMoneyCoeff = new SetMoneyCoeff();
            setMoneyCoeff.ShowDialog();
            var _moneyCoeff = Registry.GetValue(REG_PATH, REG_MONEY_COEFF_KEY, string.Empty).ToString();
            var _displayType = Registry.GetValue(REG_PATH, REG_MONEY_DISPLAYTYPE_KEY, string.Empty).ToString();
            
            MoneyCoeff = int.Parse(_moneyCoeff) / 1000;
            lbUnit.Text = int.Parse(_displayType) == 1 ? ",000" : "K";
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
            //Add card
            BaccaratRootCalculator.AddNewCard(inputValue);

            var soundPlay = 4;  //No sound
            if (inputValue == BaccratCard.Banker)
                soundPlay = 1;
            else if (inputValue == BaccratCard.Player)
                soundPlay = 2;
            PlaySound(soundPlay);

            ProcessUI();
        }
        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="input">1: Banker, 2: Player, 3: Trade, 4: No sound</param>
        private void PlaySound(int input)
        {
            if (input == 4)
                return; 
            try
            {
                var soundFile = input == 1 ? @"Sound\Speech On.wav" :
                                input == 2 ? @"Sound\Speech Off.wav"
                                : @"Sound\Alarm10.wav";
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundFile);
                player.Play();
            }
            catch
            {

            }
        }

        private void ProcessUI()
        {
            //Predict
            var predict = BaccaratRootCalculator.Predict();

            txtValue.Text = predict.Value.ToString();
            txtValue.ForeColor = predict.Value == BaccratCard.NoTrade ? Color.Black :
                                            predict.Value == BaccratCard.Banker ? Color.Red : Color.Blue;
            txtVolume.ForeColor = txtValue.ForeColor;
            txtVolume.Text = (predict.Volume * MoneyCoeff).ToString();

            //ToDo: Change
            lbl_ClickedReport.Text = "Đã ghi nhận " + BaccaratRootCalculator.GlobalOrder + ": " + BaccaratRootCalculator.ShowLastCard();
            txt_1.Text = "";

            if (predict.Volume > 0)
                PlaySound(3);
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

        private void btnBackward_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa bước cuối chứ?", "Quan trọng", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            BaccaratRootCalculator.Backward();

            ProcessUI();

        }

        private void btnBackward_MouseEnter(object sender, EventArgs e)
        {
            var button = (sender as Button);
            var text = button.Name == btnBackward.Name ? "Lùi về bước trước" : "Reset";

            toolTip1.SetToolTip((sender as Button), text);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn chứ?", "Quan trọng lắm, đọc kĩ nè!!!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            BaccaratRootCalculator.Reset();

            if (BaccaratRootCalculator.GlobalOrder > 0)
            {
                var lastcard = BaccaratRootCalculator.ShowLastCard();
                lbl_ClickedReport.Text = "Đã ghi nhận " + BaccaratRootCalculator.GlobalOrder + ": " + lastcard.ToString();
            }
            else
            {
                lbl_ClickedReport.Text = "Bắt đầu chơi";
            }
        }

        private void btnSeeLog_Click(object sender, EventArgs e)
        {

        }
    }
}
