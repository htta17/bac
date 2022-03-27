using Midas.Automation;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

using OpenQA.Selenium.Edge;
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
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chromium;


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
                CollectDataDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            catch (Exception ex)
            {
                btnCollectDataAuto.Visible = false;
                MessageBox.Show(ex.Message);

                btnCollectDataAuto.Visible = btnStartStopAuto.Visible = false;
            }                   

            Timers_Setup();

            Account_Setup();

            SetBaseUnit();

            //Khởi tạo 10 bàn cho thuật toán ROOT
            if (string.IsNullOrEmpty(StartApp.GlobalConnectionString))
            {
                StartApp.LoadRegistryConnectionString();
            }
            
            AutoBacMaster = new AutoBacMaster(StartApp.GlobalConnectionString);            
        }

        #endregion

        #region Properties                
        private readonly ChromeDriver CollectDataDriver = null;
        private IWebDriver CollectData_Scanned_Driver;
        private EdgeDriver TradeDriver = null;
        private IWebDriver Trade_Action_Driver;  
        /// <summary>
        /// Lấy kết quả khi đang ở chế độ tất cả các bàn
        /// hoặc 1 bàn 
        /// </summary>
        private System.Timers.Timer CheckResultTimer { get; set; }

        private int BaseUnit { get; set; }

        /// <summary>
        /// Chuyển chế độ giữa tất cả các bàn hoặc 1 bàn cụ thể, chống việc time out
        /// </summary>
        private System.Timers.Timer SwitchTableTimer { get; set; }

        //private IDictionary<int, System.Threading.Thread> AllThreads  {get;set;}

        

        const string SAVED_SESSION_FILENAME_KEY = "BrowserSession.json";

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
                CheckResultTimer = new System.Timers.Timer();
            if (SwitchTableTimer == default)
                SwitchTableTimer = new System.Timers.Timer();

            CheckResultTimer.Interval = 6_123;  //5 giây 1 lần
            CheckResultTimer.Elapsed += CheckResultTimer_Tick;

            SwitchTableTimer.Interval = 1000 * 60 * 5; //5 phút
            SwitchTableTimer.Elapsed += SwitchTableTimer_Tick;
        }

        private void Account_Setup()
        {            
            txtUserName.Text = (string)RegisterUtil.LoadRegistry(RegisterUtil.USER_KEY);
            txtPassword.Text = (string)RegisterUtil.LoadRegistry(RegisterUtil.PWD_KEY);
            txtTradeUser.Text = (string)RegisterUtil.LoadRegistry(RegisterUtil.TRADE_USER_KEY);
            txtTradePassword.Text = (string)(RegisterUtil.LoadRegistry(RegisterUtil.TRADE_PWD_KEY));
        }

        private void SetBaseUnit()
        {
            int.TryParse(txtBaseUnit.Text, out int _baseUnit);
            BaseUnit = _baseUnit < 30 ? 30 : _baseUnit;
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
            if (CollectData_Scanned_Driver == null)
                return;

            //var table1 = CollectData_Scanned_Driver.FindElement(By.CssSelector("#IconAllTables"));
            var table1 = CollectData_Scanned_Driver.FindElements(By.CssSelector(".lobbyTable"))[0];
            table1.Click(); //Nhấn vô chế độ tất cả các bàn
            IsInAllTableView = false;
            CheckResultTimer.Stop();

            System.Threading.Thread.Sleep(2000); //Đợi khoảng 2 giây
            try
            {
                var allTableButton = CollectData_Scanned_Driver.FindElement(By.CssSelector("#IconBaccarat"));
                allTableButton.Click();

                //Hiển thị số tiền
                //SetLabel(CollectDataDriver.FindElement(By.Id("Balance")).Text, lb_Balance);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
            finally
            {
                IsInAllTableView = true;
                CheckResultTimer.Start();
            }
        }

        int ParseInt(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;
            return int.Parse(s);
        }

        private void Log(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(Log), new object[] { text });
                return;
            }

            txtLog.InsertOnTop($"{DateTime.Now: yyyy-MM-dd HH:mm:ss}: {text}"
                + Environment.NewLine, Color.Black);
            LogService.Log(text);
        }
        private void SetLabel(AutomationTableResult result, Label banker, Label player, Label tie)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<AutomationTableResult, Label, Label, Label>(SetLabel),
                    new object[] { result, banker, player, tie });
                return;
            }
            banker.Text = $"{result.TotalBanker}";
            player.Text = $"{result.TotalPlayer}";
            tie.Text = $"{result.TotalTie}";
        }

        private void SetLabel(string text, Label label)
        {
            if (InvokeRequired)
            { 
                this.Invoke(new Action<string, Label>(SetLabel), new object[] { text, label });
                return;
            }
            label.Text = text;
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
                Log(ex.Message);
            }

            if (_tableNumberInt == 0)
                return;

            CheckBox chkBox = this.Controls.Find($"chbxCollect{_tableNumberInt}", true).FirstOrDefault() as CheckBox;
            if (chkBox != null && !chkBox.Checked)
                return;


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
            else if (lastTableResult.Total != scannedResult.Total)
            {

                var detailResult = $"Bàn {_tableNumber}: [All,B,P,T]:[{lastTableResult.Total},{lastTableResult.TotalBanker},{lastTableResult.TotalPlayer},{lastTableResult.TotalTie}]"; 
                detailResult += $"-->[{scannedResult.Total},{scannedResult.TotalBanker},{scannedResult.TotalPlayer},{scannedResult.TotalTie}].";
                                

                if (scannedResult.Total == 0)
                {
                    //Task.Run(() =>
                    var newThread = new System.Threading.Thread(() =>
                    {
                        var newSessionID = AutoBacMaster.ResetTable(_tableNumberInt);
                        Log($"Tạo mới bàn số {_tableNumberInt} khi chưa có card nào. SessionID: {newSessionID}.");

                        var predict = AutoBacMaster.Process(_tableNumberInt, newCard, new AutomationTableResult
                        {
                            TableNumber = _tableNumberInt.ToString(),
                            TotalBanker = 0,
                            TotalPlayer = 0,
                            TotalTie = 0
                        });
                        
                        Trade(predict, _tableNumberInt, BaseUnit);

                        Log(detailResult + $" Dự đoán: {predict}");
                       
                        
                    });
                    newThread.Start();
                }
                else if (scannedResult.Total == 1 && AutoBacMaster.TableIsNull(_tableNumberInt))
                {
                    newCard = scannedResult.TotalBanker == 1 ? BaccratCard.Banker :
                                    scannedResult.TotalPlayer == 1 ? BaccratCard.Player : BaccratCard.Tie;

                    //Vừa mới join vào, đáng lẽ đợi hết phiên
                    //nhưng vì mới có 1 card, nên có thể chơi chơi luôn vì có thể vẫn kịp
                        
                    var newThread = new System.Threading.Thread(() =>
                    {
                        var newSessionID = AutoBacMaster.ResetTable(_tableNumberInt);
                        Log($"Tạo mới bàn số {_tableNumberInt} khi có 1 card  {newCard}. SessionID: {newSessionID}.");

                        var predict = AutoBacMaster.Process(_tableNumberInt, newCard, scannedResult);

                        Trade(predict, _tableNumberInt, BaseUnit);

                        Log(detailResult + $" Card { newCard.ToString().ToUpper() }. Dự đoán: {predict}");                        
                    });
                    newThread.Start();                        
                }
                else if (lastTableResult.Total + 1 == scannedResult.Total)
                {
                    newCard = lastTableResult.TotalBanker + 1 == scannedResult.TotalBanker ? BaccratCard.Banker
                                        : lastTableResult.TotalPlayer + 1 == scannedResult.TotalPlayer ? BaccratCard.Player
                                        : BaccratCard.Tie;
                    var newThread = new System.Threading.Thread(() =>
                    {
                        var predict = AutoBacMaster.Process(_tableNumberInt, newCard, scannedResult);

                        Trade(predict, _tableNumberInt, BaseUnit);

                        Log(detailResult + $" Card { newCard.ToString().ToUpper() }. Dự đoán: {predict}");
                    });
                    newThread.Start();
                }
                SavedAllTableResults.Remove(lastTableResult);
                SavedAllTableResults.Add(scannedResult);                                
            }


            //End of function

        }


        /// <summary>
        /// Lấy và xử lý kết quả cho tất cả các bàn
        /// </summary>
        private void GetResult_AllTableView()
        {
            //Lấy kết quả BANKER, PLAYER và TIE ở tất cả các bàn             
            try
            {
                var uiAllTables = CollectData_Scanned_Driver.FindElements(By.CssSelector("table-list table-item")).ToList();
                foreach (var table in uiAllTables)
                {                    
                    GetResult_AllTableView_Table(table);
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
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
            if (CollectDataDriver == null)
                return;

            CollectDataDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20.0);
            CollectDataDriver.Manage().Window.Maximize();
        }

        private void AutoLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckResultTimer.Stop();
            SwitchTableTimer.Stop();

            CheckResultTimer.Dispose();
            SwitchTableTimer.Dispose();

            CollectDataDriver?.Quit();
            TradeDriver?.Quit();
        }
        #endregion         

        private void btnCamera_Click(object sender, EventArgs e)
        {            
            var screenShot = CollectDataDriver.GetScreenshot();
            PhotoService.TakeScreenshot(screenShot);
        }        

        private void btn_CollectData_Click(object sender, EventArgs e)
        {
            if (CollectDataDriver == null)
                return;

            RegisterUtil.SaveRegistry(RegisterUtil.USER_KEY, txtUserName.Text);
            RegisterUtil.SaveRegistry(RegisterUtil.PWD_KEY, txtPassword.Text);
            
            try
            {                
                var loginSuccessed = UIProcess.LoginToTheSite(CollectDataDriver, txtUserName.Text, txtPassword.Text);

                if (loginSuccessed)
                    EnableAuto(true);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
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
                if (CollectDataDriver.WindowHandles.Count > 1)
                {
                    CollectData_Scanned_Driver = CollectDataDriver.SwitchTo().Window(CollectDataDriver.WindowHandles[1]);

                    //Lưu lại sessionID, nếu cần thì load lại 
                    File.AppendAllText(SAVED_SESSION_FILENAME_KEY, JsonConvert.SerializeObject(
                        new BrowserSession {
                        SessionID = CollectDataDriver.SessionId,
                        URL = CollectDataDriver.Url}));

                }
                IsInAllTableView = true;

                CheckResultTimer.Start();
                SwitchTableTimer.Start();

                btnStartStopAuto.Text = "Stop auto";
                btnStartStopAuto.ForeColor = Color.Red;
                Log("Start auto successfully.");
                CheckResultTimer_Tick(null, null);
            }
            else
            {
                CheckResultTimer.Stop();
                SwitchTableTimer.Stop();

                btnStartStopAuto.Text = "Start auto";
                btnStartStopAuto.ForeColor = Color.Green;
                Log( "Stop auto successfully.");
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
            if (CollectDataDriver.WindowHandles.Count > 1)
            {
                CollectData_Scanned_Driver = CollectDataDriver.SwitchTo().Window(CollectDataDriver.WindowHandles[1]);
            }
            IsInAllTableView = false; 

            var table1 = CollectData_Scanned_Driver.FindElements(By.CssSelector(".lobbyTable"))[1];
            table1.Click(); //Nhấn vô bàn số 7
        }

        private void btnTradeLogin_Click(object sender, EventArgs e)
        {
            try
            {
                TradeDriver = new EdgeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (TradeDriver == null)
                return;

            RegisterUtil.SaveRegistry(RegisterUtil.TRADE_USER_KEY, txtTradeUser.Text);
            RegisterUtil.SaveRegistry(RegisterUtil.TRADE_PWD_KEY, txtTradePassword.Text);

            var anotherThread = new System.Threading.Thread(() =>
            {
                var loginSuccess = UIProcess.LoginToTheSite(TradeDriver, txtTradeUser.Text, txtTradePassword.Text);
                                
                try
                {
                    if (loginSuccess && TradeDriver.WindowHandles.Count > 1)
                    {
                        Trade_Action_Driver = TradeDriver.SwitchTo().Window(TradeDriver.WindowHandles[1]);
                        System.Threading.Thread.Sleep(100);
                        var allTable = Trade_Action_Driver.FindElement(By.CssSelector("#IconAllTables"));
                        allTable.Click(); //Nhấn vô chế độ tất cả các bàn
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    LogService.LogError(ex.Message);
                }                
                
            });

            anotherThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predict"></param>
        /// <param name="tableNum"></param>
        /// <param name="baseUnit">Default 30K</param>
        void Trade(BaccaratPredict predict, int tableNum, int baseUnit = 30)
        {
            if (TradeDriver == null)
                return;
            if (TradeDriver.WindowHandles.Count > 1)
            {
                Trade_Action_Driver = TradeDriver.SwitchTo().Window(TradeDriver.WindowHandles[1]);
            }
            CheckBox chkBox = this.Controls.Find($"chkTb{tableNum}", true).FirstOrDefault() as CheckBox;

            if (!chkAllowAutomatic.Checked || Trade_Action_Driver == null || predict == null || predict.Volume == 0)
                return;                

            if (!chkBox.Checked)            
            {                
                return;
            }                

            //Tìm đến nút để nhấn
            //OverlayChipMsg0102001 --Nút TIE 
            //OverlayChipMsg0101002 --Nút PLAYER 
            //OverlayChipMsg0101001 --Nút BANKER            
            var buttonID = predict.Value == BaccratCard.Banker ? "OverlayChipMsg0101001"
                                : predict.Value == BaccratCard.Player ? "OverlayChipMsg0101002"
                                : "OverlayChipMsg0102001";

            var finalAmount = predict.Volume * baseUnit;

            if (TradeDriver.WindowHandles.Count > 1)
            {
                Trade_Action_Driver = TradeDriver.SwitchTo().Window(TradeDriver.WindowHandles[1]);
            }

            try
            {                
                UIProcess.ClickOnChips(Trade_Action_Driver, tableNum, finalAmount, buttonID);                
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }            
        }       

        private void btnSetBaseUnit_Click(object sender, EventArgs e)
        {
            SetBaseUnit();
        }
    }
}
