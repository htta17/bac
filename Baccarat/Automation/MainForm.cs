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
            catch (Exception ex)
            {
                btnTest.Visible = false;
                MessageBox.Show("Vui lòng download Chrome Driver tại: http://chromedriver.storage.googleapis.com/index.html. \r\n\r\n" + ex.Message);

                btnTest.Visible = btnStartStopAuto.Visible = false;
            }

            Timers_Setup();

            Account_Setup();


            //Khởi tạo 10 bàn cho thuật toán ROOT
            if (string.IsNullOrEmpty(StartApp.GlobalConnectionString))
            {
                StartApp.LoadRegistryConnectionString();
            }

            //LogicAllTables = new Dictionary<int, AutoBacRootAlgorithm>();
            AutoBacMaster = new AutoBacMaster(StartApp.GlobalConnectionString);
        }

        #endregion

        #region Properties                
        private readonly ChromeDriver Driver = null;
        private IWebDriver AllTable_NoBet_Driver;

        /// <summary>
        /// Lấy kết quả khi đang ở chế độ tất cả các bàn
        /// hoặc 1 bàn 
        /// </summary>
        private Timer CheckResultTimer { get; set; }

        /// <summary>
        /// Chuyển chế độ giữa tất cả các bàn hoặc 1 bàn cụ thể, chống việc time out
        /// </summary>
        private Timer SwitchTableTimer { get; set; }

        readonly string USER_KEY = "UserName";
        readonly string PWD_KEY = "Password";

        //Dictionary<int, AutoBacRootAlgorithm> LogicAllTables { get; set; }
        AutoBacMaster AutoBacMaster { get; set; }

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
            if (CheckResultTimer == default)
                CheckResultTimer = new Timer();
            if (SwitchTableTimer == default)
                SwitchTableTimer = new Timer();

            CheckResultTimer.Interval = 6_123;  //5 giây 1 lần
            CheckResultTimer.Tick += CheckResultTimer_Tick;

            SwitchTableTimer.Interval = 1000 * 60 * 5; //5 phút
            SwitchTableTimer.Tick += SwitchTableTimer_Tick;
        }

        private void Account_Setup()
        {
            txtUserName.Text = (string)RegisterUtil.LoadRegistry(USER_KEY);
            txtPassword.Text = (string)RegisterUtil.LoadRegistry(PWD_KEY);
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
            if (AllTable_NoBet_Driver == null)
                return;
            
            var table1 = AllTable_NoBet_Driver.FindElements(By.CssSelector(".lobbyTable"))[0];
            table1.Click(); //Nhấn vô bàn số 1
            IsInAllTableView = false;
            CheckResultTimer.Stop();

            System.Threading.Thread.Sleep(2000); //Đợi khoảng 2 giây
            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
            try
            {
                var allTableButton = AllTable_NoBet_Driver.FindElement(By.CssSelector("#IconBaccarat"));
                allTableButton.Click();                
            }
            catch (Exception ex)
            {
                Log(Color.Red, ex.Message);
                LogService.LogError(ex.Message);
            }
            finally
            {
                IsInAllTableView = true;
                CheckResultTimer.Start();
                //CheckResultTimer_Tick(null, null);
            }
            

            //Hiển thị số tiền 
            lb_Balance.Text = Driver.FindElement(By.Id("Balance")).Text;
        }

        int ParseInt(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;
            return int.Parse(s);
        }

        
        private void Log(Color color, string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Color, string>(Log), new object[] { color, text });
                return;
            }
            
            txtLog.InsertOnTop($"{DateTime.Now : yyyy-MM-dd HH:mm:ss}: {text}" 
                + Environment.NewLine , color);
        }

        

        private void SetLabel(AutomationTableResult result, Label banker, Label player, Label tie)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<AutomationTableResult, Label, Label, Label>(SetLabel), 
                    new object[] { result, banker, player , tie });
                return;
            }
            banker.Text = $"{result.TotalBanker}";
            player.Text = $"{result.TotalPlayer}";
            tie.Text = $"{result.TotalTie}";
        }

        private void SetLabel(AutomationTableResult result)
        {         
            switch (result.TableNumber)
            {
                case "1":
                    SetLabel(result, lb_B_1, lb_P_1, lb_T_1);
                    break;
                case "2":
                    SetLabel(result, lb_B_2, lb_P_2, lb_T_2);
                    break;
                case "3":
                    SetLabel(result, lb_B_3, lb_P_3, lb_T_3);
                    break;
                case "4":
                    SetLabel(result, lb_B_4, lb_P_4, lb_T_4);
                    break;
                case "5":
                    SetLabel(result, lb_B_5, lb_P_5, lb_T_5);
                    break;
                case "6":
                    SetLabel(result, lb_B_6, lb_P_6, lb_T_6);
                    break;
                case "7":
                    SetLabel(result, lb_B_7, lb_P_7, lb_T_7);
                    break;
                case "8":
                    SetLabel(result, lb_B_8, lb_P_8, lb_T_8);
                    break;

                case "9":
                    SetLabel(result, lb_B_9, lb_P_9, lb_T_9);
                    break;
                case "10":
                default:
                    SetLabel(result, lb_B_10, lb_P_10, lb_T_10);
                    break;
            }

        }
        //object lock1, lock2, lock3, lock4, lock5, lock6, lock7, lock8, lock9, lock10; 
        public void GetResult_AllTableView_Table(IWebElement table)
        {
            var scannedResult = new AutomationTableResult { };
            var lastTableResult = new AutomationTableResult { };

            var _currentBanker = string.Empty;
            var _currentPlayer = string.Empty;
            var _currentTie = string.Empty; 
            var _tableNumber = string.Empty;
            var _tableNumberInt = 0;
            try
            {
                _currentBanker = table.FindElement(By.Id("StatisticsB")).Text;
                _currentPlayer = table.FindElement(By.Id("StatisticsP")).Text;
                _currentTie = table.FindElement(By.Id("StatisticsT")).Text;
                _tableNumber = table.FindElement(By.Id("TableNo")).Text.Replace("T", "");
                _tableNumberInt = ParseInt(_tableNumber);
            }
            catch (Exception ex)
            {
                Log(Color.Red, ex.Message);
            }
            
            if (_tableNumberInt == 0)
                return;

            //if (System.Threading.Monitor.TryEnter(
            //    _tableNumberInt == 1 ? lock1
            //    : _tableNumberInt == 2 ? lock2
            //    : _tableNumberInt == 3 ? lock3
            //    : _tableNumberInt == 4 ? lock4
            //    : _tableNumberInt == 5 ? lock5
            //    : _tableNumberInt == 6 ? lock6
            //    : _tableNumberInt == 7 ? lock7
            //    : _tableNumberInt == 8 ? lock8
            //    : _tableNumberInt == 9 ? lock9
            //    : lock10
            //    ))
            //{
            //    try
            //    {
                    scannedResult = new AutomationTableResult
                    {
                        TotalBanker = ParseInt(_currentBanker),
                        TotalPlayer = ParseInt(_currentPlayer),
                        TotalTie = ParseInt(_currentTie),
                        TableNumber = _tableNumber
                    };
                    SetLabel(scannedResult);

                    BaccratCard newCard = BaccratCard.NoTrade;
                    lastTableResult = SavedAllTableResults.FirstOrDefault(c => c.TableNumber == _tableNumber);

                    //Nếu chưa có bàn này trong kết quả
                    if (lastTableResult == null)
                    {
                        SavedAllTableResults.Add(scannedResult);
                    }
                    else
                    {
                        if (lastTableResult.Total != scannedResult.Total)
                        {
                            if (chboxShowDetail.Checked)
                            {
                                Log(Color.Black, $"Bàn {_tableNumber}: { lastTableResult.TextResult } ---> { scannedResult.TextResult }");
                            }

                            var predict = new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };

                            if (scannedResult.Total == 0)
                            {
                                var newSessionID = AutoBacMaster.ResetTable(_tableNumberInt);
                                Log(Color.Green, $"Tạo mới bàn số {_tableNumberInt} khi chưa có card nào. SessionID: {newSessionID}.");
                            }
                            else if (scannedResult.Total == 1 && AutoBacMaster.TableIsNull(_tableNumberInt))
                            {

                                //Vừa mới join vào, đáng lẽ đợi hết phiên
                                //nhưng vì mới có 1 card, nên có thể chơi chơi luôn vì có thể vẫn kịp
                                newCard = scannedResult.TotalBanker == 1 ? BaccratCard.Banker :
                                                scannedResult.TotalPlayer == 1 ? BaccratCard.Player : BaccratCard.Tie;

                                var newSessionID = AutoBacMaster.ResetTable(_tableNumberInt);
                                Log(Color.Green, $"Tạo mới bàn số {_tableNumberInt} khi có 1 card  {newCard}. SessionID: {newSessionID}.");

                                predict = AutoBacMaster.Process(_tableNumberInt, newCard, scannedResult);
                            }
                            else if (lastTableResult.Total + 1 == scannedResult.Total)
                            {
                                newCard = lastTableResult.TotalBanker + 1 == scannedResult.TotalBanker ? BaccratCard.Banker
                                            : lastTableResult.TotalPlayer + 1 == scannedResult.TotalPlayer ? BaccratCard.Player
                                            : BaccratCard.Tie;
                                predict = AutoBacMaster.Process(_tableNumberInt, newCard, scannedResult);
                            }
                            if (chBoxShowPredict.Checked && predict.Value != BaccratCard.NoTrade)
                            {
                                Log(Color.Green, $"Bàn số {_tableNumber}, ra card {newCard}, dự đoán card tiếp {predict.Value} {predict.Volume} units");
                            }
                        }

                        SavedAllTableResults.Remove(lastTableResult);
                        SavedAllTableResults.Add(scannedResult);
                    }
            //    }
            //    catch(Exception ex)
            //    {
            //        Log(Color.Red, ex.Message);
            //        LogService.LogError(ex.Message);
            //    }
            //    finally
            //    {
            //        System.Threading.Monitor.Exit(SavedAllTableResults.FirstOrDefault(c => c.TableNumber == _tableNumber));
            //    }
            //}   
        }


        /// <summary>
        /// Lấy và xử lý kết quả cho tất cả các bàn
        /// </summary>
        private void GetResult_AllTableView()
        {
            //Lấy kết quả BANKER, PLAYER và TIE ở tất cả các bàn 
            var uiAllTables = AllTable_NoBet_Driver.FindElements(By.CssSelector("table-list table-item")).ToList();
            
            foreach (var table in uiAllTables)
            {
                //Mỗi bàn mỗi thread (bàn này khỏi phải đợi bàn kia)
                //var thread = new System.Threading.Thread(() =>
                //{
                    GetResult_AllTableView_Table(table);
                //});
                //thread.Start();
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
            CheckResultTimer.Stop();
            SwitchTableTimer.Stop();

            CheckResultTimer.Dispose();
            SwitchTableTimer.Dispose();

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

            RegisterUtil.SaveRegistry(USER_KEY, txtUserName.Text);
            RegisterUtil.SaveRegistry(PWD_KEY, txtPassword.Text);

            try
            {
                Driver.Navigate().GoToUrl("https://www.jbbodds.com/vi-vn");
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5.0);

                Driver.FindElement(By.CssSelector(".input-username input[name=username]")).SendKeys(txtUserName.Text);
                Driver.FindElement(By.CssSelector(".input-password input[name=password]")).SendKeys(txtPassword.Text);
                Driver.FindElement(By.CssSelector("button[type=submit]")).Click();
                System.Threading.Thread.Sleep(500); //Đợi nửa giây
                //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5.0);

                var liveCasino = Driver.FindElements(By.CssSelector("#menu-products > li > a"))[2];
                liveCasino.Click();
                System.Threading.Thread.Sleep(5000); //Đợi 5 giây 
                //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5.0);

                var k9 = Driver.FindElements(By.CssSelector(".casino-list li"))[0];
                Actions actions = new Actions(Driver);
                actions.MoveToElement(k9).Perform(); //Đưa chuột lên phần K9
                System.Threading.Thread.Sleep(2000); //Đợi 2 giây
                //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2.0);

                var playNowGrandSuite = Driver.FindElements(By.CssSelector(".game-popup a.btn-orange"))[0];
                playNowGrandSuite.Click();
                System.Threading.Thread.Sleep(1000);//Đợi 2 giây cho xuất hiện nút OK
                //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.0);

                var okButton = Driver.FindElement(By.CssSelector(".modal-dialog .text-center button.bet-btn"));
                okButton.Click(); //Nhấn nút OK
                System.Threading.Thread.Sleep(5000);//Đợi 5 giây để màn hình mới load
                //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5.0);

                EnableAuto(true);
            }
            catch (Exception ex)
            {
                Log(Color.Red, ex.Message);
                LogService.LogError(ex.Message);
            }
        }
        
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }
        private void EnableAuto(bool isEnable)
        {
            if (isEnable)
            {
                //Switch qua màn hình mới 
                if (Driver.WindowHandles.Count > 1)
                {
                    AllTable_NoBet_Driver = Driver.SwitchTo().Window(Driver.WindowHandles[1]);
                }
                IsInAllTableView = true;

                CheckResultTimer.Start();
                SwitchTableTimer.Start();

                btnStartStopAuto.Text = "Stop auto";
                btnStartStopAuto.ForeColor = Color.Red;
                Log(Color.Green, "Start auto successfully.");
                CheckResultTimer_Tick(null, null);
            }
            else
            {
                CheckResultTimer.Stop();
                SwitchTableTimer.Stop();

                btnStartStopAuto.Text = "Start auto";
                btnStartStopAuto.ForeColor = Color.Green;
                Log(Color.Green, "Stop auto successfully.");
            }

            //Cập nhật status trên UI
            if (CheckResultTimer == default)
            {
                lb_TimerStatus.Text = "Sleeping";
                lb_TimerStatus.BackColor = Color.Gray;
            }
            else if (CheckResultTimer.Enabled)
            {
                lb_TimerStatus.Text = "Running";
                lb_TimerStatus.BackColor = Color.Green;
            }
            else if (!CheckResultTimer.Enabled)
            {
                lb_TimerStatus.Text = "Stopped";
                lb_TimerStatus.BackColor = Color.Red;
            }
        }

        private void btnLoginManually_Click(object sender, EventArgs e)
        {
            if (CheckResultTimer.Enabled) // Đang chạy
            {
                EnableAuto(false);
            }
            else //Đang dừng
            {
                EnableAuto(true);
            }            
        }
       
        private void btnScanTableView_Click(object sender, EventArgs e)
        {
            //Switch qua màn hình mới 
            if (Driver.WindowHandles.Count > 1)
            {
                AllTable_NoBet_Driver = Driver.SwitchTo().Window(Driver.WindowHandles[1]);
            }
            IsInAllTableView = false; //Làm việc với view bàn 7           

            var table1 = AllTable_NoBet_Driver.FindElements(By.CssSelector(".lobbyTable"))[1];
            table1.Click(); //Nhấn vô bàn số 7
        }
    }
}
