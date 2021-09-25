using Eventures.Data;
using Eventures.Tests.Common;

using NUnit.Framework;

namespace Eventures.WebApp.UnitTests
{
    public class UnitTestsBase
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

