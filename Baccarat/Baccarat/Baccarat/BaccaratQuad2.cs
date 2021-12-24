using CalculationLogic;
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
using Newtonsoft.Json;
using Midas;
using System.Text.RegularExpressions;
using Midas.Utils;

namespace Baccarat
{
    public partial class BaccaratQuad2 : Form
    {
        public BaccaratQuad2()
        {
            this.KeyPreview = true;
            InitializeComponent();

            TradeFiveToEightCalculator = new BaccaratQuadruple();
            TradeOneToFourCalculator = new BaccaratQuadruple();
            TradeSevenToTwoCalculator = new BaccaratQuadruple();
            TradeThreeToSixCalculator = new BaccaratQuadruple();

            TradeFiveToEightCards = new List<BaccratCard>();
            TradeOneToFourCards = new List<BaccratCard>();
            TradeSevenToTwoCards = new List<BaccratCard>();
            TradeThreeToSixCards = new List<BaccratCard>();

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            FILENAME = string.Format(FileFormatCSV, DateTime.Now);           

            SavedNumbers = new List<AllSavedNumbers>();

            if (DateTime.Now > DateTime.Parse("2022-06-01"))
            {
                MessageBox.Show("Mời kiểm tra chế độ của phiên");
                this.Close();
            }
        }
        public BaccaratQuadruple TradeFiveToEightCalculator { get; set; }
        public List<BaccratCard> TradeFiveToEightCards { get; set; }
        public BaccaratQuadruple TradeOneToFourCalculator { get; set; }
        public List<BaccratCard> TradeOneToFourCards { get; set; }

        public BaccaratQuadruple TradeThreeToSixCalculator { get; set; }
        public List<BaccratCard> TradeThreeToSixCards { get; set; }

        public BaccaratQuadruple TradeSevenToTwoCalculator { get; set; }
        public List<BaccratCard> TradeSevenToTwoCards { get; set; }

        /// <summary>
        /// Used to save most recent steps to reverse 
        /// </summary>
        private List<AllSavedNumbers> SavedNumbers { get; set; }

        const string BANKER_VALUE = "BANKER";
        const string PLAYER_VALUE = "PLAYER";
        const string LOG_FILE_FORMAT = "Logs\\Midas_{0}";
        
        private string ReadFile(string filePath)
        {
            var text = "";
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    text = sr.ReadToEnd();
                }
            }
            catch 
            {                
            }
            return text;
        }

        /// <summary>
        /// Coeff of currency, default is 1, get from database
        /// </summary>
        private int COEFF = 1;
        
        /// <summary>
        /// ID 
        /// </summary>
        int IDCounting = 0;        

        /// <summary>
        /// Current file name
        /// </summary>
        private string FILENAME = null;
        private const string LogTitle = "ID,Time,Card,Loss/Profit,Total\r\n";
        private const string FileFormatCSV = "{0:yyyyMMdd_HHmmss}.csv";
        QuadrupleResult CurrentPredict14 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict36 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict58 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict72 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };

        /// <summary>
        /// Calculate accumulate loss/profit 
        /// </summary>
        int Total = 0;
        
        private void btn51_Click(object sender, EventArgs e)
        {
            var text = (sender as Button).Text;            

            var buttonName = (sender as Button).Name;
            var textBoxName = buttonName.Substring(3, buttonName.Length - 4);

            var textBox = this.Controls.Find("txt_" + textBoxName, false).First() as TextBox;

            textBox.Text = text;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (txt_1.Text == "")
            {
                MessageBox.Show("Vui lòng nhập giá trị");
                return;
            }
            //Reset if needed            
            if (IDCounting % 8 == 0)
            {
                TradeFiveToEightCards.Clear();
            }
            else if (IDCounting % 8 == 2)
            {
                TradeSevenToTwoCards.Clear();
            }
            else if (IDCounting % 8 == 4)
            {
                TradeOneToFourCards.Clear();
            }
            else if (IDCounting % 8 == 6)
            {
                TradeThreeToSixCards.Clear();
            }


            IDCounting++;
            var inputValue = txt_1.Text == BANKER_VALUE
                                    ? BaccratCard.Banker : BaccratCard.Player;

            TradeFiveToEightCards.Add(inputValue);

            if (IDCounting > 2)
            {
                TradeSevenToTwoCards.Add(inputValue);
            }

            if (IDCounting > 4)
            {
                TradeOneToFourCards.Add(inputValue);
            }

            if (IDCounting > 6)
            {
                TradeThreeToSixCards.Add(inputValue);
            }


            TradeFiveToEightCalculator.SetCards(TradeFiveToEightCards);
            TradeOneToFourCalculator.SetCards(TradeOneToFourCards);
            TradeSevenToTwoCalculator.SetCards(TradeSevenToTwoCards);
            TradeThreeToSixCalculator.SetCards(TradeThreeToSixCards);

            var profit = 0;
            profit += (CurrentPredict14.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict14.Value ? -CurrentPredict14.Volume : CurrentPredict14.Volume);
            profit += (CurrentPredict36.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict36.Value ? -CurrentPredict36.Volume : CurrentPredict36.Volume);
            profit += (CurrentPredict58.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict58.Value ? -CurrentPredict58.Volume : CurrentPredict58.Volume);
            profit += (CurrentPredict72.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict72.Value ? -CurrentPredict72.Volume : CurrentPredict72.Volume);
            Total += profit;

            TradeFiveToEightCalculator.UpdateCoff();
            TradeOneToFourCalculator.UpdateCoff();
            TradeThreeToSixCalculator.UpdateCoff();
            TradeSevenToTwoCalculator.UpdateCoff();

            CurrentPredict58 = TradeFiveToEightCalculator.Predict();
            CurrentPredict14 = TradeOneToFourCalculator.Predict();
            CurrentPredict36 = TradeThreeToSixCalculator.Predict();
            CurrentPredict72 = TradeSevenToTwoCalculator.Predict();

            var predict14 = CurrentPredict14.Value == BaccratCard.Banker ? CurrentPredict14.Volume : -CurrentPredict14.Volume;
            var predict36 = CurrentPredict36.Value == BaccratCard.Banker ? CurrentPredict36.Volume : -CurrentPredict36.Volume;
            var predict58 = CurrentPredict58.Value == BaccratCard.Banker ? CurrentPredict58.Volume : -CurrentPredict58.Volume;
            var predict72 = CurrentPredict72.Value == BaccratCard.Banker ? CurrentPredict72.Volume : -CurrentPredict72.Volume;

            var totalPredict = predict14 + predict36 + predict58 + predict72; 

            txtValue.Text = totalPredict < 0 ? PLAYER_VALUE : BANKER_VALUE;
            txtValue.ForeColor = totalPredict == 0 ? Color.Black :
                                            totalPredict > 0 ? Color.Red : Color.Blue;
            txtVolume.ForeColor = txtValue.ForeColor;
            txtVolume.Text = totalPredict.ToString();            
            
            lbl_Same_5_8.Text = "Same: " + CurrentPredict58.Same_Coff.ToString();
            lbl_Diff_5_8.Text = "Diff: " + CurrentPredict58.Diff_Coff.ToString();
            lbl_Same_1_4.Text = "Same: " + CurrentPredict14.Same_Coff.ToString();
            lbl_Diff_1_4.Text = "Diff: " + CurrentPredict14.Diff_Coff.ToString();
            
            var maxLengthOf2Arr = TradeOneToFourCards.Count > TradeFiveToEightCards.Count
                                        ? TradeOneToFourCards.Count
                                        : TradeFiveToEightCards.Count;
            maxLengthOf2Arr = maxLengthOf2Arr > 4 ? 4 : maxLengthOf2Arr;

            lbl_ClickedReport.Text = "Đã ghi nhận " + IDCounting + ": " + txt_1.Text;
            txt_1.Text = "";

            if (IDCounting == 1)
            {
                File.AppendAllText(string.Format(LOG_FILE_FORMAT, FILENAME), LogTitle);
            }

            var logger = string.Format("{0},{1:yyyy-MM-dd HH:mm:ss},{2},{3},{4}\r\n", 
                                    IDCounting, 
                                    DateTime.Now, 
                                    TradeFiveToEightCalculator.BaccratCards.Last(),
                                    profit == 0 ? "" : profit.ToString(), 
                                    Total);
            //Save data
            var saved = false;
            while (!saved)
            {
                try
                {
                    File.AppendAllText(string.Format(LOG_FILE_FORMAT, FILENAME), logger);
                    saved = true;
                }
                catch
                {                    
                    MessageBox.Show("Vui lòng đóng file CSV khi đang ghi, sau đó nhấn nút OK");
                }
            }             
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa toàn bộ số liệu trên các ô chứ?", "Quan trọng lắm, đọc kĩ nè!!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                txt_1.Text = "";
                TradeFiveToEightCalculator.Reset();
                TradeOneToFourCalculator.Reset();
                TradeSevenToTwoCalculator.Reset();

                lbl_Same_5_8.Text = "Same: 0";
                lbl_Diff_5_8.Text = "Diff: 0";

                lbl_Same_1_4.Text = "Same: 0";
                lbl_Diff_1_4.Text = "Diff: 0";

                txtValue.Text = "";
                txtVolume.Text = "";

                FILENAME = string.Format(FileFormatCSV, DateTime.Now);                
                IDCounting = 0;
                lbl_ClickedReport.Text = "Bắt đầu phiên....";
            }
        }

        private void txt_8_DoubleClick(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "";
        }
        private void BaccaratQuad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                btnCalculate_Click(null, null);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                tabControls.Visible = !tabControls.Visible;
            }
            else if (e.Control && e.KeyCode == Keys.K && e.Shift)
            {                
                btnProcessData.Visible = !btnProcessData.Visible;
            }
        }
        private void btn10_MouseHover(object sender, EventArgs e)
        {
            var button = (sender as Button);
            if (button.Text == PLAYER_VALUE)
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
            if (button.Text == PLAYER_VALUE)
            {
                button.BackColor = Color.Blue;
            }
            else
            {
                button.BackColor = Color.Red;
            }
            button.ForeColor = Color.White;
        }
        private void txt_8_TextChanged(object sender, EventArgs e)
        {
            var textBox = (sender as TextBox);
            textBox.ForeColor = textBox.Text == BANKER_VALUE ? Color.Red : textBox.Text == PLAYER_VALUE ? Color.Blue : Color.Black;

            textBox.BackColor = textBox.Text == "" ? Color.White : Color.FromArgb(255,255,192);
        }       

        private void btnProcessData_Click(object sender, EventArgs e)
        {
            var folderChoose = new FolderBrowserDialog();
            var folderPath = "";
            if (folderChoose.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderChoose.SelectedPath;
            }
            if (folderPath == "") 
                return;

            Regex reg = new Regex("((Midas_)?(\\d{8})_(\\d{6})\\.csv)");
            var files = Directory.GetFiles(folderPath, "*.csv")
                                    .Where(c => reg.IsMatch(c))
                                    .ToArray();
            var fileOrder = 0;
            foreach (var file in files)
            {
                fileOrder++;
                ProcessFile(file, fileOrder);
            }
            MessageBox.Show("Đã xử lý xong. Xin mời xem các files trong cùng thư mục");
        }

        private void ProcessFile(string filePath, int num)
        {
            Generate4ThreadLogs.ProcessFile(filePath, num);
            #region Moved to Generate4ThreadLogs
            /*
            var processedFilePath = filePath.Replace(".csv", "") + "_process_" + DateTime.Now.ToString("yyMMddHHmmss_") + num.ToString() + ".csv"; 
            var allLines = File.ReadAllLines(filePath).ToList();

            BaccaratQuadruple _tradeFiveToEightCalculator = new BaccaratQuadruple { BaccratCards = new List<BaccratCard>(), SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeFiveToEightCards = new List<BaccratCard>();
            
            BaccaratQuadruple _tradeOneToFourCalculator = new BaccaratQuadruple { BaccratCards = new List<BaccratCard>(), SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeOneToFourCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeThreeToSixCalculator = new BaccaratQuadruple { BaccratCards = new List<BaccratCard>(), SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeThreeToSixCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeSevenToTwoCalculator = new BaccaratQuadruple { BaccratCards = new List<BaccratCard>(), SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeSevenToTwoCards = new List<BaccratCard>();

            var loss_Profit_Total58 = 0;
            var loss_Profit_Total14 = 0;
            var loss_Profit_Total36 = 0;
            var loss_Profit_Total72 = 0;

            var same_58 = 0;
            var diff_58 = 0;
            var same_14 = 0;
            var diff_14 = 0;

            var same_36 = 0;
            var diff_36 = 0;
            var same_72 = 0;
            var diff_72 = 0;

            foreach (var line in allLines)
            {
                var texts = line.Split(',');
                var card = texts[2];
                if (card == "Banker" || card == "Player")
                {
                    var id = int.Parse(texts[0]);
                    if (id % 8 == 1)
                    {
                        _tradeFiveToEightCards.Clear();
                    }
                    else if (id % 8 == 3)
                    {
                        _tradeSevenToTwoCards.Clear();
                    }
                    else if (id % 8 == 5)
                    {
                        _tradeOneToFourCards.Clear();
                    }
                    else if (id % 8 == 7)
                    {
                        _tradeThreeToSixCards.Clear();
                    }

                    var _inputValue = card == "Banker"
                                    ? BaccratCard.Banker : BaccratCard.Player;

                    _tradeFiveToEightCards.Add(_inputValue);
                    if (id > 2)
                    {
                        _tradeSevenToTwoCards.Add(_inputValue);
                    }

                    if (id > 4)
                    {
                        _tradeOneToFourCards.Add(_inputValue);
                    }

                    if (id > 6)
                    {
                        _tradeThreeToSixCards.Add(_inputValue);
                    }


                    _tradeFiveToEightCalculator.BaccratCards = new List<BaccratCard>();
                    for (var i = 0; i < _tradeFiveToEightCards.Count; i++)
                    {
                        _tradeFiveToEightCalculator.BaccratCards.Add(_tradeFiveToEightCards[i]);
                    }

                    _tradeOneToFourCalculator.BaccratCards = new List<BaccratCard>();
                    for (var i = 0; i < _tradeOneToFourCards.Count; i++)
                    {
                        _tradeOneToFourCalculator.BaccratCards.Add(_tradeOneToFourCards[i]);
                    }

                    _tradeSevenToTwoCalculator.BaccratCards = new List<BaccratCard>();
                    for (var i = 0; i < _tradeSevenToTwoCards.Count; i++)
                    {
                        _tradeSevenToTwoCalculator.BaccratCards.Add(_tradeSevenToTwoCards[i]);
                    }

                    _tradeThreeToSixCalculator.BaccratCards = new List<BaccratCard>();
                    for (var i = 0; i < _tradeThreeToSixCards.Count; i++)
                    {
                        _tradeThreeToSixCalculator.BaccratCards.Add(_tradeThreeToSixCards[i]);
                    }                    
                    
                    _tradeFiveToEightCalculator.UpdateCoff();
                    _tradeOneToFourCalculator.UpdateCoff();
                    _tradeSevenToTwoCalculator.UpdateCoff();
                    _tradeThreeToSixCalculator.UpdateCoff();

                    same_58 = ((id % 8 >= 5) || (id % 8 == 0)) ?  _tradeFiveToEightCalculator.ShowCoff().Item1 : 0;
                    diff_58 = ((id % 8 >= 5) || (id % 8 == 0)) ? _tradeFiveToEightCalculator.ShowCoff().Item2 : 0;
                    loss_Profit_Total58 += (same_58 + diff_58) / 2;

                    same_14 = (id % 8 >= 1 && id % 8 <= 4 && id > 4) ? _tradeOneToFourCalculator.ShowCoff().Item1 : 0;
                    diff_14 = (id % 8 >= 1 && id % 8 <= 4 && id > 4) ? _tradeOneToFourCalculator.ShowCoff().Item2 : 0;
                    loss_Profit_Total14 += (same_14 + diff_14) / 2;

                    same_36 = (id % 8 >= 3 && id % 8 <= 6 && id > 6) ? _tradeThreeToSixCalculator.ShowCoff().Item1 : 0;
                    diff_36 = (id % 8 >= 3 && id % 8 <= 6 && id > 6) ? _tradeThreeToSixCalculator.ShowCoff().Item2 : 0;
                    loss_Profit_Total36 += (same_36 + diff_36) / 2;

                    same_72 = ((id % 8 >= 7 || id % 8 <= 2) && id > 2) ? _tradeSevenToTwoCalculator.ShowCoff().Item1 : 0;
                    diff_72 = ((id % 8 >= 7 || id % 8 <= 2) && id > 2) ? _tradeSevenToTwoCalculator.ShowCoff().Item2 : 0;
                    loss_Profit_Total72 += (same_72 + diff_72) / 2;

                    //var loss_Profit_Total = texts[6];
                    var loss_Profit_Total = loss_Profit_Total58 +
                                            loss_Profit_Total14 +
                                            loss_Profit_Total36 +
                                            loss_Profit_Total72;

                    _tradeFiveToEightCalculator.Predict();
                    _tradeOneToFourCalculator.Predict();
                    _tradeThreeToSixCalculator.Predict();
                    _tradeSevenToTwoCalculator.Predict();

                    var log = string.Format("{0},{1},{2},{3},{4},,{5},{6},{7},{8},,{9},{10},{11},{12},,{13},{14},{15},{16},{17}\r\n",
                                        _inputValue == BaccratCard.Banker ? 1 : -1, //Result --> {0}
                                        //Group 1
                                        same_58, //Same 5-8 --> {1}
                                        diff_58, //Diff 5-8 --> {2}
                                        (same_58 + diff_58) /2, //--> {3}
                                        loss_Profit_Total58, //--> {4}

                                        //Group 2
                                        same_14, //--> {5}
                                        diff_14,//--> {6}
                                        (same_14 + diff_14) / 2, //--> {7}
                                        loss_Profit_Total14,//--> {8}

                                        //Group 3
                                        same_36, //--> {9}
                                        diff_36,//--> {10}
                                        (same_36 + diff_36) / 2, //--> {11}
                                        loss_Profit_Total36,//--> {12}

                                        //Group 4
                                        same_72, //--> {13}
                                        diff_72,//--> {14}
                                        (same_72 + diff_72) / 2, //--> {15}
                                        loss_Profit_Total72,//--> {16}

                                        loss_Profit_Total); //-> 17

                    File.AppendAllText(processedFilePath, log);
                }
            }
            */
            #endregion
        }


    }
}
