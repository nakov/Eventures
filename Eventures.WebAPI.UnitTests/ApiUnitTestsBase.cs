using Eventures.Data;
using Eventures.Tests.Common;

using NUnit.Framework;

namespace Eventures.WebAPI.UnitTests
{
    public class ApiUnitTestsBase
    {
        protected TestDb testDb;
        protected ApplicationDbContext dbContext;

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            // Instantiate the testing db with a db context
            this.testDb = new TestDb();
            this.dbContext = testDb.CreateDbContext();
        }
    }
}
