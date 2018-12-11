using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumAdvanced_second_lection
{
    [TestFixture]
    class TestSuite
    {
        private IWebDriver driver;
        private IJavaScriptExecutor executor;


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

            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;

            var imagesList = driver.FindElement(By.Id("gridMulti"));           

            bool isBottomOfPage = false;

            while (!isBottomOfPage)
            {
                long prevPosition = (long)executor.ExecuteScript("return arguments[0].scrollTop", imagesList);
                executor.ExecuteScript("arguments[0].scrollIntoView()", imagesList);
                long currentPosition = (long)executor.ExecuteScript("return arguments[0].scrollTop", imagesList);
                if (prevPosition == currentPosition)
                    isBottomOfPage = true;
            }

            var photosDownloadButtons = driver.FindElements(By.XPath("//a[@title='Download photo']//span"));

            IWebElement lastPhotoDownloadButton = null;

            foreach(IWebElement element in photosDownloadButtons)
            {
                if (element.Displayed)
                {
                    lastPhotoDownloadButton = element;
                    break;
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
