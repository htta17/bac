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

            Timer.Interval = 1000 * 60 * 5; //5 mins
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
        const string IMAGE_FORMAT = "Logs\\zz_NewPhoto_{0:yyyyMMdd_HHmmss}.jpeg";
        

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
                    bitmap.Save(string.Format(IMAGE_FORMAT, DateTime.Now), ImageFormat.Jpeg);
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

                Driver.FindElement(By.CssSelector(".input-username input[name=username]")).SendKeys("winter9");
                Driver.FindElement(By.CssSelector(".input-password input[name=password]")).SendKeys("satthu123");
                Driver.FindElement(By.CssSelector("button[type=submit]")).Click();

                //Driver.FindElement(By.CssSelector("a[href=\"/vi-vn/live\"]")).Click();

                //var baccaratUImage = Driver.FindElement(By.CssSelector("img[src=https://doc-cdn.docb18a1.com/contents/images/live/471x439-Generic_Baccarat.jpg]"));

                ////Instantiating Actions class
                //Actions actions = new Actions(Driver);

                ////Hovering on main menu
                //actions.MoveToElement(baccaratUImage);
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

        bool Enabled = false;

        private void button1_Click(object sender, EventArgs e)
        {
            Enabled = !Enabled;
            if (Enabled)
            {
                Timer.Start();
                button1.Text = "Stop"; 
            }
            else 
            {
                Timer.Stop();
                button1.Text = "Start";
            }
            
        }
    }
}
