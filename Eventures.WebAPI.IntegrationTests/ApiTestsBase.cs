using System.Net.Http;

using NUnit.Framework;

using Eventures.Data;
using Eventures.Tests.Common;

namespace Eventures.WebAPI.IntegrationTests
{
    public class ApiTestsBase
    {
        protected TestDb testDb;
        protected ApplicationDbContext dbContext;
        protected TestingWebApiFactory testFactory;
        protected HttpClient httpClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.testDb = new TestDb();
            this.dbContext = testDb.CreateDbContext();
            this.testFactory = new TestingWebApiFactory(testDb);
            this.httpClient = testFactory.Client;
        }
    }
}
