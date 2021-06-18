using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Eventures.SeleniumTests
{
    public class SeleniumTests_User
    {
        IWebDriver driver;
        string username = "pesho" + DateTime.UtcNow.Ticks;
        string password = "pass123123";

        [OneTimeSetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            this.driver.Manage().Window.Maximize();
        }

        [Test, Order(1)]
        public void Test_User_Register()
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

        [Test, Order(2)]
        public void Test_User_Login()
        {
            driver.Url = "https://localhost:44395/Identity/Account/Login";

            driver.FindElement(By.Id("Input_Username")).SendKeys(username);
            driver.FindElement(By.Id("Input_Password")).SendKeys(password);
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Log in')]")).Click();

            Assert.AreEqual("https://localhost:44395/", driver.Url);
            Assert.That(driver.PageSource.Contains($"Welcome, {username}"));
        }

        [Test, Order(3)]
        public void Test_User_Logout()
        {
            driver.Url = "https://localhost:44395/";
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Logout')]")).Click();
            Assert.AreEqual("https://localhost:44395/", driver.Url);
            Assert.That(driver.PageSource.Contains("Eventures: Events and Tickets"));
        }

        [Test]
        public void Test_HomePage_LoginLinks_Anonymous()
        {
            driver.Url = "https://localhost:44395/";

            driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Login'])[1]")).Click();
            Assert.AreEqual("https://localhost:44395/Identity/Account/Login", driver.Url);
            Assert.That(driver.Title.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Use a local account to log in"));

            driver.Url = "https://localhost:44395/";
            driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Login'])[2]")).Click();
            Assert.AreEqual("https://localhost:44395/Identity/Account/Login", driver.Url);
            Assert.That(driver.Title.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Use a local account to log in"));
        }

        [Test]
        public void Test_HomePage_RegisterLinks_Anonymous()
        {
            driver.Url = "https://localhost:44395/";

            driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Register'])[1]")).Click();
            Assert.AreEqual("https://localhost:44395/Identity/Account/Register", driver.Url);
            Assert.That(driver.Title.Contains("Register"));
            Assert.That(driver.PageSource.Contains("Register"));
            Assert.That(driver.PageSource.Contains("Create a new account"));

            driver.Url = "https://localhost:44395/";
            driver.FindElement(By.XPath("(//a[@href='/Identity/Account/Register'])[2]")).Click();
            Assert.AreEqual("https://localhost:44395/Identity/Account/Register", driver.Url);
            Assert.That(driver.Title.Contains("Register"));
            Assert.That(driver.PageSource.Contains("Register"));
            Assert.That(driver.PageSource.Contains("Create a new account"));
        }

        [Test]
        public void Test_AllEventsPage_Anonymous()
        {
            driver.Url = "https://localhost:44395/Events/All";
            Assert.AreEqual("https://localhost:44395/Identity/Account/LogIn?ReturnUrl=%2FEvents%2FAll", driver.Url);
            Assert.That(driver.Title.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Log in"));
            Assert.That(driver.PageSource.Contains("Use a local account to log in"));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}