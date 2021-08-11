using NUnit.Framework;

using Eventures.Data;
using Eventures.Tests.Common;

namespace Eventures.WebApp.UnitTests
{
    public class UnitTestsBase
    {
        protected TestDb testDb;
        protected ApplicationDbContext dbContext;

        [OneTimeSetUp]
        public void OneTimeSetupBase()
        {
            testDb = new TestDb();
            dbContext = testDb.CreateDbContext();
        }
    }
}
