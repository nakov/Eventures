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

        [SetUp]
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

        [Test]
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

        [Test]
        public void Test_User_Login()
        {
            RegisterUser();
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

        [Test]
        public void Test_User_Logout()
        {
            RegisterUser();
            // Arrange: go to the "Home" page
            driver.Navigate().GoToUrl(this.baseUrl);

            // Locate and click on the "Logout" button
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Logout')]")).Click();

            // Assert user is redirected to the "Home" page and is logged out
            Assert.AreEqual(this.baseUrl + "/", driver.Url);
            Assert.That(driver.PageSource.Contains("Eventures: Events and Tickets"));
        }

        private void RegisterUser()
        {
            driver.Navigate().GoToUrl(this.baseUrl + "/Identity/Account/Register");

            username = "testuser" + DateTime.UtcNow.Ticks;
            password = "password" + DateTime.UtcNow.Ticks;
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

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            this.testEventuresApp.Dispose();
        }
    }
}