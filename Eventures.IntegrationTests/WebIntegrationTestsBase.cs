using System;
using System.Net.Http;

using NUnit.Framework;

using Eventures.Tests.Common;
using System.Text.RegularExpressions;

namespace Eventures.IntegrationTests
{
    public class WebIntegrationTestsBase
    {
        protected TestDb testDb;
        protected TestEventuresApp testEventuresApp;
        protected HttpClient httpClient;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.testDb = new TestDb();
            this.testEventuresApp = new TestEventuresApp(testDb);
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
        public void TearDown()
        {
            this.testEventuresApp.Dispose();
        }
    }
}
