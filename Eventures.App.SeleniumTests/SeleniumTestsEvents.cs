using System;
using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;

namespace Eventures.App.SeleniumTests
{
    public class SeleniumTestsEvents : SeleniumTestsBase
    {
        [OneTimeSetUp]
        public void SetupUser()
        {
            RegisterUserForTesting();
        }

        [Test]
        public void Test_HomePage_AllEventsLink()
        {
            // Arrange: go to the "Home" page and locate the link to the "All Events" page
            driver.Navigate().GoToUrl(this.baseUrl);
            var allEventsPageLink = driver.FindElement(
                By.XPath("//a[@href='/Events/All'][contains(.,'all events')]"));

            // Act: click on the link
            allEventsPageLink.Click();

            // Assert the user is redirected to the "All Events" page
            Assert.AreEqual(this.baseUrl + "/Events/All", driver.Url);
            Assert.That(driver.Title.Contains("All Events"));
            Assert.That(driver.PageSource.Contains("<h1>All Events</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a href=""/Events/Create"">Create New</a>"));
        }

        [Test]
        public void Test_AllEventsPage_ThroughNavigation()
        {
            // Arrange: go to the "Home" page
            driver.Navigate().GoToUrl(this.baseUrl);

            // Locate the dropdown menu and click on it
            driver.FindElement(By.Id("dropdownMenuLink")).Click();
            var dropdownItems = driver.FindElements(By.XPath("//a[@class='dropdown-item']"));

            // Get the first element from the dropdown menu
            var allEventsLinkItem = dropdownItems[0];

            // Act: click on the element
            allEventsLinkItem.Click();

            // Assert user is redirected to the "All Events" page
            Assert.AreEqual(this.baseUrl + "/Events/All", driver.Url);
            Assert.That(driver.Title.Contains("All Events"));
            Assert.That(driver.PageSource.Contains("<h1>All Events</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a href=""/Events/Create"">Create New</a>"));
        }

        [Test]
        public void Test_AllEventsPage_ThroughUrl()
        {
            // Arrange

            // Act: go to the "All Events" page
            driver.Navigate().GoToUrl(this.baseUrl + "/Events/All");

            // Assert the page appears
            Assert.That(driver.Title.Contains("All Events"));
            Assert.That(driver.PageSource.Contains("<h1>All Events</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a href=""/Events/Create"">Create New</a>"));
        }

        [Test]
        public void Test_HomePage_CreateEventPageLink()
        {
            // Arrange: go to the "Home" page and find the link to the "Create Event" page
            driver.Navigate().GoToUrl(this.baseUrl);
            var createEventPageLink = driver.FindElement(By.XPath("//a[@href='/Events/Create'][contains(.,'new event')]"));

            // Act: click on the link
            createEventPageLink.Click();

            // Assert the user is redirected to the "Create Event" page
            Assert.AreEqual(this.baseUrl + "/Events/Create", driver.Url);
            Assert.That(driver.Title.Contains("Create Event"));
            Assert.That(driver.PageSource.Contains("<h1>Create New Event</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a class=""btn btn-secondary"" href=""/Events/All"">Back to List</a>"));
        }

        [Test]
        public void Test_CreateEventPage_ThroughNavigation()
        {
            // Arrange: go to the "Home" page
            driver.Navigate().GoToUrl(this.baseUrl);

            // Locate the dropdown menu and click on it
            driver.FindElement(By.Id("dropdownMenuLink")).Click();

            // Get the second element from the dropdown menu
            var dropdownItems = driver.FindElements(By.XPath("//a[@class='dropdown-item']"));
            var createEventLinkItem = dropdownItems[1];

            // Act: click on the element
            createEventLinkItem.Click();

            // Assert the user is redirected to the "Create Event" page
            Assert.AreEqual(this.baseUrl + "/Events/Create", driver.Url);
            Assert.That(driver.Title.Contains("Create Event"));
            Assert.That(driver.PageSource.Contains("<h1>Create New Event</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a class=""btn btn-secondary"" href=""/Events/All"">Back to List</a>"));
        }

        [Test]
        public void Test_CreateEventPage_ThroughUrl()
        {
            // Arrange

            // Act: go to the "Create Event" page
            driver.Navigate().GoToUrl(this.baseUrl + "/Events/Create");

            // Assert the "Create Event" page appears
            Assert.That(driver.Title.Contains("Create Event"));
            Assert.That(driver.PageSource.Contains("<h1>Create New Event</h1>"));
            Assert.That(driver.PageSource.Contains(@"<a class=""btn btn-secondary"" href=""/Events/All"">Back to List</a>"));
        }

        [Test]
        public void Test_CreateEventPage_BackToListLink()
        {
            // Arrange: go to the "Create Event" page
            driver.Navigate().GoToUrl(this.baseUrl + "/Events/Create");
            Assert.That(driver.Title.Contains("Create Event"));

            // Act: click on the "Back to List" button
            driver.FindElement(By.XPath("//a[@href='/Events/All'][contains(.,'Back to List')]")).Click();

            // Assert the user is redirected to the "All Events" page
            Assert.AreEqual(this.baseUrl + "/Events/All", driver.Url);
            Assert.That(driver.Title.Contains("All Events"));
            Assert.That(driver.PageSource.Contains("<h1>All Events</h1>"));
        }

        [Test]
        public void Test_CreateEventPage_CreateEvent_ValidData()
        {
            // Arrange: go to the "Create Event" page
            driver.Navigate().GoToUrl(this.baseUrl + "/Events/Create");
            Assert.That(driver.Title.Contains("Create Event"));

            // Locate fields and fill them in with event data
            var eventName = "Party" + DateTime.UtcNow.Ticks;
            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(eventName);

            var eventPlace = "Beach";
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

            // Locate the "Create" button
            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));

            // Click on the button
            createButton.Click();

            // Assert user is redirected to the "All Events" page
            Assert.AreEqual(this.baseUrl + "/Events/All", driver.Url);
            Assert.That(driver.Title.Contains("All Events"));
            Assert.That(driver.PageSource.Contains("<h1>All Events</h1>"));

            // Assert the new event appears on the page
            Assert.That(driver.PageSource.Contains(eventName));
            Assert.That(driver.PageSource.Contains(eventPlace));
            Assert.That(driver.PageSource.Contains(username));

            // Assert the last row is the new event and it has "Delete" and "Edit" buttons
            var lastRow = driver.FindElements(By.TagName("tr")).Last();
            Assert.That(lastRow.Text.Contains(eventName));
            Assert.That(lastRow.Text.Contains("Delete"));
            Assert.That(lastRow.Text.Contains("Edit"));
        }

        [Test]
        public void Test_CreateEventPage_CreateEvent_InvalidData()
        {
            // Arrange: go to the "Create Event" page
            driver.Navigate().GoToUrl(this.baseUrl + "/Events/Create");
            Assert.That(driver.Title.Contains("Create Event"));

            // Locate fields and fill them in with event data
            // Fill in an invalid event name: name == empty string
            var invalidEventName = string.Empty;
            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(invalidEventName);

            // Locate the "Create" button
            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));

            // Click on the button
            createButton.Click();

            // Assert the user stays on the same page
            Assert.AreEqual(this.baseUrl + "/Events/Create", driver.Url);

            // Assert that an error message appears on the page
            Assert.That(driver.PageSource.Contains("The Name field is required."));
        }

        [Test]
        public void Test_DeleteEvent()
        {
            // Arrange: go to the "Create Event" page and create a new event for deleting
            driver.Navigate().GoToUrl(this.baseUrl + "/Events/Create");
            Assert.That(driver.Title.Contains("Create Event"));
            
            var eventName = "Best Show" + DateTime.UtcNow.Ticks;
            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(eventName);

            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));
            createButton.Click();

            // Assert user is redirected to the "All Events" page and the new event appears on the page
            Assert.AreEqual(this.baseUrl + "/Events/All", driver.Url);
            Assert.That(driver.PageSource.Contains(eventName));

            // Get the last row with the new event and locate the "Delete" button of the event
            var lastRow = driver.FindElements(By.TagName("tr")).Last();
            Assert.That(lastRow.Text.Contains(eventName));
            var deleteBtn = driver.FindElements(By.XPath("//a[contains(.,'Delete')]")).Last();

            // Click on the "Delete" button
            deleteBtn.Click();

            // Assert the user is redirected to the "Delete Event" page
            Assert.That(driver.Url.Contains("/Events/Delete/"));
            Assert.That(driver.Title.Contains("Delete Event"));
            Assert.That(driver.PageSource.Contains(eventName));

            // Click on the new "Delete" button to confirm deletion
            var confirmDeleteButton = driver.FindElement(By.XPath("//input[contains(@value,'Delete')]"));
            confirmDeleteButton.Click();

            // Assert the user is redirected to the "All Events" page
            Assert.AreEqual(this.baseUrl + "/Events/All", driver.Url);

            // Assert that the event doesn't appear on the page
            Assert.That(!driver.PageSource.Contains(eventName));
        }

        [Test]
        public void Test_EditEvent_ValidData()
        {
            // Arrange: go to the "Create Event" page and create a new event for editing
            driver.Navigate().GoToUrl(this.baseUrl + "/Events/Create");
            Assert.That(driver.Title.Contains("Create Event"));

            var eventName = "Best Show" + DateTime.UtcNow.Ticks;
            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(eventName);

            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));
            createButton.Click();

            // Assert user is redirected to the "All Events" page and the new event appears on the page
            Assert.AreEqual(this.baseUrl + "/Events/All", driver.Url);
            Assert.That(driver.PageSource.Contains(eventName));

            // Get the last row with the event and locate the "Edit" button of the event
            var lastRow = driver.FindElements(By.TagName("tr")).Last();
            Assert.That(lastRow.Text.Contains(eventName));
            var editButton = driver.FindElements(By.XPath("//a[contains(.,'Edit')]")).Last();

            // Click on the "Edit" button
            editButton.Click();

            // Assert the user is redirected to the "Edit Event" page
            Assert.That(driver.Url.Contains("/Events/Edit/"));
            Assert.That(driver.Title.Contains("Edit Event"));

            // Change the name of the event
            var editNameField = driver.FindElement(By.Id("Name"));
            var changedName = "Best Best Show" + DateTime.UtcNow.Ticks;
            editNameField.Clear();
            editNameField.SendKeys(changedName);

            var confirmEditButton = driver.FindElement(By.XPath("//input[contains(@value,'Edit')]"));

            // Click on the new "Edit" button to confirm edition
            confirmEditButton.Click();

            // Assert the user is redirected to the "All Events" page
            Assert.AreEqual(this.baseUrl + "/Events/All", driver.Url);

            // Assert that the page contains the new event name and not the old one
            Assert.That(driver.PageSource.Contains(changedName));
            Assert.That(!driver.PageSource.Contains(eventName));
        }

        [Test]
        public void Test_EditEvent_InvalidData()
        {
            // Arrange: go to the "Create Event" page and create a new event for editing
            driver.Navigate().GoToUrl(this.baseUrl + "/Events/Create");
            Assert.That(driver.Title.Contains("Create Event"));

            var eventName = "Best Show" + DateTime.UtcNow.Ticks;
            var nameField = driver.FindElement(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(eventName);

            var createButton = driver.FindElement(By.XPath("//input[contains(@value,'Create')]"));
            createButton.Click();

            // Assert the user is redirected to the "All Events" page and the new event appears on the page
            Assert.AreEqual(this.baseUrl + "/Events/All", driver.Url);
            Assert.That(driver.PageSource.Contains(eventName));

            // Get the last row with the new event and locate the "Edit" button of the event
            var lastRow = driver.FindElements(By.TagName("tr")).Last();
            Assert.That(lastRow.Text.Contains(eventName));
            var editButton = driver.FindElements(By.XPath("//a[contains(.,'Edit')]")).Last();

            // Click on the "Edit" button
            editButton.Click();

            // Assert the user is redirected to the "Edit Event" page
            Assert.That(driver.Url.Contains("/Events/Edit/"));
            Assert.That(driver.Title.Contains("Edit Event"));

            // Change the name of the event with invalid one: name == empty string
            var editNameField = driver.FindElement(By.Id("Name"));
            var invalidName = string.Empty;
            editNameField.Clear();
            editNameField.SendKeys(invalidName);

            var confirmEditButton = driver.FindElement(By.XPath("//input[contains(@value,'Edit')]"));

            // Click on the new "Edit" button to confirm edition
            confirmEditButton.Click();

            // Assert the user is on the same page
            Assert.That(driver.Url.Contains("/Events/Edit/"));

            // Assert an error message appears on the page
            Assert.That(driver.PageSource.Contains("The Name field is required."));
        }

        private void RegisterUserForTesting()
        {
            driver.Navigate().GoToUrl(this.baseUrl + "/Identity/Account/Register");

            driver.FindElement(By.Id("Input_Username")).SendKeys(username);
            driver.FindElement(By.Id("Input_Email")).SendKeys($"{username}@mail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys(password);
            driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys(password);
            driver.FindElement(By.Id("Input_FirstName")).SendKeys("Pesho");
            driver.FindElement(By.Id("Input_LastName")).SendKeys("Petrov");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Register')]")).Click();

            Assert.AreEqual(this.baseUrl + "/", driver.Url);
            Assert.That(driver.PageSource.Contains($"Welcome, {username}"));
        }
    }
}
