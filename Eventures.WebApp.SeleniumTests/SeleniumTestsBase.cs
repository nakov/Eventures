﻿using System;
using System.Diagnostics;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using Eventures.Tests.Common;

namespace Eventures.WebApp.SeleniumTests
{
    public abstract class SeleniumTestsBase
    {
        protected TestDb testDb;
        protected IWebDriver driver;
        protected TestEventuresApp<Startup> testEventuresApp;
        protected string baseUrl;
        protected string username = "testuser" + DateTime.UtcNow.Ticks;
        protected string password = "password" + DateTime.UtcNow.Ticks;

        [OneTimeSetUp]
        public void OneTimeSetupBase()
        {
            // Run the Web app in a local Web server
            this.testDb = new TestDb();
            this.testEventuresApp = new TestEventuresApp<Startup>(
                testDb, "../../../../Eventures.WebApp");
            this.baseUrl = this.testEventuresApp.ServerUri;

            // Setup the ChromeDriver
            var chromeOptions = new ChromeOptions();
            if (! Debugger.IsAttached)
                chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("--start-maximized");
            this.driver = new ChromeDriver(chromeOptions);

            // Set an implicit wait for the UI interaction
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            // Stop and dispose the Selenium driver
            driver.Quit();

            // Stop and dispose the local Web server
            this.testEventuresApp.Dispose();
        }
    }
}