using NUnit.Framework;
using System;
using System.Threading;

namespace PublicApi
{
    public class AppManager
    {
        public Entries etalonData;
        public Exception ex;
        public string baseUrl = "https://api.publicapis.org/entries";
        
        private static ThreadLocal<AppManager> app = new ThreadLocal<AppManager>();
        public AppManager()
        {
            Cmhelp = new ComnHelper(this);

            etalonData = Cmhelp.GetJsonFromUrl(baseUrl);
            if (etalonData == null)
                Assert.Fail("EtalonData is null.");
        }
        public ComnHelper Cmhelp { get; }
        public static AppManager GetInstance()
        {
            if (!app.IsValueCreated)
            {
                AppManager newInstance = new AppManager();
                app.Value = newInstance;
            }
            return app.Value;
        }
    }
}