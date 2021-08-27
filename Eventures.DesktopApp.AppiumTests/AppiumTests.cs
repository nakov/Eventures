using System;
using System.Linq;
using System.Threading;
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

        [Test, Order(8)]
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


            // TODO: debug
            Console.WriteLine("Page source before [Create] btn click:\r\n" + driver.PageSource);

            // Click on the [Create] button under the "Create" form
            var createConfirmationBtn = driver.FindElementByAccessibilityId("buttonCreateConfirm");
            createConfirmationBtn.Click();

            // TODO: debug
            Console.WriteLine("Page source after [Create] btn click:\r\n" + driver.PageSource);

            Thread.Sleep(3000);

            // TODO: debug
            Console.WriteLine("Page source after 3000 ms click:\r\n" + driver.PageSource);


            // Assert a success message is displayed in the status bar
            var loadSuccessfulMsgAppered = 
                this.wait.Until(s => driver.FindElementByXPath("/Window/StatusBar/Text")
                    .Text.Contains($"Load successful: {eventsCountBefore+1} events loaded"));
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

            // Assert the events count increased by 1
            var eventsCountAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsCountBefore + 1, eventsCountAfter);
        }

        [Test, Order(9)]
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