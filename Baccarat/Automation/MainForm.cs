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
using CoreLogic;

using System.Windows.Forms;
using CoreLogic.StandardlizedAlgorithms;
using Midas.Utils;
using Newtonsoft.Json;
using OpenQA.Selenium.Support.Events;

namespace Midas
{
    public partial class MainForm : Form
    {
        #region Ctor
        public MainForm()
        {
            InitializeComponent();

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

            Timers_Setup();

            //Khởi tạo 10 bàn cho thuật toán ROOT
            if (string.IsNullOrEmpty(StartApp.GlobalConnectionString))
            {
                StartApp.LoadRegistryConnectionString();
            }

            LogicAllTables = new Dictionary<int, AutoBacRootAlgorithm>();
        }

       
        #endregion

        #region Properties
        

        Timer PhotoTakenTimer = new Timer();
        private readonly ChromeDriver Driver = null;
        private IWebDriver AllTableDriver;

        /// <summary>
        /// Lấy kết quả khi đang ở chế độ tất cả các bàn
        /// hoặc 1 bàn 
        /// </summary>
        Timer CheckResultTimer = new Timer();

        /// <summary>
        /// Chuyển chế độ giữa tất cả các bàn hoặc 1 bàn cụ thể, chống việc time out
        /// </summary>
        Timer SwitchTableTimer = new Timer();

        Dictionary<int, AutoBacRootAlgorithm> LogicAllTables { get; set; }

        /// <summary>
        /// Đánh dấu cho biết đang ở trạng thái tất cả các bàn hay đang ở tại 1 bàn cụ thể nào
        /// Nếu =TRUE: Đang ở màn hình tất cả các bàn
        /// Nếu =FALSE: Đang ở trong 1 bàn cụ thể
        /// </summary>
        bool IsInAllTableView = false;
        

        /// <summary>
        /// Ghi lại kết quả hiện tại trên tất cả các bàn.
        /// Sau đó, so sánh kết quả vừa lấy với kết quả này để nhận diện bàn nào có card mới
        /// </summary>
        private List<AutomationTableResult> SavedAllTableResults = new List<AutomationTableResult>();
        #endregion

        #region Các hàm cho việc khởi tạo
        private void Timers_Setup()
        {
            CheckResultTimer.Interval = 5_000;  //5 giây 1 lần
            CheckResultTimer.Tick += CheckResultTimer_Tick;

            SwitchTableTimer.Interval = 1000 * 60 * 5; //5 phút
            SwitchTableTimer.Tick += SwitchTableTimer_Tick;
        }

       
        #endregion

        #region Timer ticks events
        /// <summary>
        /// Chuyển qua chuyển lại giữa chế độ đánh auto và 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchTableTimer_Tick(object sender, EventArgs e)
        {
            if (AllTableDriver == null)
                return; 

            var table1 = AllTableDriver.FindElements(By.CssSelector(".lobbyTable"))[0];
            table1.Click(); //Nhấn vô bàn số 1
            IsInAllTableView = false;

            System.Threading.Thread.Sleep(2000); //Đợi khoảng 2 giây

            var allTableButton = AllTableDriver.FindElement(By.CssSelector("#IconBaccarat"));
            allTableButton.Click();
            IsInAllTableView = true;
        }

        int ParseInt(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;
            return int.Parse(s);
        }

        private AutoBacRootAlgorithm GetTable(int _tableNo)
        {
            return LogicAllTables.ContainsKey(_tableNo) 
                ? LogicAllTables[_tableNo]
                : default;
        }
        private void Log(string text)
        {
            txtLog.Text = $"{DateTime.Now : yyyy-MM-dd HH:mm:ss}: {text}" + Environment.NewLine + txtLog.Text;
        }       

        /// <summary>
        /// Lấy và xử lý kết quả cho tất cả các bàn
        /// </summary>
        private void GetResult_AllTableView()
        {
            //Lấy kết quả BANKER, PLAYER và TIE ở tất cả các bàn 
            var uiAllTables = AllTableDriver.FindElements(By.CssSelector("table-list table-item")).ToList();

            foreach(var table in uiAllTables)
            {
                var _currentBanker = table.FindElement(By.Id("StatisticsB")).Text;
                var _currentPlayer = table.FindElement(By.Id("StatisticsP")).Text;
                var _currentTie = table.FindElement(By.Id("StatisticsT")).Text;
                var _tableNumber = table.FindElement(By.Id("TableNo")).Text.Replace("T", "");
                var _tableNumberInt = ParseInt(_tableNumber);
                if (_tableNumberInt == 0)
                    continue;

                var newCard = AutomationCardResult.NO_CARD; 
                var scannedResult = new AutomationTableResult
                {
                    TotalBanker = ParseInt(_currentBanker),
                    TotalPlayer = ParseInt(_currentPlayer),
                    TotalTie = ParseInt(_currentTie),
                    TableNumber = _tableNumber
                };

                //Nếu chưa có bàn này trong kết quả
                if (!SavedAllTableResults.Any(c => c.TableNumber == _tableNumber))
                {
                    SavedAllTableResults.Add(scannedResult);
                }
                else
                {
                    var lastTableResult = SavedAllTableResults.FirstOrDefault(c => c.TableNumber == _tableNumber);

                    if (lastTableResult.Total != scannedResult.Total) 
                    {
                        if (lastTableResult.TotalBanker + 1 == scannedResult.TotalBanker)
                        {
                            newCard = AutomationCardResult.BANKER; 
                        }
                        else if (lastTableResult.TotalPlayer + 1 == scannedResult.TotalPlayer)
                        {
                            newCard = AutomationCardResult.PLAYER;
                        }
                        else if (lastTableResult.TotalTie + 1 == scannedResult.TotalTie)
                        {
                            newCard = AutomationCardResult.TIE;
                        }
                        else
                        {
                            newCard = AutomationCardResult.SHOE_CHANGE_OR_CLOSE;
                        }      
                        Log($"Bàn số {_tableNumber}, ra card {newCard}");
                    }
                    SavedAllTableResults.Remove(lastTableResult);
                    SavedAllTableResults.Add(scannedResult);
                }

                var logicTable = GetTable(_tableNumberInt);                
                if (scannedResult.Total == 0)
                {                    
                    if (logicTable == null)
                    {
                        logicTable = new AutoBacRootAlgorithm(StartApp.GlobalConnectionString, _tableNumberInt);
                        LogicAllTables.Add(_tableNumberInt, logicTable);
                        Log($"Tạo mới bàn số {_tableNumberInt} khi chưa có card nào. SessionID: {logicTable.CurrentAutoSession.ID}");
                    }
                    else if (newCard == AutomationCardResult.SHOE_CHANGE_OR_CLOSE)
                    {
                        logicTable.Reset();
                        Log($"Reset bàn số {_tableNumberInt}. SessionID: {logicTable.CurrentAutoSession.ID}");
                    }
                }
                //Trường hợp mới join vào và có đúng 1 kết quả, bàn vẫn đang chơi 
                //nhưng vẫn biết kết quả
                // - Kiểm tra xem bàn hiện tại có bao nhiêu steps
                // - Nếu có 1 step (đang ở đúng bàn) thì không làm gì
                // - Nếu có hơn 1 step thì reset bàn đó
                else if (scannedResult.Total == 1 && logicTable == null)
                {
                    logicTable = new AutoBacRootAlgorithm(StartApp.GlobalConnectionString, _tableNumberInt);
                    LogicAllTables.Add(_tableNumberInt, logicTable);
                    Log($"Tạo mới bàn số {_tableNumberInt} khi có 1 card. SessionID: {logicTable.CurrentAutoSession.ID}");
                    if (newCard == AutomationCardResult.BANKER || newCard == AutomationCardResult.PLAYER)
                    {
                        logicTable.Process((BaccratCard)newCard);
                    }
                }
                else if (logicTable != null && newCard != AutomationCardResult.NO_CARD)
                {                   
                    //Đánh với nhiều thuật toán khác nhau 
                    //Làm việc với những thuật toán chỉ dùng BANKER và PLAYER                    
                    if (newCard == AutomationCardResult.BANKER || newCard == AutomationCardResult.PLAYER)
                    {                        
                        logicTable.Process((BaccratCard)newCard);
                    }
                }
            }

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

        private void btnCamera_Click(object sender, EventArgs e)
        {
            PhotoService.TakeScreenshot(true);
        }

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
                CheckResultTimer.Start();
                SwitchTableTimer.Start();

                CheckResultTimer_Tick(null, null);
            }
            catch (Exception ex)
            {
                txtLog.AppendText(ex.Message + Environment.NewLine);
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        private void btnLoginManually_Click(object sender, EventArgs e)
        {
            //Switch qua màn hình mới 
            AllTableDriver = Driver.SwitchTo().Window(Driver.WindowHandles[1]);

            IsInAllTableView = true;
            CheckResultTimer.Start();
            SwitchTableTimer.Start();

            CheckResultTimer_Tick(null, null);
        }
    }
}
