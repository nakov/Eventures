using NUnit.Framework;

using Eventures.Data;
using Eventures.Tests.Common;

namespace Eventures.App.UnitTests
{
    public class UnitTestsBase
    {
        protected TestDb testDb;
        protected ApplicationDbContext dbContext;

        [OneTimeSetUp]
        public void Setup()
        {
            testDb = new TestDb();
            dbContext = testDb.CreateDbContext();
        }
    }
}
