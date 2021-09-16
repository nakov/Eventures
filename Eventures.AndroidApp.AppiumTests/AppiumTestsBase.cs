using System;
using System.IO;

using Eventures.Data;
using Eventures.WebAPI;
using Eventures.Tests.Common;

using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Support.UI;

namespace Eventures.AndroidApp.AppiumTests
{
    public class AppiumTestsBase
    {
        protected TestDb testDb;
        protected ApplicationDbContext dbContext;
        private TestEventuresApp<Startup> testEventuresApp;
        protected string baseUrl;
        private AppiumLocalService appiumLocalService;
        private string ApiPath = @"../../../../Eventures.WebAPI";
        private string AppPath = @"../../../../Eventures.AndroidApp/app/build/outputs/apk/debug/eventures.apk";
        protected AndroidDriver<AndroidElement> driver;
        protected WebDriverWait wait;

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            this.testDb = new TestDb();
            this.dbContext = testDb.CreateDbContext();
            this.testEventuresApp = new TestEventuresApp<Startup>(testDb, ApiPath);

            // Android Emulator accesses the server's localhost through "10.0.2.2"
            this.baseUrl = this.testEventuresApp.ServerUri.Replace("127.0.0.1", "10.0.2.2");

            // Initialize Appium Local Service to start the Appium server automatically
            appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            appiumLocalService.Start();

            var appiumOptions = new AppiumOptions() { PlatformName = "Android" };
            var fullPathName = Path.GetFullPath(AppPath);
            appiumOptions.AddAdditionalCapability("app", fullPathName);

            // Initialize the Windows driver with Appium local service and options
            driver = new AndroidDriver<AndroidElement>(
                appiumLocalService, appiumOptions);

            // Set an implicit wait for the UI interaction
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Set an explicit wait for the UI interaction
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            // Close the app and quit the driver
            driver.CloseApp();
            driver.Quit();

            // Dispose of the Appium Local Service
            appiumLocalService.Dispose();

            // Stop and dispose the local Web API server
            this.testEventuresApp.Dispose();
        }
    }
}