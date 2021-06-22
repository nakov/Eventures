using Eventures.Tests.Common;
using Eventures.UnitTests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Eventures.SeleniumTests
{
    public class SeleniumTests_User
    {
        TestDb testDb;
        IWebDriver driver;
        string username = "testuser" + DateTime.UtcNow.Ticks;
        string password = "password" + DateTime.UtcNow.Ticks;
        TestEventuresApp testEventuresApp;
        string baseUrl;

        [OneTimeSetUp]
        public void Setup()
        {
            // Run the Web app in a local Web server
            this.testDb = new TestDb();
            this.testEventuresApp = new TestEventuresApp(testDb);
            this.baseUrl = this.testEventuresApp.ServerUri;

            // Setup the ChromeDriver
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("--allow-insecure-localhost");
            chromeOptions.AddArguments("--start-maximized");
            this.driver = new ChromeDriver(chromeOptions);
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test, Order(1)]
        public void Test_User_Register()
        {
            // Arrange: go to the "Registration" page
            driver.Navigate().GoToUrl(this.baseUrl + "/Identity/Account/Register");

            // Locate fields and fill them in
            driver.FindElement(By.Id("Input_Username")).SendKeys(username);
            driver.FindElement(By.Id("Input_Email")).SendKeys($"{username}@mail.com");
            driver.FindElement(By.Id("Input_Password")).SendKeys(password);
            driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys(password);
            driver.FindElement(By.Id("Input_FirstName")).SendKeys("Pesho");
            driver.FindElement(By.Id("Input_LastName")).SendKeys("Petrov");

            // Act: locate and click on the "Register" button
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Register')]")).Click();

            // Assert user is redirected to the "Home" page and is logged in
            Assert.AreEqual(this.baseUrl + "/", driver.Url);
            Assert.That(driver.PageSource.Contains($"Welcome, {username}"));
        }

        [Test, Order(2)]
        public void Test_User_Login()
        {
            // Arrange: go to the "Login" page
            driver.Navigate().GoToUrl(this.baseUrl + "/Identity/Account/Login");

            // Locate fields and fill them in with valid credentials
            driver.FindElement(By.Id("Input_Username")).SendKeys(username);
            driver.FindElement(By.Id("Input_Password")).SendKeys(password);

            // Act: locate and click on the "Login" button
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Log in')]")).Click();

            // Assert user is redirected to the "Home" page and is logged in
            Assert.AreEqual(this.baseUrl + "/", driver.Url);
            Assert.That(driver.PageSource.Contains($"Welcome, {username}"));
        }

        [Test, Order(3)]
        public void Test_User_Logout()
        {
            // Arrange: go to the "Home" page
            driver.Navigate().GoToUrl(this.baseUrl);

            // Locate and click on the "Logout" button
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Logout')]")).Click();

            // Assert user is redirected to the "Home" page and is logged out
            Assert.AreEqual(this.baseUrl + "/", driver.Url);
            Assert.That(driver.PageSource.Contains("Eventures: Events and Tickets"));
        }

        [Test]
        public void Test_HomePage_LoginPageLink_InNavigation()
        {
            // Arrange: go to the "Home" page
            driver.Navigate().GoToUrl(this.baseUrl);

            // Act: locate and click on [Login] in the navigation bar
            driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Login'])[1]")).Click();
            
            // Assert user is redirected to the "Log in" page
            Assert.AreEqual(this.baseUrl + "/Identity/Account/Login", driver.Url);
            Assert.That(driver.Title.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Use a local account to log in"));
        }

        [Test]
        public void Test_HomePage_LoginPageLink_OnPage()
        {
            // Arrange: go to the "Home" page
            driver.Navigate().GoToUrl(this.baseUrl);

            // Act: locate and click on [Login] on the main page
            driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Login'])[2]")).Click();

            // Assert user is redirected to the "Log in" page
            Assert.AreEqual(this.baseUrl + "/Identity/Account/Login", driver.Url);
            Assert.That(driver.Title.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Use a local account to log in"));
        }

        [Test]
        public void Test_HomePage_RegisterPageLink_InNavigation()
        {
            // Arrange: go to the "Home" page
            driver.Navigate().GoToUrl(this.baseUrl);

            // Act: locate and click on [Register] in the navigation bar
            driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Register'])[1]")).Click();

            // Assert user is redirected to the "Register" page
            Assert.AreEqual(this.baseUrl + "/Identity/Account/Register", driver.Url);
            Assert.That(driver.Title.Contains("Register"));
            Assert.That(driver.PageSource.Contains("Register"));
            Assert.That(driver.PageSource.Contains("Create a new account"));
        }

        [Test]
        public void Test_HomePage_RegisterPageLink_OnPage()
        {
            // Arrange: go to the "Home" page
            driver.Navigate().GoToUrl(this.baseUrl);

            // Act: locate and click on [Register] on the main page
            driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Register'])[2]")).Click();

            // Assert user is redirected to the "Register" page
            Assert.AreEqual(this.baseUrl + "/Identity/Account/Register", driver.Url);
            Assert.That(driver.Title.Contains("Register"));
            Assert.That(driver.PageSource.Contains("Register"));
            Assert.That(driver.PageSource.Contains("Create a new account"));
        }

        [Test]
        public void Test_AllEventsPage_Anonymous()
        {
            // Arrange
            // Act: go to the "All Events" page

            driver.Navigate().GoToUrl(this.baseUrl + "/Events/All");

            // Assert user is redirected to the "Log in" page
            Assert.AreEqual(this.baseUrl + "/Identity/Account/LogIn?ReturnUrl=%2FEvents%2FAll", driver.Url);
            Assert.That(driver.Title.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Use a local account to log in"));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
            this.testEventuresApp.Dispose();
        }
    }
}