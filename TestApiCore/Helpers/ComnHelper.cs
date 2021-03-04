using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace PublicApi
{
    public class ComnHelper : HelperBase
    {
        public ComnHelper(AppManager app) : base(app) { }
        internal string CreateUrl(string filter)
        {
            if (!filter.Contains("="))
                return null;
            else
                return app.baseUrl + "?" + filter;
        }
        internal Entries GetJsonFromUrl(string url)
        {
            Entries parsedData = new Entries() { entries = new List<Entry>() };
            using (var wc = new WebClient())
            {
                try
                {
                    var response = wc.DownloadString(url);
                    parsedData = JsonConvert.DeserializeObject<Entries>(response);
                    if (parsedData.entries == null)
                        parsedData.entries = new List<Entry>();
                }
                catch (Exception e)
                {
                    app.ex = e;
                    return null;
                }
                return parsedData;
            }
        }

        internal void SortEntries(ref Entries unsorted)
        {
            if (unsorted.count > 0)
                unsorted.entries.Sort((x, y) => x.Link.CompareTo(y.Link));
        }

        internal Entries FilterApply(Entries rawData, string filters)
        {
            var filterArray = filters.Split('&');

            foreach (var filter in filterArray)
            {
                FilterRawData(filter, ref rawData);
            }

            return rawData;
        }

        private void FilterRawData(string filter, ref Entries rawData)
        {
            var tmp = new Entries { entries = new List<Entry>() };
            for (var i = 0; i < rawData.entries.Count; i++)
            {
                var el = rawData.entries[i];
                if (Condition(filter, el))
                    tmp.entries.Add(el);
            }

            tmp.count = tmp.entries.Count;
            rawData = tmp;
        }

        private bool Condition(string filter, Entry el)
        {
            bool https = false;
            string value = "";
            string field;
            if (filter.ToLower().Contains("https="))
            {
                if (filter.Split('=')[1] != "true" && filter.Split('=')[1] != "false")
                    return false;
                if (filter.Split('=')[1] == "true")
                    https = true;
                else
                    https = false;

                field = "https";
            }
            else
            {
                field = filter.Split('=')[0].ToLower();
                value = filter.Split('=')[1].ToLower();
            }

            switch (field)
            {
                case "api":
                    return el.API.ToLower() == value;
                case "description":
                    return el.Description.ToLower() == value;
                case "auth":
                    return el.Auth.ToLower() == value;
                case "https":
                    return el.HTTPS == https;
                case "cors":
                    return el.Cors.ToLower() == value;
                case "link":
                    return el.Link.ToLower() == value;
                case "category":
                    return el.Category.ToLower() == value;
                default:
                    return false;
            }
        }
    }
}