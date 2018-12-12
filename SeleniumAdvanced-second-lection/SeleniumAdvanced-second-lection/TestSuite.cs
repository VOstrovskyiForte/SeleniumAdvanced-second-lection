using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumAdvanced_second_lection
{
    [TestFixture]
    class TestSuite
    {
        private IWebDriver driver;
        IJavaScriptExecutor executor;


        [SetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();
            string downloadPath = Path.Combine(@"C:\", "SeleniumDownloads", DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
            options.AddUserProfilePreference("download.default_directory", downloadPath);
            //options.AddArgument("--start-fullscreen");
            driver = new ChromeDriver(options);
            
        }

        [Test]
        public void SeleniumAdvancedSimpleTest()
        {


            driver.Navigate().GoToUrl(@"https://unsplash.com/search/photos/test");

            executor = (IJavaScriptExecutor)driver;

            //var imagesList = driver.FindElement(By.Id("gridMulti"));

            executor.ScrollToBottomByFindingElement(driver);

            var photosDownloadButtons = driver.FindElements(By.XPath("//a[@title='Download photo']/span"));

            IWebElement lastPhotoDownloadButton = null;
            int lastPhotoDownloadButtonPosition = 0;

            foreach (IWebElement element in photosDownloadButtons)
            {
                if (element.Location.Y > lastPhotoDownloadButtonPosition)
                {
                    lastPhotoDownloadButton = element;
                    lastPhotoDownloadButtonPosition = element.Location.Y;
                }
            }

            if (lastPhotoDownloadButton != null)
                executor.ExecuteScript($"arguments[0].click()", lastPhotoDownloadButton);
            else
                throw new Exception("Last photo download button is not found");

        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }
    }
}
