
using NUnit.Framework;
using Eventures.Data;
using Eventures.Tests.Common;
using Eventures.WebAPI;

namespace Eventures.Desktop.AppiumTests
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
        
        [OneTimeTearDown]
        public void TearDown()
        {
            this.testEventuresApp.Dispose();
        }
    }
}
