using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SnapshotDemo.Consts;

namespace SnapshotDemo
{
    /// <summary>
    /// 商品頁快照
    /// </summary>
    public class SalePageSnapshot
    {
        /// <summary>
        /// 執行快照
        /// </summary>
        /// <param name="snapshotUrl">The snapshot URL.</param>
        public void Do(string snapshotUrl)
        {
            using (var driver = this.GenerateChromeDriver(null))
            {
                WebDriverWait _wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
                driver.Navigate().GoToUrl(snapshotUrl);

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                _wait.Until(d => js.ExecuteScript("return document.readyState").ToString() == "complete");

                var targetElement = _wait.Until<IWebElement>(d => driver.FindElement(By.TagName("body")));
                driver.Manage().Window.Size = new Size(targetElement.Size.Width, targetElement.Size.Height);

                //// 拍照
                Screenshot screenShot = this.GetScreenshot(driver);

                //// 儲存照片
                screenShot.SaveAsFile("/var/task/YourSnapshotImage.png", ScreenshotImageFormat.Png);
            }
        }

        /// <summary>
        /// Gets the screenshot.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <returns>Screenshot</returns>
        private Screenshot GetScreenshot(IWebDriver driver)
        {
            return ((ITakesScreenshot)driver).GetScreenshot();
        }

        /// <summary>
        /// 產生Chrome Driver.
        /// </summary>
        /// <param name="options">設定檔</param>
        /// <returns>IWebDriver</returns>
        private IWebDriver GenerateChromeDriver(ChromeOptions options)
        {
            if (options == null)
            {
                options = new ChromeOptions();
            }

            options.AddArguments("no-sandbox", "headless", "disable-dev-shm-usage", "disable-gpu", "single-process", "no-zygote", "hide-scrollbars", "lang=zh-TW,zh", this.GetUserAgentDescription(true));

            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            options.BinaryLocation = PathConst.Chromium;

            var driver = new ChromeDriver(PathConst.ChromeDriverFolder, options);


            //// 設定頁面讀取逾時秒數
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 30);

            return driver;
        }

        /// <summary>
        /// Gets the user agent description.
        /// </summary>
        /// <returns>user agent description</returns>
        private string GetUserAgentDescription(bool isMobileWeb)
        {
            if (isMobileWeb)
            {
                //// Mobile 
                return "user-agent=Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1";
            }
            //// PC
            return "user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
        }
    }
}
