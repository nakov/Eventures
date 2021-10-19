using System;

using Eventures.WebApp.SeleniumPOMTests.PageObjects;

using NUnit.Framework;

namespace Eventures.WebApp.SeleniumPOMTests
{
    public class SeleniumPOMTestsEvents : SeleniumPOMTestsBase
    {
        [OneTimeSetUp]
        public void SetupUser()
        {
            RegisterUserForTesting();
        }

        [Test]
        public void Test_HomePage_AllEventsLink()
        {
            // Arrange: go to the "Home" page
            var homePage = new HomePage(driver);
            homePage.Open(baseUrl);

            // Act: click on the "All Events" page link
            homePage.AllEventsPageLink.Click();

            // Assert the user is redirected to the "All Events" page
            var allEventsPage = new AllEventsPage(driver);
            Assert.IsTrue(allEventsPage.IsOpen(baseUrl));
            Assert.AreEqual("All Events - Eventures App", allEventsPage.GetPageTitle());
            Assert.AreEqual("All Events", allEventsPage.GetPageHeadingText());
            Assert.That(allEventsPage.Contains("Create New"));
        }

        [Test]
        public void Test_CreateEventPage_CreateEvent_ValidData()
        {
            // Arrange: open the "Create Event" page
            var createEventPage = new CreateEventPage(driver);
            createEventPage.Open(baseUrl);

            // act: create a new event
            var eventName = "Party" + DateTime.Now.Ticks;
            var eventPlace = "Beach";
            createEventPage.CreateEvent(eventName, eventPlace, "100", "10.00");

            // Assert user is redirected to the "All Events" page
            var allEventsPage = new AllEventsPage(driver);
            Assert.IsTrue(allEventsPage.IsOpen(baseUrl));

            // Assert the new event appears
            var eventRowText = allEventsPage.GetEventRow(eventName);
            Assert.That(eventRowText.Contains(eventName));
            Assert.That(eventRowText.Contains(eventPlace));
            Assert.That(eventRowText.Contains(username));
            Assert.That(eventRowText.Contains("Delete"));
            Assert.That(eventRowText.Contains("Edit"));
        }

        [Test]
        public void Test_CreateEventPage_CreateEvent_InvalidData()
        {
            // Arrange: go to the "Create Event" page
            var createEventPage = new CreateEventPage(driver);
            createEventPage.Open(baseUrl);

            // Create an event with an invalid name
            var invalidEventName = string.Empty;
            var eventPlace = "Beach";
            createEventPage.CreateEvent(invalidEventName, eventPlace, "100", "10.00");

            // Assert the user stays on the same page
            Assert.IsTrue(createEventPage.IsOpen(baseUrl));

            // Assert that an error message appears on the page
            Assert.AreEqual("The Name field is required.", createEventPage.GetNameError());
        }

        [Test]
        public void Test_DeleteEvent()
        {
            // Arrange: create an event for deleting
            string eventName = "Best Show" + DateTime.Now.Ticks;
            this.CreateEvent(eventName);

            // Assert the user is redirected to the "All Events" page
            var allEventsPage = new AllEventsPage(driver);
            Assert.IsTrue(allEventsPage.IsOpen(baseUrl));

            // Get the row with the new event
            var eventRowText = allEventsPage.GetEventRow(eventName);
            Assert.That(eventRowText.Contains(eventName));

            // Click on the "Edit" button of the new event
            allEventsPage.PressEventDeleteButton(eventName);

            // Assert the user is redirected to the "Delete Event" page
            var deletePage = new DeletePage(driver);
            Assert.IsTrue(deletePage.IsOpen(baseUrl));
            Assert.AreEqual("Delete Event - Eventures App", deletePage.GetPageTitle());
            Assert.AreEqual("Delete Existing Event", deletePage.GetPageHeadingText());
            Assert.IsTrue(deletePage.Contains(eventName));

            // Act: click on the "Delete" button to confirm deletion
            deletePage.PressConfirmDeleteButton();

            // Assert the user is redirected to the "All Events" page
            Assert.IsTrue(allEventsPage.IsOpen(baseUrl));

            // Assert that the event doesn't appear on the page
            Assert.IsFalse(allEventsPage.Contains(eventName));
        }

        [Test]
        public void Test_EditEvent_ValidData()
        {
            // Create an event for editing
            string eventName = "Best Show" + DateTime.Now.Ticks;
            this.CreateEvent(eventName);

            // Assert user is redirected to the "All Events" page
            var allEventsPage = new AllEventsPage(driver);
            Assert.IsTrue(allEventsPage.IsOpen(baseUrl));

            // Get the row with the new event
            var eventRowText = allEventsPage.GetEventRow(eventName);
            Assert.That(eventRowText.Contains(eventName));

            // Click on the "Edit" button of the new event
            allEventsPage.PressEventEditButton(eventName);

            // Assert the user is redirected to the "Edit Event" page
            var editPage = new EditPage(driver);
            Assert.IsTrue(editPage.IsOpen(baseUrl));
            Assert.AreEqual("Edit Event - Eventures App", editPage.GetPageTitle());
            Assert.AreEqual("Edit Event", editPage.GetPageHeadingText());
            Assert.IsTrue(editPage.Contains(eventName));

            // Change the name of the event
            var changedName = "Best Best Show" + DateTime.Now.Ticks;
            editPage.EditEventName(changedName);

            // Assert the user is redirected to the "All Events" page
            Assert.IsTrue(allEventsPage.IsOpen(baseUrl));

            // Assert that the page contains the new event name and not the old one
            Assert.IsTrue(allEventsPage.Contains(changedName));
            Assert.IsFalse(allEventsPage.Contains(eventName));
        }

        [Test]
        public void Test_EditEvent_InvalidData()
        {
            // Create an event for editing
            string eventName = "Best Show" + DateTime.Now.Ticks;
            this.CreateEvent(eventName);

            // Assert user is redirected to the "All Events" page
            var allEventsPage = new AllEventsPage(driver);
            Assert.IsTrue(allEventsPage.IsOpen(baseUrl));

            // Get the row with the new event
            var eventRowText = allEventsPage.GetEventRow(eventName);
            Assert.That(eventRowText.Contains("Edit"));

            // Click on the "Edit" button of the new event
            allEventsPage.PressEventEditButton(eventName);

            // Assert the user is redirected to the "Edit Event" page
            var editPage = new EditPage(driver);
            Assert.IsTrue(editPage.IsOpen(baseUrl));
            Assert.AreEqual("Edit Event - Eventures App", editPage.GetPageTitle());
            Assert.AreEqual("Edit Event", editPage.GetPageHeadingText());
            Assert.IsTrue(editPage.Contains(eventName));

            // Change the name of the event with an invalid one
            var invalidName = string.Empty;
            editPage.EditEventName(invalidName);

            // Assert the user is on the same page
            Assert.IsTrue(editPage.IsOpen(baseUrl));

            // Assert an error message appears on the page
            Assert.AreEqual("The Name field is required.", editPage.GetNameError());
        }

        private void RegisterUserForTesting()
        {
            // Register a user through the "Register" page
            var registerPage = new RegisterPage(driver);
            registerPage.Open(baseUrl);
            registerPage.RegisterUser(username, username + "@mail.com", password, 
                password, "Pesho", "Peshov");

            // Assert the user is redirected to the "Home" page and is logged in
            var homePage = new HomePage(driver);
            Assert.IsTrue(homePage.IsOpen(baseUrl));
            Assert.That(homePage.Contains($"Welcome, {username}"));
        }

        private void CreateEvent(string eventName)
        {
            // Create a new event through the "Create Event" page
            var createPage = new CreateEventPage(driver);
            createPage.Open(baseUrl);
            createPage.CreateEvent(eventName, "Beach", "100", "10");
        }
    }
}
