using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using Midas.Utils;

namespace Midas.Automation
{
    public static class UIProcess
    {
        /// <summary>
        /// Luôn dùng main thread để login
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void LoginToTheSite(ChromiumDriver driver, string userName, string password)
        {
            if (driver == null)
                return;

            try
            {
                driver.Navigate().GoToUrl("https://www.jbbodds.com/vi-vn");
                System.Threading.Thread.Sleep(5000);

                driver.FindElement(By.CssSelector(".input-username input[name=username]")).SendKeys(userName);
                driver.FindElement(By.CssSelector(".input-password input[name=password]")).SendKeys(password);
                driver.FindElement(By.CssSelector("button[type=submit]")).Click();
                System.Threading.Thread.Sleep(500); //Đợi nửa giây  

                var liveCasino = driver.FindElements(By.CssSelector("#menu-products > li > a"))[2];
                liveCasino.Click();
                System.Threading.Thread.Sleep(5000); //Đợi 5 giây 

                var k9 = driver.FindElements(By.CssSelector(".casino-list li"))[0];
                Actions actions = new Actions(driver);
                actions.MoveToElement(k9).Perform(); //Đưa chuột lên phần K9
                System.Threading.Thread.Sleep(2000); //Đợi 2 giây

                var playNowGrandSuite = driver.FindElements(By.CssSelector(".game-popup a.btn-orange"))[0];
                playNowGrandSuite.Click();
                System.Threading.Thread.Sleep(1000);//Đợi 2 giây cho xuất hiện nút OK

                var okButton = driver.FindElement(By.CssSelector(".modal-dialog .text-center button.bet-btn"));
                okButton.Click(); //Nhấn nút OK
                System.Threading.Thread.Sleep(5000);//Đợi 5 giây để màn hình mới load
            }
            catch (Exception ex)
            {
                LogService.LogError(ex.Message);
            }

            
        }
    }
}
