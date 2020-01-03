using System.Collections.Generic;

namespace SEP.Data.Utilities
{
    public interface IQueryHelper
    {
        Condition GetID();
        bool CheckID(KeyValuePair<string, object> item);
        string GetCondition();
        string GetEntity();
        string GetAllCondition();
        string GetListPropertyName();
        string GetListValue();
    }
}