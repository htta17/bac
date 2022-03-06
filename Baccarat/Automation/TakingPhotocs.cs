using Midas.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Midas.Automation
{
    public partial class TakingPhotocs : Form
    {
        public TakingPhotocs()
        {
            InitializeComponent();
        }
        bool StatusEnabled { get; set; } = false;
        Timer PhotoTakenTimer = new Timer();
        private readonly ChromeDriver Driver = null;
        private IWebDriver AllTableDriver;

        const string IMAGE_FORMAT = FOLDER_FORMAT + "\\Image_{0:HHmmss}.png";
        const string FOLDER_FORMAT = "Logs\\{0:yyyy-MM-dd}";
        const string AUTO_LOG_FOLDER = "Logs\\AUTO\\{0:yyyy-MM-dd}.log";

        private void UIColor_Setup()
        {
            //Màu cho status 
            btnTakePhoto.ForeColor = Color.Green;
            lbCurrentStatus.BackColor = Color.Red;
            lbCurrentStatus.ForeColor = Color.White;
            lbCurrentStatus.Text = "STOPPED";
        }

        private void Timers_Setup()
        {
            numInterval.Value = 5;
            PhotoTakenTimer.Interval = 1000 * 60 * (int)numInterval.Value; //5 mins
            PhotoTakenTimer.Tick += PhotoTakenTimer_Tick; ;            
        }

        private void PhotoTakenTimer_Tick(object sender, EventArgs e)
        {
            PhotoService.TakeScreenshot(false);
        }
        
        private void btnTakePhoto_Click(object sender, EventArgs e)
        {
            StatusEnabled = !StatusEnabled;
            if (StatusEnabled)
            {
                PhotoTakenTimer.Interval = (int)numInterval.Value * 1000 * 60;
                PhotoTakenTimer.Start();
                btnTakePhoto.Text = "STOP Taking Photo";
                btnTakePhoto.ForeColor = Color.Red;

                lbCurrentStatus.BackColor = Color.Green;
                lbCurrentStatus.ForeColor = Color.White;
                lbCurrentStatus.Text = "STARTED";
            }
            else
            {
                PhotoTakenTimer.Stop();
                btnTakePhoto.Text = "START Taking Photo";
                btnTakePhoto.ForeColor = Color.Green;

                lbCurrentStatus.BackColor = Color.Red;
                lbCurrentStatus.ForeColor = Color.White;
                lbCurrentStatus.Text = "STOPPED";
            }
        }
    }
}
