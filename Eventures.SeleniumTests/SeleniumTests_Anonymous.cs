using Eventures.Tests.Common;
using Eventures.UnitTests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Eventures.SeleniumTests
{
    public class SeleniumTests_Anonymous
    {
        TestDb testDb;
        IWebDriver driver;
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
