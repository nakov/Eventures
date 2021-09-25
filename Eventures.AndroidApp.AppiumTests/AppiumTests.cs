using System;
using System.Linq;

using NUnit.Framework;

namespace Eventures.AndroidApp.AppiumTests
{
    public class AppiumTests : AppiumTestsBase
    {
        private string username = "user" + DateTime.UtcNow.Ticks.ToString().Substring(10);
        private string password = "pass" + DateTime.UtcNow.Ticks.ToString().Substring(10);
        private const string ButtonConnectId = "buttonConnect";
        private const string ButtonLoginId = "buttonLogin";
        private const string ButtonRegisterId = "buttonRegister";
        private const string ButtonAddId = "buttonAdd";
        private const string ButtonReloadId = "buttonReload";
        private const string StatusTextBoxId = "textViewStatus";

        [Test, Order(1)]
        public void Test_Connect_WithInvalidUrl()
        {
            // Assert that [Login] and [Register] buttons are disabled
            var loginBtn = driver.FindElementById(ButtonLoginId);
            Assert.That(loginBtn.Enabled == false);

            var registerBtn = driver.FindElementById(ButtonRegisterId);
            Assert.That(registerBtn.Enabled == false);

            // Locate and click on the [Connect] button
            var connectBtn = driver.FindElementById(ButtonConnectId);
            connectBtn.Click();

            // Locate the "API URL" field
            var apiUrlField = driver.FindElementById("editTextApiUrl");
            apiUrlField.Clear();

            // Type in an invalid URL, e.g. with invalid port number (1234)
            var invalidUrl = "http://10.0.2.2:1234/api/";
            apiUrlField.SendKeys(invalidUrl);

            // Click on the [Connect] button under the "Connect" form
            var confirmConnectBtn = driver.FindElementById("buttonConfirmConnect");
            confirmConnectBtn.Click();

            // Locate the status box
            var statusTextBox = driver.FindElementById(StatusTextBoxId);

            // Wait until the server finishes connecting
            // and assert that an error message appears
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Equals("Could not connect. Try again."));

            Assert.IsTrue(messageAppears);

            // Assert the [Login] and [Register] buttons are still disabled
            Assert.That(loginBtn.Enabled == false);
            Assert.That(registerBtn.Enabled == false);
        }

        [Test, Order(2)]
        public void Test_Connect_WithValidUrl()
        {
            // Assert that [Login] and [Register] buttons are disabled
            var loginBtn = driver.FindElementById(ButtonLoginId);
            Assert.That(loginBtn.Enabled == false);

            var registerBtn = driver.FindElementById(ButtonRegisterId);
            Assert.That(registerBtn.Enabled == false);

            // Locate and click on the [Connect] button
            var connectBtn = driver.FindElementById(ButtonConnectId);
            connectBtn.Click();

            // Locate the "API URL" field and send valid URL from the local server
            var apiUrlField = driver.FindElementById("editTextApiUrl");
            apiUrlField.Clear();
            apiUrlField.SendKeys(@$"{this.baseUrl}/api/");

            // Click on the [Connect] button under the "Connect" form
            var confirmConnectBtn = driver.FindElementById("buttonConfirmConnect");
            confirmConnectBtn.Click();

            // Locate the status box
            var statusTextBox = driver.FindElementById(StatusTextBoxId);

            // Wait until the server finishes connecting
            // and assert that a success message appears
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Equals("Connected successfully."));

            Assert.IsTrue(messageAppears);

            // Assert the [Login] and [Register] buttons are enabled
            Assert.That(loginBtn.Enabled == true);
            Assert.That(registerBtn.Enabled == true);
        }

        [Test, Order(3)]
        public void Test_Register()
        {
            // Assert the [Add] and [Reload] buttons are disabled
            var addBtn = driver.FindElementById(ButtonAddId);
            Assert.That(addBtn.Enabled == false);

            var reloadBtn = driver.FindElementById(ButtonReloadId);
            Assert.That(reloadBtn.Enabled == false);

            // Locate and click on the [Register] button
            var registerBtn = driver.FindElementById(ButtonRegisterId);
            Assert.That(registerBtn.Enabled == true);
            registerBtn.Click();

            // Locate fields and fill them in with valid user data
            var usernameField = driver.FindElementById("editTextUsername");
            usernameField.Clear();
            usernameField.SendKeys(this.username);

            var emailField = driver.FindElementById("editTextEmail");
            emailField.Clear();
            emailField.SendKeys(this.username + "@mail.com");

            var passwordField = driver.FindElementById("editTextPassword");
            passwordField.Clear();
            passwordField.SendKeys(this.password);

            var confirmPasswordField = driver
                .FindElementById("editTextConfirmPassword");
            confirmPasswordField.Clear();
            confirmPasswordField.SendKeys(this.password);

            var firstNameField = driver.FindElementById("editTextFirstName");
            firstNameField.Clear();
            firstNameField.SendKeys("Test");

            var lastNameField = driver.FindElementById("editTextLastName");
            lastNameField.Clear();
            lastNameField.SendKeys("User");

            // Click on the [Register] button under the "Register" form
            var confirmRegisterBtn = driver.FindElementById("buttonConfirmRegister");
            confirmRegisterBtn.Click();

            // Locate the status box
            var statusTextBox = driver.FindElementById(StatusTextBoxId);

            // Wait until the server finishes authorizing
            // and assert that events are displayed
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains($"Events found:"));

            Assert.IsTrue(messageAppears);

            // Assert the displayed events count is correct
            var eventsInDb = this.dbContext.Events.Count();
            Assert.AreEqual($"Events found: {eventsInDb}", statusTextBox.Text);

            // Assert the [Add] and [Reload] buttons are enabled
            Assert.That(addBtn.Enabled == true);
            Assert.That(reloadBtn.Enabled == true);
        }

        [Test]
        public void Test_Login()
        {
            // Locate and click on the [Login] button
            var loginBtn = driver.FindElementById(ButtonLoginId);
            Assert.That(loginBtn.Enabled == true);
            loginBtn.Click();

            // Locate fields and fill them in with valid user data
            var usernameField = driver.FindElementById("editTextUsername");
            usernameField.Clear();
            usernameField.SendKeys(this.username);

            var passwordField = driver.FindElementById("editTextPassword");
            passwordField.Clear();
            passwordField.SendKeys(this.password);

            // Click on the [Login] button under the "Login" form
            var confirmLoginBtn = driver.FindElementById("buttonConfirmLogin");
            confirmLoginBtn.Click();

            // Locate the status box
            var statusTextBox = driver.FindElementById(StatusTextBoxId);

            // Wait until the server finishes authorizing
            // and assert that events are displayed
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains($"Events found:"));

            Assert.IsTrue(messageAppears);

            // Assert the displayed events count is correct
            var eventsInDb = this.dbContext.Events.Count();
            Assert.AreEqual($"Events found: {eventsInDb}", statusTextBox.Text);

            // Assert the [Add] and [Reload] buttons are enabled
            var addBtn = driver.FindElementById(ButtonAddId);
            Assert.That(addBtn.Enabled == true);

            var reloadBtn = driver.FindElementById(ButtonReloadId);
            Assert.That(reloadBtn.Enabled == true);
        }

        [Test]
        public void Test_Reload()
        {
            // Locate and click on the [Reload] button
            var reloadBtn = driver.FindElementById(ButtonReloadId);
            Assert.That(reloadBtn.Enabled == true);
            reloadBtn.Click();

            // Locate the status box
            var statusTextBox = driver.FindElementById(StatusTextBoxId);

            // Wait until the server finishes authorizing
            // and assert that events are displayed
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains($"Events found:"));

            Assert.IsTrue(messageAppears);

            // Assert the displayed events count is correct
            var eventsInDb = this.dbContext.Events.Count();
            Assert.AreEqual($"Events found: {eventsInDb}", statusTextBox.Text);
        }

        [Test]
        public void Test_CreateEvent_WithValidData()
        {
            // Get the current events count in the db
            var eventsInDbBefore = this.dbContext.Events.Count();

            // Locate and click on the [Add] button
            var addBtn = driver.FindElementById(ButtonAddId);
            Assert.That(addBtn.Enabled == true);
            addBtn.Click();

            // Locate fields and fill them in with valid event data
            var nameField = driver.FindElementById("editTextName");
            nameField.Clear();
            nameField.SendKeys("Fun Event" + DateTime.Now.Ticks);

            var placeField = driver.FindElementById("editTextPlace");
            placeField.Clear();
            placeField.SendKeys("Beach");

            // Locate and click on the "Start Date" field
            var startDateField = driver.FindElementById("editTextStartDate");
            startDateField.Click();

            // On the datepicker window, locate and click on the [Ok] button
            var okBtnXPath = "//android.widget.ScrollView/android.widget.LinearLayout/android.widget.Button[2]";
            var okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            var startTimeField = driver.FindElementById("editTextStartTime");
            startTimeField.Click();
            okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            // Locate and click on the "End Date" field
            var endDateField = driver.FindElementById("editTextEndDate");
            endDateField.Click();

            // Locate and click on the next month's button
            var nextMonthBtn = driver.FindElementByXPath("//android.widget.NumberPicker[1]/android.widget.Button[2]");
            nextMonthBtn.Click();

            // Locate and click on the [Ok] button
            okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            var endTimeField = driver.FindElementById("editTextEndTime");
            endTimeField.Click();
            okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            var ticketsField = driver.FindElementById("editTextTickets");
            ticketsField.Clear();
            ticketsField.SendKeys("50");

            var priceField = driver.FindElementById("editTextPrice");
            priceField.Clear();
            priceField.SendKeys("10.50");

            // Locate and click on the [Create] button under the "Create" form
            var createBtn = driver.FindElementById("buttonCreate");
            createBtn.Click();

            // Locate the status box
            var statusTextBox = driver.FindElementById(StatusTextBoxId);

            // Wait until the server finishes creating the event
            // and assert that events are displayed
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Contains($"Events found: "));

            Assert.IsTrue(messageAppears);

            // Assert the events count is the db is increased
            var eventsInDbAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsInDbBefore + 1, eventsInDbAfter);
        }

        [Test]
        public void Test_CreateEvent_WithInvalidData()
        {
            // Get the current events count in the db
            var eventsInDbBefore = this.dbContext.Events.Count();

            // Locate and click on the [Add] button
            var addBtn = driver.FindElementById(ButtonAddId);
            Assert.That(addBtn.Enabled == true);
            addBtn.Click();

            // Locate fields and fill them in with valid event data
            // but leave the "End Date" field empty
            // (thus, the whole event will be invalid)
            var nameField = driver.FindElementById("editTextName");
            nameField.Clear();
            nameField.SendKeys("Fun Event" + DateTime.Now.Ticks);

            var placeField = driver.FindElementById("editTextPlace");
            placeField.Clear();
            placeField.SendKeys("Beach");

            var startDateField = driver.FindElementById("editTextStartDate");
            startDateField.Click();
            var okBtnXPath = "//android.widget.ScrollView/android.widget.LinearLayout/android.widget.Button[2]";
            var okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            var startTimeField = driver.FindElementById("editTextStartTime");
            startTimeField.Click();
            okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            var endTimeField = driver.FindElementById("editTextEndTime");
            endTimeField.Click();
            okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            var ticketsField = driver.FindElementById("editTextTickets");
            ticketsField.Clear();
            ticketsField.SendKeys("50");

            var priceField = driver.FindElementById("editTextPrice");
            priceField.Clear();
            priceField.SendKeys("10.50");

            // Locate and click on the [Create] button under the "Create" form
            var createBtn = driver.FindElementById("buttonCreate");
            createBtn.Click();

            // Locate the status box
            var statusTextBox = driver.FindElementById(StatusTextBoxId);

            // Wait until the server finishes trying to create the event
            // and assert that an error message is displayed
            var messageAppears = this.wait
                .Until(s => statusTextBox.Text.Equals("Could not create the new event. Try again."));

            Assert.IsTrue(messageAppears);

            // Assert the events count is the db is not changed
            var eventsInDbAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsInDbBefore, eventsInDbAfter);
        }
    }
}
