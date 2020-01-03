using SEP.Data.Common;
using System.Collections.Generic;
using System.Text;

namespace SEP.Data.Utilities
{
    public class QueryHelper : IQueryHelper
    {
        private ISEPDataRow sepRow = null;

        public QueryHelper() { }
        public QueryHelper(ISEPDataRow sepRow)
        {
            this.sepRow = sepRow;
        }
        
        // 2 function below is to use for Insert Command:
        // insert into table_name (...) values (...)

        /// <summary>
        /// Retrieve list name of propertys and convert it to string type
        /// </summary>
        public string GetListPropertyName()
        {
            StringBuilder propertyNames = new StringBuilder();

            foreach(KeyValuePair<string, object> item in this.sepRow.Dictionary)
            {
                if (item.Key == "id" || item.Key == "iD" || item.Key == "Id" || item.Key == "ID")
                {
                    continue;
                }

                propertyNames.Append(item.Key);
                propertyNames.Append(", ");
            }

            return propertyNames.Remove(propertyNames.Length - 2, 2).ToString();
        }

        /// <summary>
        /// Retrieve list value of propertys and convert it to string type
        /// </summary>
        public string GetListValue()
        {
            StringBuilder values = new StringBuilder();

            foreach (KeyValuePair<string, object> item in this.sepRow.Dictionary)
            {
                if (item.Key == "id" || item.Key == "iD" || item.Key == "Id" || item.Key == "ID")
                {
                    continue;
                }

                if (item.Value.GetType() == typeof(string))
                    values.Append($"N'{item.Value}'");
                else
                    values.Append($"{item.Value}");

                values.Append(", ");
            }
            
            return values.Remove(values.Length - 2, 2).ToString();
        }

        // function below is use for condition "Where ..." Command

        public Condition GetID()
        {
            foreach (KeyValuePair<string, object> item in this.sepRow.Dictionary)
            {
                if (item.Key == "ID" ||
                    item.Key == "Id" ||
                    item.Key == "iD" ||
                    item.Key == "id")
                {
                    return new Condition(item.Key, item.Value.ToString());
                }
            }
            return null;
        }

        public bool CheckID(KeyValuePair<string, object> item)
        {
            return item.Key == "ID" || item.Key == "Id" || item.Key == "iD" || item.Key == "id"
                ? true : false;
        }

        public string GetAllCondition()
        {
            StringBuilder lCon = new StringBuilder();

            foreach(KeyValuePair<string, object> item in this.sepRow.Dictionary)
            {
                if (item.Key.GetType() == typeof(string))
                    lCon.Append($"{item.Key} = N'{item.Value}'");
                else
                    lCon.Append($"{item.Key} = {item.Value}");

                lCon.Append(" and ");
            }

            return lCon.Remove(lCon.Length - 5, 5).ToString();
        }

        public string GetCondition()
            => this.GetID() != null
                ? $"{this.GetID().Name} = {this.GetID().Value}"
                : this.GetAllCondition();

        // function below is use for command:
        // update table_name set <GetEntity()> where ...
        public string GetEntity()
        {
            StringBuilder entity = new StringBuilder();

            foreach(KeyValuePair<string, object> item in this.sepRow.Dictionary)
            {
                if (this.CheckID(item))
                {
                    continue;
                }

                object value = item.Value.GetType() == typeof(string)
                    ? $"N'{item.Value}'"
                    : item.Value;
                entity.Append($"{item.Key} = {value}, ");
            }
            return entity.Remove(entity.Length - 2, 2).ToString();
        }
    }
}
