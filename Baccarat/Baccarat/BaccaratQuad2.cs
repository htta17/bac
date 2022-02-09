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
using System.Drawing.Imaging;
using DatabaseContext;

namespace Baccarat
{
    public partial class BaccaratQuad2 : Form
    {
        public BaccaratQuad2()
        {
            this.KeyPreview = true;
            InitializeComponent();
            if (string.IsNullOrEmpty(StartApp.GlobalConnectionString))
            {
                StartApp.LoadRegistryConnectionString();
            }

            QuadrupleMaster = new BaccaratQuadrupleMaster(ThreadMode.Five_Eight | ThreadMode.One_Four, StartApp.GlobalConnectionString);

            

            FILENAME_DATETIME = DateTime.Now;

            if (DateTime.Now > DateTime.Parse("2022-06-01"))
            {
                MessageBox.Show("Unexpected error. HRESULT = 0x800AX472. Please contact https://support.microsoft.com/contactus.");
                this.Close();
            }

            

            //BaccaratDBContext = new BaccaratDBContext();
        }

        BaccaratQuadrupleMaster QuadrupleMaster { get; set; }       

        //BaccaratDBContext BaccaratDBContext { get; set; }

        private DateTime? FILENAME_DATETIME = null;
        private const string LogTitle = "ID,Time,Card,Loss/Profit,Total\r\n";
        //const string LOG_FILE_FORMAT = "Logs\\Midas_{0:yyyyMMdd_HHmmss}.csv";
        const string IMAGE_FORMAT = "Logs\\Midas_{0:yyyyMMdd_HHmmss}_{1}.jpeg";
        private int Screenshot_Counter = 0;

        //string FULL_PATH_FILE
        //{
        //    get
        //    {
        //        return string.Format(LOG_FILE_FORMAT, FILENAME_DATETIME.Value);
        //    }
        //}

        string FULL_PATH_IMAGE
        {
            get
            {
                return string.Format(IMAGE_FORMAT, FILENAME_DATETIME.Value, Screenshot_Counter);
            }
        }

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

        private void ProcessInput(BaccratCard inputValue, string importFileName)
        {
            QuadrupleMaster.Process(inputValue);

            var predict = QuadrupleMaster.MasterPredict;

            txtValue.Text = predict.Value.ToString();
            txtValue.ForeColor = predict.Value == BaccratCard.NoTrade ? Color.Black :
                                            predict.Value == BaccratCard.Banker ? Color.Red : Color.Blue;
            txtVolume.ForeColor = txtValue.ForeColor;
            txtVolume.Text = predict.Volume.ToString();

            lbl_ClickedReport.Text = "Đã ghi nhận " + QuadrupleMaster.MasterID + ": " + txt_1.Text;
            txt_1.Text = "";

            var soundPlay = 4;  //No sound
            if (inputValue == BaccratCard.Banker)
                soundPlay = 1;
            else if (inputValue == BaccratCard.Player)
                soundPlay = 2;
            PlaySound(soundPlay);

            //WriteLogNDatabase(inputValue, importFileName);
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
                                : @"Sound\Windows Logon.wav";
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundFile);
                player.Play();
            }
            catch
            {

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

            ProcessInput(inputValue, null);
        }

        private void FinishSession()
        {
            txt_1.Text = "";
            txtValue.Text = "";
            txtVolume.Text = "";
            FILENAME_DATETIME = DateTime.Now;

            Screenshot_Counter = 0;

            QuadrupleMaster.ResetAll();
            lbl_ClickedReport.Text = "Bắt đầu phiên....";
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa toàn bộ số liệu trên các ô chứ?", "Quan trọng lắm, đọc kĩ nè!!!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            FinishSession();
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
            else if (e.Control && e.KeyCode == Keys.A && e.Alt)
            {
                btnImport.Visible = !btnImport.Visible;
            }
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
        private void txt_8_TextChanged(object sender, EventArgs e)
        {
            var textBox = (sender as TextBox);
            textBox.ForeColor = textBox.Text == Constants.BANKER_VALUE ? Color.Red : textBox.Text == Constants.PLAYER_VALUE ? Color.Blue : Color.Black;

            textBox.BackColor = textBox.Text == "" ? Color.White : Color.FromArgb(255, 255, 192);
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
            if (QuadrupleMaster.MasterID > 0)
            {
                lbl_ClickedReport.Text = "Đã ghi nhận " + QuadrupleMaster.MasterID + ": " + QuadrupleMaster.ShowLastCard().ToString();
            }
            else
            {
                lbl_ClickedReport.Text = "Bắt đầu phiên....";
            }

        }

        private void BaccaratQuad2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //BaccaratDBContext.Dispose();
            TakeScreenshot(false);
        }

        #region Import file to database
        private void ImportFiles(string fullFilePathName)
        {
            var fileName = fullFilePathName.Split('\\').Last();

            var allLines = File.ReadAllLines(fullFilePathName).ToList();
            foreach (var line in allLines)
            {
                var texts = line.Split(',');
                var card = texts[2].ToUpper();
                if (card == Constants.BANKER_VALUE || card == Constants.PLAYER_VALUE)
                {
                    var inputValue = card == Constants.BANKER_VALUE
                           ? BaccratCard.Banker : BaccratCard.Player;

                    ProcessInput(inputValue, fileName);
                }
            }
            FinishSession();
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (QuadrupleMaster.MasterID > 0)
            {
                MessageBox.Show("Vui lòng kết thúc phiên rồi mới import dữ liệu");
                return;
            }

            var folderChoose = new FolderBrowserDialog();
            folderChoose.SelectedPath = Directory.GetCurrentDirectory();
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
            
            foreach (var file in files)
            {
                ImportFiles(file);
            }
            MessageBox.Show("Import thành công.");
        }

        #endregion

        #region Take screen shot
        private void TakeScreenshot(bool showMessage)
        {
            try
            {
                Rectangle bounds = Screen.FromControl(this).Bounds;
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(new Point(0, 0), Point.Empty, bounds.Size);
                    }
                    bitmap.Save(FULL_PATH_IMAGE, ImageFormat.Jpeg);
                }
            }
            catch (Exception e)
            {
                if (showMessage)
                {
                    MessageBox.Show("Error" + e.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }
            
        }

        #endregion

        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (FILENAME_DATETIME.HasValue)
            {
                Screenshot_Counter++;
                TakeScreenshot(true);
            }
            else
            {
                MessageBox.Show("Xin mời bắt đầu phiên. Chương trình chỉ chụp ảnh màn hình khi đã bắt đầu phiên", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCamera_MouseEnter(object sender, EventArgs e)
        {
            var button = (sender as Button);
            var text = button.Name == btnBackward.Name ? "Lùi về bước trước" : "Chụp ảnh màn hình"; 

            toolTip.SetToolTip((sender as Button), text);
        }

        private void btnSeeLog_Click(object sender, EventArgs e)
        {

        }
    }
}
