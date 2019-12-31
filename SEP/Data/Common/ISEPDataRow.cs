using System.Collections.Generic;

namespace SEP.Data.Common
{
    public interface ISEPDataRow
    {
        KeyValuePair<string, object> this[int index] { get; }
        object this[string key] { get; set; }

        Dictionary<string, object> Dictionary { get; }

        void Add(string name, object value);
    }
}