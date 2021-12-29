﻿using CalculationLogic;
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
            QuadrupleMaster = new BaccaratQuadrupleMaster(ThreadMode.Five_Eight | ThreadMode.One_Four);

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            FILENAME = string.Format(FileFormatCSV, DateTime.Now);

            if (DateTime.Now > DateTime.Parse("2022-06-01"))
            {
                MessageBox.Show("Unexpected error. HRESULT = 0x800AX472. Please contact https://support.microsoft.com/contactus.");
                this.Close();
            }

            if (string.IsNullOrEmpty(StartApp.GlobalConnectionString))
            {
                StartApp.LoadRegistryConnectionString();
            }

            BaccaratDBContext = new BaccaratDBContext();

            //SlackWebHookSender.SendMessage("Xin chào <@U02RUDHU2LD> đập chai. Tổng tiền thu về hôm nay là `98` tỉ  ", "important-report");
        }

        BaccaratQuadrupleMaster QuadrupleMaster { get; set; }       

        const string BANKER_VALUE = "BANKER";
        const string PLAYER_VALUE = "PLAYER";
        const string LOG_FILE_FORMAT = "Logs\\Midas_{0}";        

        BaccaratDBContext BaccaratDBContext { get; set; }
        
        private string FILENAME = null;
        private const string LogTitle = "ID,Time,Card,Loss/Profit,Total\r\n";
        private const string FileFormatCSV = "{0:yyyyMMdd_HHmmss}.csv";
        
        //Database
        private Session CurrentSession { get; set; }
        
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

            var inputValue = txt_1.Text == BANKER_VALUE
                                ? BaccratCard.Banker : BaccratCard.Player;

            QuadrupleMaster.Process(inputValue);

            var predict = QuadrupleMaster.MasterPredict;

            txtValue.Text = predict.Value.ToString();
            txtValue.ForeColor = predict.Value == BaccratCard.NoTrade ? Color.Black :
                                            predict.Value == BaccratCard.Banker ? Color.Red : Color.Blue;
            txtVolume.ForeColor = txtValue.ForeColor;
            txtVolume.Text = predict.Volume.ToString(); 

            lbl_ClickedReport.Text = "Đã ghi nhận " + QuadrupleMaster.MasterID + ": " + txt_1.Text;
            txt_1.Text = "";

            if (QuadrupleMaster.MasterID == 1)
            {
                File.AppendAllText(string.Format(LOG_FILE_FORMAT, FILENAME), LogTitle);

                CurrentSession = new Session
                {
                    StartDateTime = DateTime.Now
                };

                BaccaratDBContext.AddSession(CurrentSession);
            }

            WriteLog(inputValue);
            
        }

        private void FinishCurrentSession()
        {
            
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa toàn bộ số liệu trên các ô chứ?", "Quan trọng lắm, đọc kĩ nè!!!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            FinishCurrentSession();


            txt_1.Text = ""; 
            txtValue.Text = "";
            txtVolume.Text = "";
            FILENAME = string.Format(FileFormatCSV, DateTime.Now);

            QuadrupleMaster.ResetAll();                
            lbl_ClickedReport.Text = "Bắt đầu phiên....";

            //SlackWebHookSender.SendMessage("Trader bắt đầu phiên giao dịch mới.", "brothers-project");
        }

        private void WriteLog(BaccratCard baccratCard)
        {
            //Save to database
            var datetimeNow = DateTime.Now;
            BaccaratDBContext.AddResult(new Result
            {
                Card = (short)(baccratCard == BaccratCard.Banker ? 1 : -1),
                InputDateTime = datetimeNow,
                SessionID = CurrentSession.ID
            });
            var logger = string.Format("{0},{1:yyyy-MM-dd HH:mm:ss},{2},{3},{4}\r\n",
                                        QuadrupleMaster.MasterID,
                                        datetimeNow,
                                        baccratCard,
                                        QuadrupleMaster.LastStepProfit == 0 ? "" : QuadrupleMaster.LastStepProfit.ToString(),
                                        QuadrupleMaster.TotalProfit);

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
            GenerateAllThreadLogs.ProcessFile(filePath, num);            
        }

        private void btnBackward_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa bước cuối chứ?", "Quan trọng", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            QuadrupleMaster.Reverse();
            //Update UI
            lbl_ClickedReport.Text = "Đã ghi nhận " + QuadrupleMaster.MasterID + ": " + txt_1.Text;
        }

        private void BaccaratQuad2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentSession != null)
            {
                CurrentSession.NoOfSteps = QuadrupleMaster.MasterID;
                CurrentSession.Result4Threads = QuadrupleMaster.TotalProfit;
                BaccaratDBContext.UpdateSession(CurrentSession);
            }
            BaccaratDBContext.Dispose();

            var total = QuadrupleMaster.TotalProfit;

            //SlackWebHookSender.SendMessage($"Kết thúc phiên, kết quả: { total }.", "brothers-project");
        }
    }
}
