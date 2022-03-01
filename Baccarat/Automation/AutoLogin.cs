using Midas.Automation;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Windows.Forms;

namespace Midas
{
    public partial class AutoLogin : Form
    {
        #region Ctor
        public AutoLogin()
        {
            InitializeComponent();

            numInterval.Value = 5;
            Timer.Interval = 1000 * 60 * (int)numInterval.Value; //5 mins
            Timer.Tick += Timer_Tick;

            AllTableResultTimer.Interval = 5_000;
            AllTableResultTimer.Tick += CheckResultTimer_Tick;


            SwitchTableTimer.Interval = 1000 * 60 * 5; //10 phút
            SwitchTableTimer.Tick += SwitchTableTimer_Tick;

            OneTableResultTimer.Interval = 1000;
            OneTableResultTimer.Tick += OneTableResultTimer_Tick;

            try
            {
                Driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            catch (DriverServiceNotFoundException ex)
            {
                btnTest.Visible = false;
                MessageBox.Show("Vui lòng download Chrome Driver tại: http://chromedriver.storage.googleapis.com/index.html. \r\n\r\n" + ex.Message);
            }
            catch (Exception ex)
            { 
                //
            }
            btnTakePhoto.ForeColor = Color.Green;
            lbCurrentStatus.BackColor = Color.Red;
            lbCurrentStatus.ForeColor = Color.White;
            lbCurrentStatus.Text = "STOPPED";
        }
        #endregion

        #region Properties
        bool StatusEnabled { get; set; } = false;

        Timer Timer = new Timer();
        private readonly ChromeDriver Driver = null;
        private IWebDriver AllTableDriver;

        /// <summary>
        /// Lấy kết quả khi đang ở chế độ tất cả các bàn
        /// </summary>
        Timer AllTableResultTimer = new Timer();

        /// <summary>
        /// Lấy kết quả khi đang ở chế độ 1 bàn
        /// </summary>
        Timer OneTableResultTimer = new Timer();

        /// <summary>
        /// Chuyển chế độ giữa tất cả các bàn hoặc 1 bàn cụ thể, chống việc time out
        /// </summary>
        Timer SwitchTableTimer = new Timer();

        /// <summary>
        /// Đánh dấu cho biết đang ở trạng thái tất cả các bàn hay đang ở tại 1 bàn cụ thể nào
        /// Nếu =TRUE: Đang ở màn hình tất cả các bàn
        /// Nếu =FALSE: Đang ở trong 1 bàn cụ thể
        /// </summary>
        bool IsInAllTableView = false;

        const string IMAGE_FORMAT = FOLDER_FORMAT + "\\Image_{0:HHmmss}.png";
        const string FOLDER_FORMAT = "Logs\\{0:yyyy-MM-dd}";
        const string AUTO_LOG_FOLDER = "Logs\\AUTO\\{0:yyyy-MM-dd}.log";

        /// <summary>
        /// Ghi lại kết quả hiện tại trên tất cả các bàn.
        /// Sau đó, so sánh kết quả vừa lấy với kết quả này để nhận diện bàn nào có card mới
        /// </summary>
        private List<AutomationTableResult> SavedAllTableResults = new List<AutomationTableResult>();
        #endregion

        #region Timer ticks events
        private void OneTableResultTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException("Sẽ triển khai sau");
        }

        private void SwitchTableTimer_Tick(object sender, EventArgs e)
        {
            if (AllTableDriver == null)
                return; 

            var table1 = AllTableDriver.FindElements(By.CssSelector(".lobbyTable"))[0];
            table1.Click(); //Nhấn vô bàn số 1
            IsInAllTableView = false;

            System.Threading.Thread.Sleep(5000); //Đợi khoảng 5 giây

            var allTableButton = AllTableDriver.FindElement(By.CssSelector("#IconBaccarat"));
            allTableButton.Click();
            IsInAllTableView = false;
        }

        int ParseInt(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;
            return int.Parse(s);
        }

        /// <summary>
        /// Lấy và xử lý kết quả cho tất cả các bàn
        /// </summary>
        private void GetResult_AllTableView()
        {
            //Lấy kết quả BANKER, PLAYER và TIE ở tất cả các bàn 
            var allTables = AllTableDriver.FindElements(By.CssSelector("table-list table-item"));
            var allBankers = AllTableDriver.FindElements(By.Id("StatisticsB")).Select(c => c.Text).ToList();
            var allPlayers = AllTableDriver.FindElements(By.Id("StatisticsP")).Select(c => c.Text).ToList();
            var allTies = AllTableDriver.FindElements(By.Id("StatisticsT")).Select(c => c.Text).ToList();            

            if (!SavedAllTableResults.Any()) //Lần đầu, chưa có bàn nào 
            {
                for (var i = 0; i < allTables.Count; i++)
                {
                    SavedAllTableResults.Add(new AutomationTableResult
                    {
                        TotalBanker = ParseInt(allBankers[i]),
                        TotalPlayer = ParseInt(allPlayers[i]),
                        TotalTie = ParseInt(allTies[i])
                    });
                }
            }
            else //Kiểm tra xem có bàn nào thay đổi kết quả thì ghi biết bàn đó đã có thêm card 
            {
                for (var i = 0; i < allTables.Count; i++)
                {
                    var savedTableResult = SavedAllTableResults[i];
                    var newResult = new AutomationTableResult
                    {
                        TotalBanker = ParseInt(allBankers[i]),
                        TotalPlayer = ParseInt(allPlayers[i]),
                        TotalTie = ParseInt(allTies[i])
                    };
                    if (savedTableResult.Total != newResult.Total) //Có thêm card mới
                    {
                        var cardResult = "";
                        if (savedTableResult.TotalBanker + 1 == newResult.TotalBanker)
                        {
                            cardResult = "BANKER"; 
                        }
                        else if (savedTableResult.TotalPlayer + 1 == newResult.TotalPlayer)
                        {
                            cardResult = "PLAYER";
                        }
                        else if (savedTableResult.TotalTie + 1 == newResult.TotalTie)
                        {
                            cardResult = "TIE";
                        }                        
                        txtLog.Text = $"{DateTime.Now : yyyy-MM-dd HH:mm:ss}, Bàn số {i + 1}, card { cardResult}" 
                            + Environment.NewLine 
                            + txtLog.Text ;
                    }

                    //Cập nhật lại thông tin cho card 
                    //SavedAllTableResults[i] = newResult;
                    SavedAllTableResults.RemoveAt(i);
                    SavedAllTableResults.Insert(i, newResult);
                }
            }
            

            //Đánh với nhiều thuật toán khác nhau 


            //Ghi vào dữ liệu
        }
        private void CheckResultTimer_Tick(object sender, EventArgs e)
        {
            if (IsInAllTableView) //Chế độ tất cả các bàn
            {
                GetResult_AllTableView();
            }
            else //Chế độ ở 1 bàn cụ thể
            { 
            
            
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TakeScreenshot(false);
        }

        #endregion

        #region Form Load + Close
        private void AutoLogin_Load(object sender, EventArgs e)
        {
            if (Driver == null)
                return;

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20.0);
            Driver.Manage().Window.Maximize();
        }

        private void AutoLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Driver?.Quit();
        }
        #endregion             

        #region Take photo (Auto + Manual)
        private void TakeScreenshot(bool showMessage)
        {
            var dateTimeNow = DateTime.Now;
            if (!Directory.Exists(string.Format(FOLDER_FORMAT, dateTimeNow)))
            {
                Directory.CreateDirectory(string.Format(FOLDER_FORMAT, dateTimeNow));
            }
            try
            {
                Rectangle bounds = Screen.PrimaryScreen.Bounds;
                var width = (int)numWidth.Value;
                var height = (int)numHeight.Value;
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        g.CopyFromScreen(new Point(0, 0), Point.Empty, new Size(width, height));
                    }
                    bitmap.Save(string.Format(IMAGE_FORMAT, dateTimeNow), ImageFormat.Jpeg);
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

        private void btnCamera_Click(object sender, EventArgs e)
        {
            TakeScreenshot(false);
        }

        private void btnTakePhoto_Click(object sender, EventArgs e)
        {
            StatusEnabled = !StatusEnabled;
            if (StatusEnabled)
            {
                Timer.Interval = (int)numInterval.Value * 1000 * 60;
                Timer.Start();
                btnTakePhoto.Text = "STOP Taking Photo";
                btnTakePhoto.ForeColor = Color.Red;

                lbCurrentStatus.BackColor = Color.Green;
                lbCurrentStatus.ForeColor = Color.White;
                lbCurrentStatus.Text = "STARTED";
            }
            else
            {
                Timer.Stop();
                btnTakePhoto.Text = "START Taking Photo";
                btnTakePhoto.ForeColor = Color.Green;

                lbCurrentStatus.BackColor = Color.Red;
                lbCurrentStatus.ForeColor = Color.White;
                lbCurrentStatus.Text = "STOPPED";
            }
        }
        #endregion

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (Driver == null)
                return;

            try
            {
                Driver.Navigate().GoToUrl("https://www.jbbodds.com/vi-vn");
                System.Threading.Thread.Sleep(500); //Đợi nửa giây

                Driver.FindElement(By.CssSelector(".input-username input[name=username]")).SendKeys(txtUserName.Text);
                Driver.FindElement(By.CssSelector(".input-password input[name=password]")).SendKeys(txtPassword.Text);
                Driver.FindElement(By.CssSelector("button[type=submit]")).Click();
                System.Threading.Thread.Sleep(500); //Đợi nửa giây

                var liveCasino = Driver.FindElements(By.CssSelector("#menu-products > li > a"))[2];
                liveCasino.Click();
                System.Threading.Thread.Sleep(5000); //Đợi 5 giây 

                var k9 = Driver.FindElements(By.CssSelector(".casino-list li"))[0];
                Actions actions = new Actions(Driver);
                actions.MoveToElement(k9).Perform(); //Đưa chuột lên phần K9
                System.Threading.Thread.Sleep(2000); //Đợi 2 giây

                var playNowGrandSuite = Driver.FindElements(By.CssSelector(".game-popup a.btn-orange"))[0];
                playNowGrandSuite.Click();
                System.Threading.Thread.Sleep(500);//Đợi 2 giây cho xuất hiện nút OK


                var okButton = Driver.FindElement(By.CssSelector(".modal-dialog .text-center button.bet-btn"));
                okButton.Click(); //Nhấn nút OK
                System.Threading.Thread.Sleep(5000);//Đợi 5 giây để màn hình mới load

                //Switch qua màn hình mới 
                AllTableDriver = Driver.SwitchTo().Window(Driver.WindowHandles[1]);

                IsInAllTableView = true;
                AllTableResultTimer.Start();
            }
            catch (Exception ex)
            {
                txtLog.AppendText(ex.Message + Environment.NewLine);
            }
        }

    }
}
