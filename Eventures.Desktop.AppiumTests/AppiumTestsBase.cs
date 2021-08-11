
using NUnit.Framework;
using Eventures.Data;
using Eventures.Tests.Common;
using Eventures.WebAPI;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System.IO;
using System;
using OpenQA.Selenium.Appium.Service.Options;
using System.Collections.Generic;

namespace Eventures.Desktop.AppiumTests
{
    public class AppiumTestsBase
    {
        protected TestDb testDb;
        protected ApplicationDbContext dbContext;
        private TestEventuresApp<Startup> testEventuresApp;
        protected string baseUrl;
        private AppiumLocalService appiumLocalService;
        private string AppPath = @"../../../../Eventures.Desktop/bin/Debug/net5.0-windows/Eventures.Desktop.exe";
        protected WindowsDriver<WindowsElement> driver;

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            this.testDb = new TestDb();
            this.dbContext = testDb.CreateDbContext();
            this.testEventuresApp = new TestEventuresApp<Startup>(
                testDb, "../../../../Eventures.WebAPI");
            this.baseUrl = this.testEventuresApp.ServerUri;

            // Initialize Appium Local Service to start the Appium server automatically
            appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            appiumLocalService.Start();

            var appiumOptions = new AppiumOptions() { PlatformName = "Windows" };
            var fullPathName = Path.GetFullPath(AppPath);
            appiumOptions.AddAdditionalCapability("app", fullPathName);

            // Initialize the Windows driver with Appium local service and options
            driver = new WindowsDriver<WindowsElement>(
                appiumLocalService, appiumOptions);
            
            // Set an implicit wait for the UI interaction
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
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
