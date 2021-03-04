using System.Collections;
using System.Collections.Generic;

namespace PublicApi
{
    public class Entry
    {
        public string API { get; set; }
        public string Description { get; set; }
        public string Auth { get; set; }
        public bool HTTPS { get; set; }
        public string Cors { get; set; }
        public string Link { get; set; }
        public string Category { get; set; }
    }

    public class Entries
    {
        public int count { get; set; }
        public List<Entry> entries { get; set; }
    }
    public class CollectionComparer : IComparer, IComparer<Entry>
    {
        public int Compare(Entry x, Entry y)
        {
            if (x == null || y == null) return -1;
            return x.Link == y.Link ? 0 : -1;
        }

        public int Compare(object x, object y)
        {
            var modelX = x as Entry;
            var modelY = y as Entry;
            return Compare(modelX, modelY);
        }
    }
}
