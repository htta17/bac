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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Midas
{
    public partial class AutoLogin : Form
    {
        public AutoLogin()
        {
            InitializeComponent();

            numInterval.Value = 5;
            Timer.Interval = 1000 * 60 * (int)numInterval.Value; //5 mins
            Timer.Tick += Timer_Tick;

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
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TakeScreenshot(false);
        }
        const string IMAGE_FORMAT = FOLDER_FORMAT + "\\Image_{0:HHmmss}.jpeg";
        const string FOLDER_FORMAT = "Logs\\{0:yyyy-MM-dd}"; 

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

        Timer Timer = new Timer();
        private readonly ChromeDriver Driver = null; 

        private void AutoLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Driver?.Quit();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (Driver == null)
                return; 

            try
            {
                Driver.Navigate().GoToUrl("https://www.jbbodds.com/vi-vn");

                Driver.FindElement(By.CssSelector(".input-username input[name=username]")).SendKeys(txtUserName.Text);
                Driver.FindElement(By.CssSelector(".input-password input[name=password]")).SendKeys(txtPassword.Text);
                Driver.FindElement(By.CssSelector("button[type=submit]")).Click();                
            }
            catch (Exception ex)
            { 
            
            }           

        }

        private void AutoLogin_Load(object sender, EventArgs e)
        {
            if (Driver == null)
                return;

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20.0);
            Driver.Manage().Window.Maximize();
        }

        bool StatusEnabled { get; set; } = false;

        private void button1_Click(object sender, EventArgs e)
        {
            StatusEnabled = !StatusEnabled;
            if (StatusEnabled)
            {
                Timer.Interval = (int)numInterval.Value * 1000 * 60;
                Timer.Start();
                btnTakePhoto.Text = "Stop";
                btnTakePhoto.ForeColor = Color.Red;
            }
            else 
            {
                Timer.Stop();
                btnTakePhoto.Text = "Start";
                btnTakePhoto.ForeColor = Color.Green;
            }
            
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            TakeScreenshot(false);
        }
    }
}
