using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Eventures.Desktop.AppiumTests
{
    public class AppiumTests
    {
        private const string AppiumServerUri = "http://[::1]:4723/wd/hub";
        private string AppPath = @"../../../../Eventures.Desktop/bin/Debug/net5.0-windows/Eventures.Desktop.exe";
        private WindowsDriver<WindowsElement> driver;

        private string username = "newUser";
        private string eventBoard = "Event Board";
        private string createEvent = "Create New Event";

        [OneTimeSetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions() { PlatformName = "Windows" };
            var fullPathName = Path.GetFullPath(AppPath);
            appiumOptions.AddAdditionalCapability("app", fullPathName);
            driver = new WindowsDriver<WindowsElement>(
                new Uri(AppiumServerUri), 
                appiumOptions);
            //Connect(driver);
        }

        [Test]
        public void Test_StatusBar()
        {
            var connectBtn = driver.FindElementByAccessibilityId("buttonConnect");
            connectBtn.Click();

            // Switch to the new active window (which have just changed)
            var activeWindow = driver.WindowHandles[0];
            driver.SwitchTo().Window(activeWindow);

            var statusTextBox = driver.FindElementByXPath(
                "/Window/StatusBar/Text");
            string text = statusTextBox.Text;
            Debug.WriteLine(text);
            string text2 = statusTextBox.TagName;
            Debug.WriteLine(text2);
        }

        [Test]
        public void Test_CreateEvent_ValidData()
        {
            var createBtn = driver.FindElementByAccessibilityId("buttonCreate");
            createBtn.Click();

            Assert.That(driver.PageSource.Contains(this.createEvent));

            var eventName = "Fun Event" + DateTime.Now.Ticks;
            var nameField = driver.FindElementByAccessibilityId("textBoxName");
            nameField.Clear();
            nameField.SendKeys(eventName);

            var eventPlace = "Beach";
            var placeField = driver.FindElementByAccessibilityId("textBoxPlace");
            placeField.Clear();
            placeField.SendKeys(eventPlace);

            var upBtns = driver.FindElementsByName("Up");
            var priceUpBtn = upBtns[0];
            priceUpBtn.Click();
            priceUpBtn.Click();

            var ticketsUpBtn = upBtns[1];
            ticketsUpBtn.Click();

            var createConfirmationBtn = driver
                .FindElementByAccessibilityId("buttonCreateConfirm");
            createConfirmationBtn.Click();

            Assert.That(!driver.PageSource.Contains(this.createEvent));
            Assert.That(driver.PageSource.Contains(this.eventBoard));
            Assert.That(driver.PageSource.Contains(eventName));
            Assert.That(driver.PageSource.Contains(eventPlace));
            Assert.That(driver.PageSource.Contains(this.username));
        }

        [Test]
        public void Test_CreateEvent_InvalidData()
        {
            var createBtn = driver.FindElementByAccessibilityId("buttonCreate");
            createBtn.Click();

            Assert.That(driver.PageSource.Contains(this.createEvent));

            var eventName = "Fun Event" + DateTime.Now.Ticks;
            var nameField = driver.FindElementByAccessibilityId("textBoxName");
            nameField.Clear();
            nameField.SendKeys(eventName);

            var invalidEventPlace = string.Empty;
            var placeField = driver.FindElementByAccessibilityId("textBoxPlace");
            placeField.Clear();
            placeField.SendKeys(invalidEventPlace);

            var createConfirmationBtn = driver.FindElementByAccessibilityId("buttonCreateConfirm");
            createConfirmationBtn.Click();

            Assert.That(!driver.PageSource.Contains(this.createEvent));
            Assert.That(driver.PageSource.Contains(this.eventBoard));
            Assert.That(!driver.PageSource.Contains(eventName));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
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