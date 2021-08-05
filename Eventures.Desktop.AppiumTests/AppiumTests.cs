using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using System.IO;

namespace Eventures.Desktop.AppiumTests
{
    public class AppiumTests
    {
        private AppiumLocalService appiumLocalService;
        //private const string AppiumServerUri = "http://[::1]:4723/wd/hub";
        private string AppPath = @"../../../../Eventures.Desktop/bin/Debug/net5.0-windows/Eventures.Desktop.exe";
        private WindowsDriver<WindowsElement> driver;

        private string username = "newUser";
        private string eventBoard = "Event Board";
        //private string createEvent = "Create New Event";

        [OneTimeSetUp]
        public void Setup()
        {
            appiumLocalService = new AppiumServiceBuilder()
                .UsingAnyFreePort()
                .Build();

            appiumLocalService.Start();
            var appiumOptions = new AppiumOptions() { PlatformName = "Windows" };
            var fullPathName = Path.GetFullPath(AppPath);
            appiumOptions.AddAdditionalCapability("app", fullPathName);
            driver = new WindowsDriver<WindowsElement>(
                appiumLocalService,
                appiumOptions);

            //Connect(driver);
        }

        [Test]
        public void Test_StatusBar()
        {
            var connectBtn = driver.FindElementByAccessibilityId("buttonConnect");
            connectBtn.Click();

            var statusTextBox = driver.FindElementByXPath(
                "/Window/StatusBar/Text");
            Assert.AreEqual("Connected to the Web API.", statusTextBox.Text);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.CloseApp();
            driver.Quit();
            appiumLocalService.Dispose();
        }

        private void Connect(WindowsDriver<WindowsElement> driver)
        {
            var apiUrlField = driver
                .FindElementByAccessibilityId("textBoxApiUrl");
            apiUrlField.Clear();
            apiUrlField.SendKeys("https://localhost:44359/api/");

            var usernameField = driver
                .FindElementByAccessibilityId("textBoxUsername");
            usernameField.Clear();
            usernameField.SendKeys(this.username);

            var emailField = driver
                .FindElementByAccessibilityId("textBoxEmail");
            emailField.Clear();
            emailField.SendKeys(this.username + "@mail.com");

            var passworField = driver
                .FindElementByAccessibilityId("textBoxPassword");
            passworField.Clear();
            passworField.SendKeys("newPassword");

            var confirmPasswordField = driver
                .FindElementByAccessibilityId("textBoxConfirmPassword");
            confirmPasswordField.Clear();
            confirmPasswordField.SendKeys("newPassword");

            var connectBtn = driver
                .FindElementByAccessibilityId("buttonConnect");
            connectBtn.Click();

            Assert.That(driver.PageSource.Contains(this.eventBoard));
        }
    }
}