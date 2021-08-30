using System;
using System.Linq;
using NUnit.Framework;

namespace Eventures.DesktopApp.AppiumTests
{
    public class AppiumTests : AppiumTestsBase
    {
        private string username = "newUser" + DateTime.UtcNow.Ticks;
        private string password = "newPassword12";
        private const string EventBoardWindowName = "Event Board";
        private const string CreateEventWindowName = "Create a New Event";

        [Test, Order(1)]
        public void Test_Connect_WithEmptyUrl()
        {
            // Locate the URL field, clear it and leave it with an empty URL
            var apiUrlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            apiUrlField.Clear();

            // Locate and click on the [Connect] button
            var connectBtn = driver.FindElementByAccessibilityId("buttonConnect");
            connectBtn.Click();

            // Assert the "Connect to Eventures API" window appeared again
            Assert.That(driver.PageSource.Contains("Connect to Eventures API"));

            // Wait for the driver loads the message
            // and assert an error message is displayed in the status box
            var statusTextBox = driver
               .FindElementByXPath("/Window/StatusBar/Text");

            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains("Error: Value cannot be null."));

            Assert.IsTrue(messageAppears);
        }

        [Test, Order(2)]
        public void Test_Connect_WithInvalidUrl()
        {
            // Locate the URL field and
            // send a URL with invalid port, e.g. invalid URL
            var apiUrlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            apiUrlField.Clear();
            var invalidPort = "1234";
            apiUrlField.SendKeys($"http://localhost:{invalidPort}/api");

            // Locate and click on the [Connect] button
            var connectBtn = driver.FindElementByAccessibilityId("buttonConnect");
            connectBtn.Click();

            // Assert the "Connect to Eventures API" window appeared again
            Assert.That(driver.PageSource.Contains("Connect to Eventures API"));

            // Assert an error message is displayed in the status box
            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");

            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains("Error: HTTP error `No connection"));

            Assert.IsTrue(messageAppears);
        }

        [Test, Order(3)]
        public void Test_Connect_WithValidUrl()
        {
            // Locate the URL field
            var apiUrlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            apiUrlField.Clear();

            // Send a valid URL- get the "baseUrl" from the "AppiumTestsBase" base class
            apiUrlField.SendKeys(@$"{this.baseUrl}/api");

            // Locate and click on the [Connect] button
            var connectBtn = driver.FindElementByAccessibilityId("buttonConnect");
            connectBtn.Click();

            // Assert the "Event Board" window appeared again
            Assert.That(driver.PageSource.Contains(AppiumTests.EventBoardWindowName));

            // Assert a sucess message is displayed in the status box
            var statusTextBox = driver.FindElementByXPath("/Window/StatusBar/Text");
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Equals("Connected to the Web API."));

            Assert.IsTrue(messageAppears);
        }

        [Test, Order(4)]
        public void Test_Reload_Unauthorized()
        {
            // Locate and click on the [Reload] button
            var reloadBtn = driver.FindElementByAccessibilityId("buttonReload");
            reloadBtn.Click();

            // Assert the "Event Board" window is displayed
            Assert.That(driver.PageSource.Contains(AppiumTests.EventBoardWindowName));

            // Assert an error message is displayed in the status box
            // as the current user is not logged-in
            var statusTextBox = driver.FindElementByXPath("/Window/StatusBar/Text");
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains("Error: HTTP error `Unauthorized`"));

            Assert.IsTrue(messageAppears);
        }

        [Test, Order(5)]
        public void Test_Register()
        {
            // Locate and click on the [Register] button
            var registerBtn = driver.FindElementByAccessibilityId("buttonRegister");
            registerBtn.Click();

            // Fill in valid data in the fields
            var usernameField = driver.FindElementByAccessibilityId("textBoxUsername");
            usernameField.Clear();
            usernameField.SendKeys(this.username);

            var emailField = driver.FindElementByAccessibilityId("textBoxEmail");
            emailField.Clear();
            emailField.SendKeys(this.username + "@mail.com");

            var passworField = driver.FindElementByAccessibilityId("textBoxPassword");
            passworField.Clear();
            passworField.SendKeys(this.password);

            var confirmPasswordField = driver
                .FindElementByAccessibilityId("textBoxConfirmPassword");
            confirmPasswordField.Clear();
            confirmPasswordField.SendKeys(this.password);

            var firstNameField = driver.FindElementByAccessibilityId("textBoxFirstName");
            firstNameField.Clear();
            firstNameField.SendKeys("Test");

            var lastNameField = driver.FindElementByAccessibilityId("textBoxLastName");
            lastNameField.Clear();
            lastNameField.SendKeys("User");

            // Click on the [Register] button under the "Register" form
            var registerConfirmBtn = driver.FindElementByAccessibilityId("buttonRegisterConfirm");
            registerConfirmBtn.Click();

            // Assert the "Event Board" windows appears
            Assert.That(driver.PageSource.Contains(AppiumTests.EventBoardWindowName));

            // Wait until the events are loaded
            var statusTextBox = driver.FindElementByXPath("/Window/StatusBar/Text");

            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains("Load successful"));

            Assert.IsTrue(messageAppears);

            // Get the events count from the database
            var eventsInDb = this.dbContext.Events.Count();

            // Assert a success message is displayed in the status box
            Assert.AreEqual($"Load successful: {eventsInDb} events loaded.", statusTextBox.Text);
        }

        [Test, Order(6)]
        public void Test_Login()
        {
            // Locate and click on the [Login] button
            var loginBtn = driver.FindElementByAccessibilityId("buttonLogin");
            loginBtn.Click();

            // Fill in valid data in the fields
            var usernameField = driver.FindElementByAccessibilityId("textBoxUsername");
            usernameField.Clear();
            usernameField.SendKeys(this.username);

            var passworField = driver.FindElementByAccessibilityId("textBoxPassword");
            passworField.Clear();
            passworField.SendKeys(this.password);

            // Click on the [Login] button under the "Login" form
            var loginConfirmBtn = driver.FindElementByAccessibilityId("buttonLoginConfirm");
            loginConfirmBtn.Click();

            // Assert the "Event Board" windows appears
            Assert.That(driver.PageSource.Contains(AppiumTests.EventBoardWindowName));

            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");

            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains("Load successful"));

            Assert.IsTrue(messageAppears);

            // Get the events count from the database
            var eventsInDb = this.dbContext.Events.Count();

            // Assert a success message is displayed in the status box
            Assert.AreEqual($"Load successful: {eventsInDb} events loaded.", statusTextBox.Text);
        }

        [Test, Order(7)]
        public void Test_Reload()
        {
            // Locate and click on the [Reload] button
            var reloadBtn = driver.FindElementByAccessibilityId("buttonReload");
            reloadBtn.Click();

            // Assert the "Event Board" windows appears
            Assert.That(driver.PageSource.Contains(AppiumTests.EventBoardWindowName));

            // Assert a success message is displayed 
            var statusTextBox = driver.FindElementByXPath("/Window/StatusBar/Text");
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains("Load successful"));

            Assert.IsTrue(messageAppears);

            // Get the events count from the db
            var eventsInDb = this.dbContext.Events.Count();
            Assert.AreEqual($"Load successful: {eventsInDb} events loaded.", statusTextBox.Text);
        }
    }
}