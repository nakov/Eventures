using System;
using System.IO;
using Eventures.WebAPI.IntegrationTests;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace Eventures.Desktop.AppiumTests
{
    public class AppiumTests : AppiumTestsBase
    {
        private AppiumLocalService appiumLocalService;
        private string AppPath = @"../../../../Eventures.Desktop/bin/Debug/net5.0-windows/Eventures.Desktop.exe";
        private WindowsDriver<WindowsElement> driver;

        private string username = "newUser" + DateTime.UtcNow.Ticks;
        private string password = "newPassword12";
        private string eventBoardWindowName = "Event Board";
        private string createEventWindowName = "Create a New Event";
        private int columnsCount = 8;

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
            driver.Manage().Timeouts().ImplicitWait = 
                TimeSpan.FromSeconds(10);
        }

        [Test, Order(1)]
        public void Test_Connect_WithInvalidUrl()
        {
            var apiUrlField = driver
                .FindElementByAccessibilityId("textBoxApiUrl");
            var invalidPort = "1234";
            apiUrlField.SendKeys($"http://localhost:{invalidPort}/api");

            var connectBtn = driver
                .FindElementByAccessibilityId("buttonConnect");
            connectBtn.Click();

            Assert.That(driver.PageSource.Contains("Connect to Eventures API"));

            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");
            Assert.That(statusTextBox.Text.Contains("Error: HTTP error `No connection"));
        }

        [Test, Order(2)]
        public void Test_Connect_WithValidUrl()
        {
            var apiUrlField = driver
               .FindElementByAccessibilityId("textBoxApiUrl");
            apiUrlField.SendKeys(@$"{this.baseUrl}/api");

            var connectBtn = driver
                .FindElementByAccessibilityId("buttonConnect");
            connectBtn.Click();

            Assert.That(driver.PageSource.Contains(this.eventBoardWindowName));

            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");
            Assert.AreEqual("Connected to the Web API.", statusTextBox.Text);
        }

        [Test, Order(3)]
        public void Test_Reload_Unauthorized()
        {
            var reloadBtn = driver
                .FindElementByAccessibilityId("buttonReload");
            reloadBtn.Click();

            Assert.That(driver.PageSource.Contains(this.eventBoardWindowName));

            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");
            Assert.That(statusTextBox.Text.Contains("Error: HTTP error `Unauthorized`"));
        }

        [Test, Order(4)]
        public void Test_Register()
        {
            var registerBtn = driver
                .FindElementByAccessibilityId("buttonRegister");
            registerBtn.Click();

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
            passworField.SendKeys(this.password);

            var confirmPasswordField = driver
                .FindElementByAccessibilityId("textBoxConfirmPassword");
            confirmPasswordField.Clear();
            confirmPasswordField.SendKeys(this.password);

            var firstNameField = driver
                .FindElementByAccessibilityId("textBoxFirstName");
            firstNameField.Clear();
            firstNameField.SendKeys("Test");

            var lastNameField = driver
               .FindElementByAccessibilityId("textBoxLastName");
            lastNameField.Clear();
            lastNameField.SendKeys("User");

            var registerConfirmBtn = driver
                .FindElementByAccessibilityId("buttonRegisterConfirm");
            registerConfirmBtn.Click();

            Assert.That(driver.PageSource.Contains(this.eventBoardWindowName));

            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");
            Assert.That(statusTextBox.Text.Contains("Load successful:"));
        }

        [Test]
        public void Test_Login()
        {
            var loginBtn = driver
                .FindElementByAccessibilityId("buttonLogin");
            loginBtn.Click();

            var usernameField = driver
                 .FindElementByAccessibilityId("textBoxUsername");
            usernameField.Clear();
            usernameField.SendKeys(this.username);

            var passworField = driver
                .FindElementByAccessibilityId("textBoxPassword");
            passworField.Clear();
            passworField.SendKeys(this.password);

            var loginConfirmBtn = driver
                .FindElementByAccessibilityId("buttonLoginConfirm");
            loginConfirmBtn.Click();

            Assert.That(driver.PageSource.Contains(this.eventBoardWindowName));

            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");
            Assert.That(statusTextBox.Text.Contains("Load successful:"));
        }

        [Test]
        public void Test_Reload()
        {
            var reloadBtn = driver
                .FindElementByAccessibilityId("buttonReload");
            reloadBtn.Click();

            Assert.That(driver.PageSource.Contains(this.eventBoardWindowName));

            var eventsListSubItems = driver
                .FindElementsByXPath("/Window/List/ListItem/Text[starts-with(@AutomationId,\"ListViewSubItem-\")]");
            var eventsCount = eventsListSubItems.Count / this.columnsCount;

            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");
            Assert.AreEqual($"Load successful: {eventsCount} events loaded.", statusTextBox.Text);
        }

        [Test]
        public void Test_CreateEvent_ValidData()
        {
            var eventsListSubItems = driver
                .FindElementsByXPath("/Window/List/ListItem/Text[starts-with(@AutomationId,\"ListViewSubItem-\")]");
            var eventsCountBefore = eventsListSubItems.Count / this.columnsCount;

            var createBtn = driver
                .FindElementByAccessibilityId("buttonCreate");
            createBtn.Click();

            Assert.That(driver.PageSource.Contains(this.createEventWindowName));

            var eventName = "Fun Event" + DateTime.Now.Ticks;
            var nameField = driver
                .FindElementByAccessibilityId("textBoxName");
            nameField.Clear();
            nameField.SendKeys(eventName);

            var eventPlace = "Beach";
            var placeField = driver
                .FindElementByAccessibilityId("textBoxPlace");
            placeField.Clear();
            placeField.SendKeys(eventPlace);

            var upBtns = driver
                .FindElementsByName("Up");
            var priceUpBtn = upBtns[0];
            priceUpBtn.Click();
            priceUpBtn.Click();

            var ticketsUpBtn = upBtns[1];
            ticketsUpBtn.Click();

            var createConfirmationBtn = driver
                .FindElementByAccessibilityId("buttonCreateConfirm");
            createConfirmationBtn.Click();

            Assert.That(!driver.PageSource.Contains(this.createEventWindowName));
            Assert.That(driver.PageSource.Contains(this.eventBoardWindowName));
            Assert.That(driver.PageSource.Contains(eventName));
            Assert.That(driver.PageSource.Contains(eventPlace));
            Assert.That(driver.PageSource.Contains(this.username));

            eventsListSubItems = driver.FindElementsByXPath("/Window/List/ListItem/Text[starts-with(@AutomationId,\"ListViewSubItem-\")]");
            var eventsCountAfter = eventsListSubItems.Count / this.columnsCount;
            Assert.AreEqual(eventsCountBefore + 1, eventsCountAfter);

            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");
            Assert.That(statusTextBox.Text.Contains("Load successful:"));
        }

        [Test]
        public void Test_CreateEvent_InvalidData()
        {
            var eventsListSubItems = driver
                .FindElementsByXPath("/Window/List/ListItem/Text[starts-with(@AutomationId,\"ListViewSubItem-\")]");
            var eventsCountBefore = eventsListSubItems.Count / this.columnsCount;

            var createBtn = driver
                .FindElementByAccessibilityId("buttonCreate");
            createBtn.Click();

            Assert.That(driver.PageSource.Contains(this.createEventWindowName));

            var eventName = "Fun Event" + DateTime.Now.Ticks;
            var nameField = driver
                .FindElementByAccessibilityId("textBoxName");
            nameField.Clear();
            nameField.SendKeys(eventName);

            var invalidEventPlace = string.Empty;
            var placeField = driver
                .FindElementByAccessibilityId("textBoxPlace");
            placeField.Clear();
            placeField.SendKeys(invalidEventPlace);

            var createConfirmationBtn = driver
                .FindElementByAccessibilityId("buttonCreateConfirm");
            createConfirmationBtn.Click();

            Assert.That(!driver.PageSource.Contains(this.createEventWindowName));
            Assert.That(driver.PageSource.Contains(this.eventBoardWindowName));
            Assert.That(!driver.PageSource.Contains(eventName));

            eventsListSubItems = driver
                .FindElementsByXPath("/Window/List/ListItem/Text[starts-with(@AutomationId,\"ListViewSubItem-\")]");
            var eventsCountAfter = eventsListSubItems.Count / this.columnsCount;
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);

            var statusTextBox = driver.FindElementByXPath("/Window/StatusBar/Text");
            Assert.That(statusTextBox.Text.Contains("Error: HTTP error `BadRequest`."));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.CloseApp();
            driver.Quit();
            appiumLocalService.Dispose();
        }
    }
}