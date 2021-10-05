using System;
using System.Linq;

using NUnit.Framework;
using SeleniumExtras.WaitHelpers;

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
            Assert.IsFalse(loginBtn.Enabled);

            var registerBtn = driver.FindElementById(ButtonRegisterId);
            Assert.IsFalse(registerBtn.Enabled);

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
            Assert.IsFalse(loginBtn.Enabled);
            Assert.IsFalse(registerBtn.Enabled);
        }

        [Test, Order(2)]
        public void Test_Connect_WithValidUrl()
        {
            // Assert that [Login] and [Register] buttons are disabled
            var loginBtn = driver.FindElementById(ButtonLoginId);
            Assert.IsFalse(loginBtn.Enabled);

            var registerBtn = driver.FindElementById(ButtonRegisterId);
            Assert.IsFalse(registerBtn.Enabled);

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
            Assert.IsTrue(loginBtn.Enabled);
            Assert.IsTrue(registerBtn.Enabled);
        }

        [Test, Order(3)]
        public void Test_Register()
        {
            // Assert the [Add] and [Reload] buttons are disabled
            var addBtn = driver.FindElementById(ButtonAddId);
            Assert.IsFalse(addBtn.Enabled);

            var reloadBtn = driver.FindElementById(ButtonReloadId);
            Assert.IsFalse(reloadBtn.Enabled);

            // Locate and click on the [Register] button
            var registerBtn = driver.FindElementById(ButtonRegisterId);
            Assert.IsTrue(registerBtn.Enabled);
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
            Assert.IsTrue(addBtn.Enabled);
            Assert.IsTrue(reloadBtn.Enabled);
        }

        [Test]
        public void Test_Login()
        {
            // Locate and click on the [Login] button
            var loginBtn = driver.FindElementById(ButtonLoginId);
            Assert.IsTrue(loginBtn.Enabled);
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
            Assert.IsTrue(addBtn.Enabled);

            var reloadBtn = driver.FindElementById(ButtonReloadId);
            Assert.IsTrue(reloadBtn.Enabled);
        }

        [Test]
        public void Test_Reload()
        {
            // Locate and click on the [Reload] button
            var reloadBtn = driver.FindElementById(ButtonReloadId);
            Assert.IsTrue(reloadBtn.Enabled);
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
        public void Test_CreateEvent()
        {
            // Get the current events count in the db
            var eventsInDbBefore = this.dbContext.Events.Count();

            // Locate and click on the [Add] button
            var addBtn = driver.FindElementById(ButtonAddId);
            Assert.IsTrue(addBtn.Enabled);
            addBtn.Click();

            // Locate fields
            // Fill in a valid event name
            var nameField = driver.FindElementById("editTextName");
            nameField.Clear();
            nameField.SendKeys("Fun Event" + DateTime.Now.Ticks);

            // Fill in an invalid event place 
            var placeField = driver.FindElementById("editTextPlace");
            placeField.Clear();
            placeField.SendKeys(string.Empty);

            // Locate and click on the start date field
            var startDateField = driver.FindElementById("editTextStartDate");
            startDateField.Click();

            // Click on the [OK] button in the datepicker
            var okBtnXPath = "//android.widget.ScrollView/" +
                "android.widget.LinearLayout/android.widget.Button[2]";
            var okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            // Locate and click on the start time field
            var startTimeField = driver.FindElementById("editTextStartTime");
            startTimeField.Click();

            // Click on the [OK] button in the timepicker
            okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            // Locate and click on the end date field
            var endDateField = driver.FindElementById("editTextEndDate");
            endDateField.Click();

            // Locate and click on the next month's button
            var nextMonthBtn = driver.FindElementByXPath("//android.widget.NumberPicker[1]" +
                "/android.widget.Button[2]");
            nextMonthBtn.Click();

            // Locate and click on the [Ok] button in the datepicker
            okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            // Locate and click on the end time field
            var endTimeField = driver.FindElementById("editTextEndTime");
            endTimeField.Click();

            // Click on the [OK] button in the timepicker
            okBtn = driver.FindElementByXPath(okBtnXPath);
            okBtn.Click();

            // Fill in valid event tickets
            var ticketsField = driver.FindElementById("editTextTickets");
            ticketsField.Clear();
            ticketsField.SendKeys("50");

            // Fill in a valid event price
            var priceField = driver.FindElementById("editTextPrice");
            priceField.Clear();
            priceField.SendKeys("10.50");

            // Locate and click on the [Create] button under the "Create" form
            var createBtn = driver.FindElementById("buttonCreate");
            createBtn.Click();

            // Switch to alert window
            wait.Until(ExpectedConditions.AlertIsPresent());
            driver.SwitchTo().Alert();

            // Assert an error appears
            var errorMsgAppered = this.wait
                    .Until(s => driver.PageSource)
                    .Contains("Place field is required.");
            Assert.IsTrue(errorMsgAppered);

            // Click on the [Ok] button to close the alert
            var okBtnErrorWindow = driver.FindElementByClassName("android.widget.Button");
            okBtnErrorWindow.Click();

            // Fill in a valid event place
            this.wait
                .Until(s => driver.FindElementById("editTextPlace"));
            placeField.SendKeys("Beach");

            // Click on the [Create] button again
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
    }
}
