using Newtonsoft.Json;
using NUnit.Framework;
using PublicApi.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using TestApiCore;
using TestApiCore.Models;

namespace PublicApi.Tests
{
    [TestFixture]
    public class Tests : TestBase
    {
        public static string[] FromJson()
        {
            var tmp = new StreamReader($"{Settings.FilePath}\\\\TestDataSimple.json").ReadToEnd();
            var root = JsonConvert.DeserializeObject<FilterJsonSimple>(tmp);
            return root.RawFilters.ToArray();
        }

        [Test, TestCaseSource(nameof(FromJson))]
        public void CheckFilterResponse(string filter)
        {
            var url = app.Cmhelp.CreateUrl(filter);
            if (url == null)
                Assert.Fail($"Current filter - '{filter}' - doesn't valid.");
            var factFromApi = app.Cmhelp.GetJsonFromUrl(url);
            if (factFromApi == null)
            {
                Assert.True(app.ex.Message.Contains("Bad Request"), 
                    $"There is no 'Bad Request' in the response with wrong filter. app.ex.Message: '{app.ex.Message}'");
                return;
            }

            Entries filteredEtalon = GetFilteredDataFromEtalon(filter);

            app.Cmhelp.SortEntries(ref filteredEtalon);
            app.Cmhelp.SortEntries(ref factFromApi);
            CollectionAssert.AreEqual(factFromApi.entries, filteredEtalon.entries, new CollectionComparer(), 
                $"For the filter '{filter}' fact and etalon are not the same.");
        }

        private Entries GetFilteredDataFromEtalon(string filter)
        {
            Entries filteredEtalon = new Entries() { entries = new List<Entry>() };
            try
            {
                filteredEtalon = app.Cmhelp.FilterApply(app.etalonData, filter);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unable to get data with filter '{filter}'. " +
                    $"EtalonData.entries.Count='{app.etalonData.entries.Count}'. " +
                    $"Message: '{ex.Message}'. StackTrace: '{ex.StackTrace}'.");
            }

            return filteredEtalon;
        }
    }
}
