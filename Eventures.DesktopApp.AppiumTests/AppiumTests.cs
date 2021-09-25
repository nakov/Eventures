using System;
using System.Linq;

using NUnit.Framework;

namespace Eventures.DesktopApp.AppiumTests
{
    public class AppiumTests : AppiumTestsBase
    {
        private string username = "user" + DateTime.UtcNow.Ticks.ToString().Substring(10);
        private string password = "pass" + DateTime.UtcNow.Ticks.ToString().Substring(10);
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

            // Assert an error message is displayed in the status box
            var statusTextBox = driver
                .FindElementByXPath("/Window/StatusBar/Text");
            Assert.That(statusTextBox.Text.Contains("Error: Value cannot be null."));
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
        public void Test_Register()
        {
            // Assert the [Create] and [Reload] buttons are disabled
            var createBtn = driver.FindElementByAccessibilityId("buttonCreate");
            Assert.IsFalse(createBtn.Enabled);
            var reloadBtn = driver.FindElementByAccessibilityId("buttonReload");
            Assert.IsFalse(reloadBtn.Enabled);

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

            // Assert the [Create] and [Reload] buttons are enabled
            Assert.IsTrue(createBtn.Enabled);
            Assert.IsTrue(reloadBtn.Enabled);
        }

        [Test]
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

            // Assert the [Create] and [Reload] buttons are enabled
            var createBtn = driver.FindElementByAccessibilityId("buttonCreate");
            Assert.IsTrue(createBtn.Enabled);
            var reloadBtn = driver.FindElementByAccessibilityId("buttonReload");
            Assert.IsTrue(reloadBtn.Enabled);
        }

        [Test]
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

        [Test]
        public void Test_CreateEvent_ValidData()
        {
            // Get the events count before
            var eventsCountBefore = this.dbContext.Events.Count();

            // Locate and click on the [Create] button
            var createBtn = driver.FindElementByAccessibilityId("buttonCreate");
            createBtn.Click();

            // Assert the "Create a New Event" windows appears
            Assert.That(driver.PageSource.Contains(AppiumTests.CreateEventWindowName));

            // Fill in valid event data in the fields
            var eventName = "Fun Event" + DateTime.Now.Ticks;
            var nameField = driver.FindElementByAccessibilityId("textBoxName");
            nameField.Clear();
            nameField.SendKeys(eventName);

            var eventPlace = "Beach";
            var placeField = driver.FindElementByAccessibilityId("textBoxPlace");
            placeField.Clear();
            placeField.SendKeys(eventPlace);

            // Locate the up arrow buttons
            var upBtns = driver.FindElementsByName("Up");

            // Click the second up arrow button to increase the event tickets field value
            var ticketsUpBtn = upBtns[1];
            ticketsUpBtn.Click();

            // Click the first up arrow button to increase the event price field value
            var priceUpBtn = upBtns[0];
            priceUpBtn.Click();
            priceUpBtn.Click();

            // Click on the [Create] button under the "Create" form
            var createConfirmationBtn = driver.FindElementByAccessibilityId("buttonCreateConfirm");
            createConfirmationBtn.Click();

            // Wait until in the database the events count is increased by 1
            this.wait.Until(
                s => this.testDb.CreateDbContext().Events.Count() == eventsCountBefore + 1);

            // Assert a success message is displayed in the status bar
            var loadSuccessfulMsgAppered =
                this.wait.Until(s => driver.FindElementByXPath("/Window/StatusBar/Text")
                    .Text.Contains($"Load successful: {eventsCountBefore + 1} events loaded"));
            Assert.IsTrue(loadSuccessfulMsgAppered);

            // Assert the "Create a New Event" windows disappears
            string pageSource = driver.PageSource;
            Assert.That(!pageSource.Contains(AppiumTests.CreateEventWindowName));

            // Assert the "Event Board" windows appears
            Assert.That(pageSource.Contains(AppiumTests.EventBoardWindowName));

            // Assert the new event is displayed correctly
            Assert.That(pageSource.Contains(eventName));
            Assert.That(pageSource.Contains(eventPlace));
            Assert.That(pageSource.Contains(this.username));
        }

        [Test]
        public void Test_CreateEvent_InvalidData()
        {
            // Get the events count before
            var eventsCountBefore = this.dbContext.Events.Count();

            // Locate and click on the [Create] button
            var createBtn = driver.FindElementByAccessibilityId("buttonCreate");
            createBtn.Click();

            // Assert the "Create a New Event" windows appears
            Assert.That(driver.PageSource.Contains(AppiumTests.CreateEventWindowName));

            // Fill in valid event name
            var eventName = "Fun Event" + DateTime.Now.Ticks;
            var nameField = driver
                .FindElementByAccessibilityId("textBoxName");
            nameField.Clear();
            nameField.SendKeys(eventName);

            // Fill in invalid event place, e.g. empty string
            var invalidPlace = string.Empty;
            var placeField = driver.FindElementByAccessibilityId("textBoxPlace");
            placeField.Clear();
            placeField.SendKeys(invalidPlace);

            // Click on the [Create] button under the "Create" form
            var createConfirmationBtn = driver
                .FindElementByAccessibilityId("buttonCreateConfirm");
            createConfirmationBtn.Click();

            // Assert an error message is displayed in the status bar
            var statusTextBox = driver.FindElementByXPath("/Window/StatusBar/Text");
            var errorMessageAppears = this.wait.Until(
                s => statusTextBox.Text.Contains("Error"));
            Assert.IsTrue(errorMessageAppears);

            // Assert the "Create a New Event" windows disappears
            Assert.That(!driver.PageSource.Contains(AppiumTests.CreateEventWindowName));

            // Assert the "Event Board" windows appears
            Assert.That(driver.PageSource.Contains(AppiumTests.EventBoardWindowName));

            // Assert the page doesn't contain the new event
            Assert.That(!driver.PageSource.Contains(eventName));

            // Assert the events count is not increased
            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore, eventsCountAfter);
        }
    }
}