using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace Eventures.SeleniumTests
{
    public class SeleniumTests_Events
    {
        IWebDriver driver;
        string username = "pesho" + DateTime.UtcNow.Ticks;
        string password = "pass123123";

        [OneTimeSetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            this.driver.Manage().Window.Maximize();
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            RegisterUser();
        }

        [Test]
        public void Test_HomePage_AllEventsLink()
        {
            driver.Url = "https://localhost:44395/";
            var allEventsPageLink = driver.FindElement(By.XPath("//a[@href='/Events/All'][contains(.,'all events')]"));
            allEventsPageLink.Click();
            Assert.AreEqual("https://localhost:44395/Events/All", driver.Url);
            Assert.That(driver.Title.Contains("All Events"));
            Assert.That(driver.PageSource.Contains("<h1>All Events</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a href=""/Events/Create"">Create New</a>"));
        }

        [Test]
        public void Test_AllEventsPage_ThroughNavigation()
        {
            driver.Url = "https://localhost:44395/";
            driver.FindElement(By.Id("dropdownMenuLink")).Click();
            var dropdownItems = driver.FindElements(By.XPath("//a[@class='dropdown-item']"));
            var allEventsLinkItem = dropdownItems[0];
            allEventsLinkItem.Click();
            Assert.AreEqual("https://localhost:44395/Events/All", driver.Url);
            Assert.That(driver.Title.Contains("All Events"));
            Assert.That(driver.PageSource.Contains("<h1>All Events</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a href=""/Events/Create"">Create New</a>"));
        }

        [Test]
        public void Test_AllEventsPage_ThroughUrl()
        {
            driver.Url = "https://localhost:44395/Events/All";
            Assert.That(driver.Title.Contains("All Events"));
            Assert.That(driver.PageSource.Contains("<h1>All Events</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a href=""/Events/Create"">Create New</a>"));
        }

        [Test]
        public void Test_HomePage_CreateEventPageLink()
        {
            driver.Url = "https://localhost:44395/";
            var createEventPageLink = driver.FindElement(By.XPath("//a[@href='/Events/Create'][contains(.,'new event')]"));
            createEventPageLink.Click();
            Assert.AreEqual("https://localhost:44395/Events/Create", driver.Url);
            Assert.That(driver.Title.Contains("Create Event"));
            Assert.That(driver.PageSource.Contains("<h1>Create New Event</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a href=""/Events/All"">Back to List</a>"));
        }

        [Test]
        public void Test_CreateEventPage_ThroughNavigation()
        {
            driver.Url = "https://localhost:44395/";
            driver.FindElement(By.Id("dropdownMenuLink")).Click();
            var dropdownItems = driver.FindElements(By.XPath("//a[@class='dropdown-item']"));
            var createEventLinkItem = dropdownItems[1];
            createEventLinkItem.Click();
            Assert.AreEqual("https://localhost:44395/Events/Create", driver.Url);
            Assert.That(driver.Title.Contains("Create Event"));
            Assert.That(driver.PageSource.Contains("<h1>Create New Event</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a href=""/Events/All"">Back to List</a>"));
        }

        [Test]
        public void Test_CreateEventPage_ThroughUrl()
        {
            driver.Url = "https://localhost:44395/Events/Create";
            Assert.That(driver.Title.Contains("Create Event"));
            Assert.That(driver.PageSource.Contains("<h1>Create New Event</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a href=""/Events/All"">Back to List</a>"));
        }

        [Test]
        public void Test_CreateEventPage_BackToListLink()
        {
            driver.Url = "https://localhost:44395/Events/Create";
            Assert.That(driver.Title.Contains("Create Event"));

            driver.FindElement(By.XPath("//a[@href='/Events/All'][contains(.,'Back to List')]")).Click();
            Assert.That(driver.Title.Contains("All Events"));
            Assert.That(driver.PageSource.Contains("<h1>All Events</h1>"));
        }

        [Test]
        public void Test_CreateEventPage_CreateEvent_ValidData()
        {
            driver.Url = "https://localhost:44395/Events/Create";
            Assert.That(driver.Title.Contains("Create Event"));
            var eventName = "Party" + DateTime.UtcNow.Ticks;
            var eventPlace = "Beach";

            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(eventName);

            var placeField = driver.FindElement(By.Id("Place"));
            placeField.Clear();
            placeField.SendKeys(eventPlace);

            // DateTime is not set properly
            //var startField = driver.FindElement(By.Id("Start"));
            //startField.Clear();
            //startField.SendKeys(DateTime.Now.ToString();

            //var endField = driver.FindElement(By.Id("End"));
            //endField.Clear();
            //endField.SendKeys("2021-06-26T10:00:00.000");

            var totalTicketsField = driver.FindElement(By.Id("TotalTickets"));
            totalTicketsField.Clear();
            totalTicketsField.SendKeys("100");

            var priceField = driver.FindElement(By.Id("PricePerTicket"));
            priceField.Clear();
            priceField.SendKeys("10.00");

            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));
            createButton.Click();

            Assert.That(driver.PageSource.Contains(eventName));
            Assert.That(driver.PageSource.Contains(eventPlace));
            Assert.That(driver.PageSource.Contains(username));

            var lastRow = driver.FindElements(By.TagName("tr")).Last();
            Assert.That(lastRow.Text.Contains(eventName));
            Assert.That(lastRow.Text.Contains("Delete"));
            Assert.That(lastRow.Text.Contains("Edit"));
        }

        [Test]
        public void Test_CreateEventPage_CreateEvent_InvalidData()
        {
            driver.Url = "https://localhost:44395/Events/Create";
            Assert.That(driver.Title.Contains("Create Event"));
            var invalidEventName = string.Empty;

            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(invalidEventName);

            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));
            createButton.Click();

            Assert.AreEqual("https://localhost:44395/Events/Create", driver.Url);
            Assert.That(driver.PageSource.Contains("The Name field is required."));
        }

        [Test]
        public void Test_DeleteEvent()
        {
            driver.Url = "https://localhost:44395/Events/Create";
            Assert.That(driver.Title.Contains("Create Event"));
            
            var eventName = "Best Show" + DateTime.UtcNow.Ticks;
            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(eventName);

            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));
            createButton.Click();

            var lastRow = driver.FindElements(By.TagName("tr")).Last();
            Assert.That(lastRow.Text.Contains(eventName));
            
            var deleteBtn = driver.FindElements(By.XPath("//a[contains(.,'Delete')]")).Last();
            deleteBtn.Click();

            Assert.That(driver.Url.Contains("https://localhost:44395/Events/Delete/"));
            Assert.That(driver.Title.Contains("Delete Event"));
            Assert.That(driver.PageSource.Contains(eventName));

            var confirmDeleteButton = driver.FindElement(By.XPath("//input[contains(@value,'Delete')]"));
            confirmDeleteButton.Click();

            Assert.AreEqual("https://localhost:44395/Events/All", driver.Url);
            Assert.That(!driver.PageSource.Contains(eventName));
        }

        [Test]
        public void Test_EditEvent_ValidData()
        {
            driver.Url = "https://localhost:44395/Events/Create";
            Assert.That(driver.Title.Contains("Create Event"));

            var eventName = "Best Show" + DateTime.UtcNow.Ticks;
            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(eventName);

            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));
            createButton.Click();

            Assert.That(driver.PageSource.Contains(eventName));

            var lastRow = driver.FindElements(By.TagName("tr")).Last();
            Assert.That(lastRow.Text.Contains(eventName));

            var editButton = driver.FindElements(By.XPath("//a[contains(.,'Edit')]")).Last();
            editButton.Click();

            Assert.That(driver.Url.Contains("https://localhost:44395/Events/Edit/"));
            Assert.That(driver.Title.Contains("Edit Event"));

            var editNameField = driver.FindElement(By.Id("Name"));
            var changedName = "Best Best Show" + DateTime.UtcNow.Ticks;
            editNameField.Clear();
            editNameField.SendKeys(changedName);

            var confirmEditButton = driver.FindElement(By.XPath("//input[contains(@value,'Edit')]"));
            confirmEditButton.Click();

            Assert.AreEqual("https://localhost:44395/Events/All", driver.Url);
            Assert.That(driver.PageSource.Contains(changedName));
            Assert.That(!driver.PageSource.Contains(eventName));
        }

        [Test]
        public void Test_EditEvent_InvalidData()
        {
            driver.Url = "https://localhost:44395/Events/Create";
            Assert.That(driver.Title.Contains("Create Event"));

            var eventName = "Best Show" + DateTime.UtcNow.Ticks;
            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(eventName);

            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));
            createButton.Click();

            Assert.That(driver.PageSource.Contains(eventName));

            var lastRow = driver.FindElements(By.TagName("tr")).Last();
            Assert.That(lastRow.Text.Contains(eventName));

            var editButton = driver.FindElements(By.XPath("//a[contains(.,'Edit')]")).Last();
            editButton.Click();

            Assert.That(driver.Url.Contains("https://localhost:44395/Events/Edit/"));
            Assert.That(driver.Title.Contains("Edit Event"));

            var editNameField = driver.FindElement(By.Id("Name"));
            var invalidName = string.Empty;
            editNameField.Clear();
            editNameField.SendKeys(invalidName);

            var confirmEditButton = driver.FindElement(By.XPath("//input[contains(@value,'Edit')]"));
            confirmEditButton.Click();

            Assert.That(driver.Url.Contains("https://localhost:44395/Events/Edit/"));
            Assert.That(driver.PageSource.Contains("The Name field is required."));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        private void RegisterUser()
        {
            driver.Url = "https://localhost:44395/Identity/Account/Register";

            driver.FindElement(By.Id("Input_Username")).SendKeys(username);
            driver.FindElement(By.Id("Input_Email")).SendKeys($"{username}@mail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys(password);
            driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys(password);
            driver.FindElement(By.Id("Input_FirstName")).SendKeys("Pesho");
            driver.FindElement(By.Id("Input_LastName")).SendKeys("Petrov");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Register')]")).Click();

            Assert.AreEqual("https://localhost:44395/", driver.Url);
            Assert.That(driver.PageSource.Contains($"Welcome, {username}"));
        }
    }
}
