using System;
using System.Net.Http;
using System.Text.RegularExpressions;

using NUnit.Framework;

using Eventures.Tests.Common;
using Eventures.Data;

namespace Eventures.WebApp.IntegrationTests
{
    public class WebIntegrationTestsBase
    {
        protected TestDb testDb;
        protected TestEventuresApp<Startup> testEventuresApp;
        protected HttpClient httpClient;
        protected ApplicationDbContext dbContext;

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            this.testDb = new TestDb();
            this.testEventuresApp = new TestEventuresApp<Startup>(
                testDb, "../../../../Eventures.WebApp");
            this.dbContext = this.testDb.CreateDbContext();
            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri(this.testEventuresApp.ServerUri)
            };
        }

        protected static string ExtractAntiForgeryToken(string htmlResponseText)
        {
            Match match = Regex.Match(htmlResponseText, @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            // Stop and dispose the local Web server
            this.testEventuresApp.Dispose();
        }
    }
}
