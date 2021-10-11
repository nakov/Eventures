using Eventures.WebApp.SeleniumPOMTests.PageObjects;

using NUnit.Framework;

namespace Eventures.WebApp.SeleniumPOMTests
{
    public class SeleniumPOMTestsUser : SeleniumPOMTestsBase
    {
        [Test, Order(1)]
        public void Test_User_Register()
        {
            var registerPage = new RegisterPage(driver);
            registerPage.Open(baseUrl);
            registerPage.RegisterUser(username, username + "@mail.com", password, password, "Pesho", "Peshov");

            // Assert the user is redirected to the "Home" page and is logged in
            var homePage = new HomePage(driver);
            Assert.IsTrue(homePage.IsOpen(baseUrl));
            Assert.IsTrue(homePage.Contains($"Welcome, {username}"));
        }

        [Test, Order(2)]
        public void Test_User_Login()
        {
            var loginPage = new LoginPage(driver);
            loginPage.Open(baseUrl);
            loginPage.LogInUser(username, password);

            // Assert user is redirected to the "Home" page and is logged in
            var homePage = new HomePage(driver);
            Assert.IsTrue(homePage.IsOpen(baseUrl));
            Assert.IsTrue(homePage.Contains($"Welcome, {username}"));
        }

        [Test, Order(3)]
        public void Test_User_Logout()
        {
            var homePage = new HomePage(driver);
            homePage.Open(baseUrl);
            homePage.LogoutButton.Click();

            // Assert user is redirected to the "Home" page and is logged out
            Assert.IsTrue(homePage.IsOpen(baseUrl));
            Assert.IsTrue(homePage.Contains($"Eventures: Events and Tickets"));
        }

        [Test]
        public void Test_HomePage_LoginPageLink_InNavigation()
        {
            var homePage = new HomePage(driver);
            homePage.Open(baseUrl);
            homePage.LoginPageLinkInNav.Click();

            // Assert the user is redirected to the "Log in" page
            var loginPage = new LoginPage(driver);
            Assert.IsTrue(loginPage.IsOpen(baseUrl));
            Assert.AreEqual("Log in - Eventures App", loginPage.GetPageTitle());
            Assert.AreEqual("Log in", loginPage.GetPageHeadingText());
            Assert.IsTrue(loginPage.Contains("Use a local account to log in"));
        }
        
        [Test]
        public void Test_AllEventsPage_Anonymous()
        {
            // Arrange

            // Act: go to the "All Events" page
            var allEventsPage = new AllEventsPage(driver);
            allEventsPage.Open(baseUrl);

            // Assert the user is redirected to the "Log in" page
            var loginPage = new LoginPage(driver);
            Assert.IsTrue(loginPage.IsOpenWhenUnauthorized(baseUrl));
            Assert.AreEqual("Log in - Eventures App", loginPage.GetPageTitle());
            Assert.AreEqual("Log in", loginPage.GetPageHeadingText());
            Assert.IsTrue(loginPage.Contains("Use a local account to log in"));
        }
    }
}