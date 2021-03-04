using NUnit.Framework;
using PublicApi.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicApi.Tests
{
    [TestFixture]
    class Tests : TestBase
    {
        [Test, Order(1)]
        public void P_Cat_Animals_Https_true()
        {
            var filter = "Category=Animals&https=true";
            var url = app.Cmhelp.CreateUrl(filter);
            var fact = app.Cmhelp.GetJsonFromUrl(url);
            if (fact == null)
                Assert.Fail($"There is a failure during parsing the answer. Message: '{app.ex.Message}'.");
            Entries filteredEtalon = new Entries() { entries = new List<Entry>() };
            try
            {
                filteredEtalon = app.Cmhelp.FilterApply(app.etalonData, filter);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unable to get data with filter '{filter}'. EtalonData.entries.Count='{app.etalonData.entries.Count}'. Message: '{ex.Message}'. StackTrace: '{ex.StackTrace}'.");
            }

            app.Cmhelp.SortEntries(ref filteredEtalon);
            app.Cmhelp.SortEntries(ref fact);
            CollectionAssert.AreEqual(fact.entries, filteredEtalon.entries, new CollectionComparer(), $"For the filter '{filter}' fact and etalon are not the same.");
        }
        [Test, Order(2)]
        public void N_Cat_Animals_NonexistentFilter()
        {
            var filter = "Category=Animals&asd=true";
            var url = app.Cmhelp.CreateUrl(filter);
            var fact = app.Cmhelp.GetJsonFromUrl(url);
            Assert.IsNull(fact);
            Assert.True(app.ex.Message.Contains("Bad Request"), $"There is no 'Bad Request' in the response with wrong filter. app.ex.Message: '{app.ex.Message}'");
        }
    }
}
