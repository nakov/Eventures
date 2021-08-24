using System;
using System.Linq;
using NUnit.Framework;

namespace Eventures.AndroidApp.AppiumTests
{
    public class AppiumTests : AppiumTestsBase
    {
        private string username = "newUser" + DateTime.UtcNow.Ticks;
        private string password = "newPassword12";
        private string buttonConnectId = "buttonConnect";
        private string buttonLoginId = "buttonLogin";
        private string buttonRegisterId = "buttonRegister";
        private string buttonAddId = "buttonAdd";
        private string buttonReloadId = "buttonReload";
        private string statusBoxId = "textViewStatus";

        [Test, Order(1)]
        public void Test_Connect_WithInvalidUrl()
        {
            var loginBtn = driver.FindElementById(buttonLoginId);
            Assert.That(loginBtn.Enabled == false);
            var registerBtn = driver.FindElementById(buttonRegisterId);
            Assert.That(registerBtn.Enabled == false);

            var connectBtn = driver.FindElementById(buttonConnectId);
            connectBtn.Click();

            var apiUrlField = driver.FindElementById("editTextApiUrl");
            apiUrlField.Clear();

            var invalidUrl = "http://10.0.2.2:1234/api/";
            apiUrlField.SendKeys(invalidUrl);

            var confirmConnectBtn = driver.FindElementById("buttonConfirmConnect");
            confirmConnectBtn.Click();

            var statusBox = driver.FindElementById(statusBoxId);
            while (statusBox.Text == "Connecting ...")
            {
                statusBox = driver.FindElementById(statusBoxId);
            }

            Assert.AreEqual("Could not connect. Try again.", statusBox.Text);

            Assert.That(loginBtn.Enabled == false);
            Assert.That(registerBtn.Enabled == false);
        }

        [Test, Order(2)]
        public void Test_Connect_WithValidUrl()
        {
            var loginBtn = driver.FindElementById(buttonLoginId);
            Assert.That(loginBtn.Enabled == false);
            var registerBtn = driver.FindElementById(buttonRegisterId);
            Assert.That(registerBtn.Enabled == false);

            var connectBtn = driver.FindElementById(buttonConnectId);
            connectBtn.Click();

            var apiUrlField = driver.FindElementById("editTextApiUrl");
            apiUrlField.Clear();
            apiUrlField.SendKeys(@$"{this.baseUrl}/api/");

            var confirmConnectBtn = driver.FindElementById("buttonConfirmConnect");
            confirmConnectBtn.Click();

            var statusBox = driver.FindElementById(statusBoxId);
            while (statusBox.Text == "Connecting ...")
            {
                statusBox = driver.FindElementById(statusBoxId);
            }

            Assert.AreEqual("Connected successfully.", statusBox.Text);

            Assert.That(loginBtn.Enabled == true);
            Assert.That(registerBtn.Enabled == true);
        }

        [Test, Order(3)]
        public void Test_Register()
        {
            var addBtn = driver.FindElementById(buttonAddId);
            Assert.That(addBtn.Enabled == false);

            var reloadBtn = driver.FindElementById(buttonReloadId);
            Assert.That(reloadBtn.Enabled == false);

            var registerBtn = driver.FindElementById(buttonRegisterId);
            Assert.That(registerBtn.Enabled == true);
            registerBtn.Click();

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

            var confirmRegisterBtn = driver.FindElementById("buttonConfirmRegister");
            confirmRegisterBtn.Click();

            var statusBox = driver.FindElementById(statusBoxId);
            while (!statusBox.Text.Contains("Events found:"))
            {
                statusBox = driver.FindElementById(statusBoxId);
            }

            var eventsInDb = this.dbContext.Events.Count();
            Assert.That(statusBox.Text.Contains($"Events found: {eventsInDb}"));

            Assert.That(addBtn.Enabled == true);
            Assert.That(reloadBtn.Enabled == true);
        }

        [Test]
        public void Test_Login()
        {
            var loginBtn = driver.FindElementById(buttonLoginId);
            Assert.That(loginBtn.Enabled == true);
            loginBtn.Click();

            var usernameField = driver.FindElementById("editTextUsername");
            usernameField.Clear();
            usernameField.SendKeys(this.username);

            var passwordField = driver.FindElementById("editTextPassword");
            passwordField.Clear();
            passwordField.SendKeys(this.password);

            var confirmLoginBtn = driver.FindElementById("buttonConfirmLogin");
            confirmLoginBtn.Click();

            var statusBox = driver.FindElementById(statusBoxId);
            while (!statusBox.Text.Contains("Events found:"))
            {
                statusBox = driver.FindElementById(statusBoxId);
            }

            var eventsInDb = this.dbContext.Events.Count();
            Assert.That(statusBox.Text.Contains($"Events found: {eventsInDb}"));

            var addBtn = driver.FindElementById(buttonAddId);
            Assert.That(addBtn.Enabled == true);

            var reloadBtn = driver.FindElementById(buttonReloadId);
            Assert.That(reloadBtn.Enabled == true);
        }

        [Test]
        public void Test_Reload()
        {
            var reloadBtn = driver.FindElementById(buttonReloadId);
            Assert.That(reloadBtn.Enabled == true);
            reloadBtn.Click();

            var statusBox = driver.FindElementById(statusBoxId);
            while (!statusBox.Text.Contains("Events found:"))
            {
                statusBox = driver.FindElementById(statusBoxId);
            }

            var eventsInDb = this.dbContext.Events.Count();
            Assert.That(statusBox.Text.Contains($"Events found: {eventsInDb}"));
        }

        [Test]
        public void Test_CreateEvent_WithValidData()
        {
            var eventsInDbBefore = this.dbContext.Events.Count();

            var addBtn = driver.FindElementById(buttonAddId);
            Assert.That(addBtn.Enabled == true);
            addBtn.Click();

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

            var endDateField = driver.FindElementById("editTextEndDate");
            endDateField.Click();
            // Change month to "Sep"
            var nextMonthBtn = driver.FindElementByXPath("//android.widget.NumberPicker[1]/android.widget.Button[2]");
            nextMonthBtn.Click();
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

            var createBtn = driver.FindElementById("buttonCreate");
            createBtn.Click();

            var statusBox = driver.FindElementById(statusBoxId);
            while (!statusBox.Text.Contains("Events found:"))
            {
                statusBox = driver.FindElementById(statusBoxId);
            }

            var eventsInDbAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsInDbBefore + 1, eventsInDbAfter);
            Assert.That(statusBox.Text.Contains($"Events found: {eventsInDbAfter}"));
        }

        [Test]
        public void Test_CreateEvent_WithInvalidData()
        {
            var eventsInDbBefore = this.dbContext.Events.Count();

            var addBtn = driver.FindElementById(buttonAddId);
            Assert.That(addBtn.Enabled == true);
            addBtn.Click();

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

            var createBtn = driver.FindElementById("buttonCreate");
            createBtn.Click();

            var statusBox = driver.FindElementById(statusBoxId);
            while (statusBox.Text.Contains("Creating new event ..."))
            {
                statusBox = driver.FindElementById(statusBoxId);
            }

            Assert.That(statusBox.Text.Contains("Could not create the new event. Try again."));

            var eventsInDbAfter = this.dbContext.Events.Count();
            Assert.AreEqual(eventsInDbBefore, eventsInDbAfter);
        }
    }
}
