using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge; 
using System.Reflection;
using System.IO;
using Midas.Utils;
using CoreLogic.StandardlizedAlgorithms;
using CoreLogic;

namespace Midas.Automation
{
    public partial class AutoTrade1Table : Form
    {
        public AutoTrade1Table()
        {
            InitializeComponent();

            //tableLimit-item-content
            //history-statistic-baccarat .tableLimit-item-content (0,1,2)
            //main-game-baccarat

            txtUserName.Text = (string)RegisterUtil.LoadRegistry(RegisterUtil.TRADE_USER_KEY);
            txtPassword.Text = (string)RegisterUtil.LoadRegistry(RegisterUtil.TRADE_PWD_KEY);

            cbxTable.SelectedIndex = 0;
            cbxBrowser.SelectedIndex = 0;

            SetBaseUnit();

            IsAutoRunning = false;
            CurrentBanker = CurrentPlayer = CurrentTie = -1;

            AutoBacMaster = new AutoBacMaster(StartApp.GlobalConnectionString);
        }

        void Trade(BaccaratPredict predict, int tableNum, int baseUnit = 30)
        {
            if (MainDriver.WindowHandles.Count > 1)
            {
                TradeDriver = MainDriver.SwitchTo().Window(MainDriver.WindowHandles[1]);
            }

            if (!chAllowTrade.Checked || TradeDriver == null || predict == null || predict.Volume == 0)
                return;

            //Tìm đến nút để nhấn
            //OverlayChipMsg0102001 --Nút TIE 
            //OverlayChipMsg0101002 --Nút PLAYER 
            //OverlayChipMsg0101001 --Nút BANKER            
            var buttonID = predict.Value == BaccratCard.Banker ? "OverlayChipMsg0101001"
                                : predict.Value == BaccratCard.Player ? "OverlayChipMsg0101002"
                                : "OverlayChipMsg0102001";

            var finalAmount = predict.Volume * baseUnit;

            try
            {
                UIProcess.ClickOnChips(TradeDriver, tableNum, finalAmount, buttonID, false);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        void Timer_Scan_Page(object state)
        {
            if (MainDriver.WindowHandles.Count > 1)
            {
                TradeDriver = MainDriver.SwitchTo().Window(MainDriver.WindowHandles[1]);
            }
            

            //var infoUI = TradeDriver.FindElement(By.Id("GameID"));
            //if (infoUI != null)
            //{
            //    var infos = infoUI.Text.Split('-');
            //    //int.TryParse(infos[2], out _scannedTotal);
            //}
            var scannedItems = TradeDriver.FindElements(By.CssSelector("history-statistic-baccarat .tableLimit-item-content"));
            if (scannedItems != null && scannedItems.Count > 3)
            {
                int.TryParse(scannedItems[0].Text, out int _scannedBanker);
                int.TryParse(scannedItems[1].Text, out int _scannedPlayer);
                int.TryParse(scannedItems[2].Text, out int _scannedTie);
                var _scannedTotal = _scannedBanker + _scannedPlayer + _scannedTie;

                if (CurrentBanker == -1)
                {
                    CurrentBanker = _scannedBanker;
                    CurrentPlayer = _scannedPlayer;
                    CurrentTie = _scannedTie;
                }
                else if (_scannedTotal != CurrentTotal)
                {
                    Log($"[All, B,P,T]: [{CurrentTotal}, {CurrentBanker}, {CurrentPlayer},{CurrentTie}]-->[{_scannedTotal}, {_scannedBanker}, {_scannedPlayer}, {_scannedTie}]");
                    BaccratCard newCard = BaccratCard.NoTrade;
                    if (_scannedTotal == 0)
                    {
                        var newThread = new System.Threading.Thread(() =>
                        {
                            var newSessionID = AutoBacMaster.ResetTable(TableNumber);
                            Log($"Tạo mới bàn số {TableNumber} khi chưa có card nào. SessionID: {newSessionID}.");

                            var predict = AutoBacMaster.Process(TableNumber, newCard, new AutomationTableResult
                            {
                                TableNumber = TableNumber.ToString(),
                                TotalBanker = 0,
                                TotalPlayer = 0,
                                TotalTie = 0
                            });
                            Trade(predict, TableNumber, BaseUnit);

                            
                            Log($"Bàn số {TableNumber}, dự đoán card tiếp dựa trên kết quả phiên cũ {predict}");

                        });
                        newThread.Start();
                    }
                    else if (_scannedTotal == 1 && AutoBacMaster.TableIsNull(TableNumber))
                    {
                        newCard = _scannedBanker == 1 ? BaccratCard.Banker :
                                        _scannedPlayer == 1 ? BaccratCard.Player : BaccratCard.Tie;

                        //Vừa mới join vào, đáng lẽ đợi hết phiên
                        //nhưng vì mới có 1 card, nên có thể chơi chơi luôn vì có thể vẫn kịp

                        var newThread = new System.Threading.Thread(() =>
                        {
                            var newSessionID = AutoBacMaster.ResetTable(TableNumber);
                            Log($"Tạo mới bàn số {TableNumber} khi có 1 card  {newCard}. SessionID: {newSessionID}.");

                            var predict = AutoBacMaster.Process(TableNumber, newCard, new AutomationTableResult
                            {
                                TableNumber = TableNumber.ToString(),
                                TotalBanker = _scannedBanker,
                                TotalPlayer = _scannedPlayer,
                                TotalTie = _scannedTie
                            });

                            Trade(predict, TableNumber, BaseUnit);

                            Log($"Bàn số {TableNumber}, ra card {newCard}, dự đoán card tiếp {predict}");
                        });
                        newThread.Start();

                    }
                    else if (CurrentTotal + 1 == _scannedTotal)
                    {
                        newCard = CurrentBanker + 1 == _scannedBanker ? BaccratCard.Banker
                                           : CurrentPlayer + 1 == _scannedPlayer ? BaccratCard.Player
                                           : BaccratCard.Tie;

                        var newThread = new System.Threading.Thread(() =>
                        {                           
                            var predict = AutoBacMaster.Process(TableNumber, newCard, new AutomationTableResult { TableNumber = TableNumber.ToString(), TotalBanker = _scannedBanker, TotalPlayer = _scannedPlayer, TotalTie = _scannedTie  });

                            Trade(predict, TableNumber, BaseUnit);

                            Log($"Bàn số {TableNumber}, ra card {newCard}, dự đoán card tiếp {predict}");

                        });
                        newThread.Start();
                    }

                    CurrentBanker = _scannedBanker;
                    CurrentPlayer = _scannedPlayer;
                    CurrentTie = _scannedTie;
                }
            }
        }


        private WebDriver MainDriver { get; set; }
        private IWebDriver TradeDriver { get; set; }
        private int TableNumber { get; set; }
        private int BaseUnit { get; set; }
        private bool IsAutoRunning { get; set; }

        private int CurrentBanker { get; set; }
        private int CurrentPlayer { get; set; }
        private int CurrentTie { get; set; }

        private int CurrentTotal { get { return CurrentBanker + CurrentPlayer + CurrentTie; } }

        private System.Threading.Timer Timer { get; set; }

        AutoBacMaster AutoBacMaster { get; set; }

        const int TIME_PERIOD = 5_000;
        const int TIME_START = 500;

        private void SetBaseUnit()
        {
            int.TryParse(txtBaseUnit.Text, out int _baseUnit);
            BaseUnit = _baseUnit < 30 ? 30 : _baseUnit;
        }

        private void Log(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(Log), new object[] { text });
                return;
            }
            text = text + Environment.NewLine;
            txtLog.InsertOnTop($"{DateTime.Now:yy-MM-dd HH:mm:ss}:{ text}", Color.Black);
            LogService.Log(text);
        }

        private void EnableAuto()
        {
            if (IsAutoRunning)
            {
                IsAutoRunning = false;
                lb_TimerStatus.Text = "Đang dừng";
                lb_TimerStatus.BackColor = Color.Red;
                btnStartAuto.Text = "Chạy tự động";
                Timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            }
            else
            {
                IsAutoRunning = true;
                lb_TimerStatus.Text = "Chạy tự động";
                lb_TimerStatus.BackColor = Color.Green;
                btnStartAuto.Text = "Dừng tự động";

                if (Timer == default)
                {
                    Timer = new System.Threading.Timer(new System.Threading.TimerCallback(Timer_Scan_Page),
                        null,
                        TIME_START,
                        TIME_PERIOD
                        );
                }
                else
                {
                    Timer.Change(TIME_START, TIME_PERIOD);
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            TableNumber = cbxTable.SelectedIndex + 1;

            if (MainDriver == null)
            {
                try
                {
                    if (cbxBrowser.SelectedIndex == 0)
                    {
                        MainDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                    }
                    else if (cbxBrowser.SelectedIndex == 1)
                    {
                        MainDriver = new FirefoxDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                    }
                    else if (cbxBrowser.SelectedIndex == 2)
                    {
                        MainDriver = new EdgeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    btnLogin.Visible = false;
                    return;
                }
            }           

            RegisterUtil.SaveRegistry(RegisterUtil.USER_KEY, txtUserName.Text);
            RegisterUtil.SaveRegistry(RegisterUtil.PWD_KEY, txtPassword.Text);

            try
            {
                var loginSuccessed = UIProcess.LoginToTheSite(MainDriver, txtUserName.Text, txtPassword.Text);

                if (MainDriver.WindowHandles.Count > 1)
                {
                    TradeDriver = MainDriver.SwitchTo().Window(MainDriver.WindowHandles[1]);
                }
                var table_N = TradeDriver.FindElements(By.CssSelector(".lobbyTable"))[ TableNumber - 1];
                table_N.Click(); //Nhấn vào bàn 

                EnableAuto();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private void btnStartAuto_Click(object sender, EventArgs e)
        {
            EnableAuto();
        }

        private void btnChangeTable_Click(object sender, EventArgs e)
        {

        }

        private void AutoTrade1Table_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAutoRunning)
            {
                EnableAuto();
            }
            MainDriver?.Quit();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Timer_Scan_Page(null);
        }
    }
}
