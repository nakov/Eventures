using System.Net.Http;

using NUnit.Framework;

using System.Threading.Tasks;
using System.Net.Http.Headers;

using Eventures.Data;
using Eventures.Tests.Common;
using Eventures.WebAPI.Models;
using System;

namespace Eventures.WebAPI.IntegrationTests
{
    public class AppiumTestsBase
    {
        protected TestDb testDb;
        protected ApplicationDbContext dbContext;
        protected TestEventuresApp<Startup> testEventuresApp;
        protected string baseUrl;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.testDb = new TestDb();
            this.dbContext = testDb.CreateDbContext();
            this.testEventuresApp = new TestEventuresApp<Startup>(
                testDb, "../../../../Eventures.WebAPI");
            this.baseUrl = this.testEventuresApp.ServerUri;
        }
    }
}
