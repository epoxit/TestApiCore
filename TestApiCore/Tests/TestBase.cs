using NUnit.Framework;

namespace PublicApi.Helpers
{
    [TestFixture]
    public class TestBase
    {
        public AppManager app;
        
        [OneTimeSetUp]
        public void SetupTest()
        {
            app = AppManager.GetInstance();            
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}
