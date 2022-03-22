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

    /// <summary>
    /// Đơn vị tính, 1000 VNĐ
    /// </summary>
    
    public enum ChipSelector
    { 
        //Set 1
        Five_K = 5,
        Ten_K = 10, 
        Twenty_K = 20, 
        Fifty_K = 50, 
        Hundred_K = 100, 
    
        //Set 2
        TwoHundred_K = 200,
        FiveHundred_K = 500,
        One_M = 1_000,
        Two_M = 2_000,
        Five_M = 5_000, //dùng cho cả set 2 và set 3

        Ten_M = 10_000, 
        Twenty_M = 20_000, 
        Fifty_M = 50_000, 
        Hundred_M = 100_000
    }

    


    public static class UIProcess
    {
        /// <summary>
        /// Luôn dùng main thread để login
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static bool LoginToTheSite(ChromiumDriver driver, string userName, string password)
        {
            if (driver == null)
                return false;

            var isSuccess = false;

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

                isSuccess = true;
            }
            catch (Exception ex)
            {
                LogService.LogError(ex.Message);
            }

            return isSuccess;
        }

        public static List<int> ChipValues { get; set; } = Enum.GetValues(typeof(ChipSelector)).Cast<int>()
                                                                .OrderByDescending(c => c)
                                                                .ToList();
        //Tối thiểu 30K
        public const int MinVal = 30; 
        public static string ChipsetClass(ChipSelector chip)
        {
            switch (chip)
            {
                case ChipSelector.Hundred_M:
                    return "ChipAreaItem_100M";
                case ChipSelector.Fifty_M:
                    return "ChipAreaItem_50M";
                case ChipSelector.Twenty_M:
                    return "ChipAreaItem_20M";
                case ChipSelector.Ten_M:
                    return "ChipAreaItem_10M";
                case ChipSelector.Five_M:
                    return "ChipAreaItem_5M";
                case ChipSelector.Two_M:
                    return "ChipAreaItem_2M";
                case ChipSelector.One_M:
                    return "ChipAreaItem_1M";
                case ChipSelector.FiveHundred_K:
                    return "ChipAreaItem_500K";
                case ChipSelector.TwoHundred_K:
                    return "ChipAreaItem_200K";
                case ChipSelector.Hundred_K:
                    return "ChipAreaItem_100K";
                case ChipSelector.Fifty_K:
                    return "ChipAreaItem_50K";
                case ChipSelector.Twenty_K:
                    return "ChipAreaItem_20K";
                case ChipSelector.Ten_K:
                    return "ChipAreaItem_10K";
                case ChipSelector.Five_K:
                default:
                    return "ChipAreaItem_5K"; 
            }
        }

        public static List<ChipSelector> SelectChips(int amount)
        {
            var list = new List<ChipSelector>();
            int index = 0; 
            while (amount > 0 && index < ChipValues.Count)
            {
                var chipValue = ChipValues[index];
                if (amount >= chipValue)
                {
                    amount -= chipValue;
                    list.Add((ChipSelector)chipValue);
                }
                else
                { 
                    index++;
                }
            }
            return list; 
        }

        public static void ClickOnChips(IWebDriver trader, int tableNum, int amount, string buttonID)
        { 
            var listChip = SelectChips(amount);

            if (listChip == null)
                return;
            //Tìm bàn N
            var querySelector = $"widget-game-baccarat[ng-reflect-table-code='010{ tableNum }']";
            var tableUI = trader.FindElement(By.CssSelector(querySelector));

            var buttonUI = tableUI.FindElement(By.Id(buttonID));
            var lastChip = ChipSelector.Five_K; 

            foreach (var chip in listChip)
            {
                if (chip == lastChip)
                {
                    buttonUI.Click();
                    System.Threading.Thread.Sleep(200);
                }
                else
                {
                    var chipUI = trader.FindElement(By.Id(ChipsetClass(chip)));
                    if (chipUI != null)
                    {
                        chipUI.Click();
                        System.Threading.Thread.Sleep(200);
                        buttonUI.Click();
                        System.Threading.Thread.Sleep(200);
                    }
                }
                
            }


        }
    }
}
