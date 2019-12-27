using System.Collections.Generic;
using System.Text;

namespace SEP.Data.Common
{
    public class SEPParameters
    {
        private class Condition
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public Condition(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }
        }

        // 2 function below is to use for Insert Command:
        // insert into table_name (...) values (...)

        /// <summary>
        /// Retrieve list name of propertys and convert it to string type
        /// </summary>
        public static string GetListPropertyName(SEPDataRow dRow)
        {
            StringBuilder propertyNames = new StringBuilder();

            foreach(KeyValuePair<string, object> item in dRow.Dictionary)
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
        public static string GetListValue(SEPDataRow dRow)
        {
            StringBuilder values = new StringBuilder();

            foreach (KeyValuePair<string, object> item in dRow.Dictionary)
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

        private static Condition CheckID(SEPDataRow dRow)
        {
            foreach (KeyValuePair<string, object> item in dRow.Dictionary)
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

        private static string GetIDKey(SEPDataRow dRow) => CheckID(dRow).Name;

        private static string GetIDValue(SEPDataRow dRow) => CheckID(dRow).Value;

        private static string GetListCondition(SEPDataRow dRow)
        {
            StringBuilder lCon = new StringBuilder();

            foreach(KeyValuePair<string, object> item in dRow.Dictionary)
            {
                if (item.Key.GetType() == typeof(string))
                    lCon.Append($"{item.Key} = N'{item.Value}'");
                else
                    lCon.Append($"{item.Key} = {item.Value}");

                lCon.Append(" and ");
            }

            return lCon.Remove(lCon.Length - 5, 5).ToString();
        }

        public static string GetCondition(SEPDataRow dRow)
            => CheckID(dRow) != null
                ? $"{CheckID(dRow).Name} = {CheckID(dRow).Value}"
                : GetListCondition(dRow);

        // function below is use for command:
        // update table_name set <GetEntity()> where ...
        public static string GetEntity(SEPDataRow dRow)
        {
            StringBuilder entity = new StringBuilder();

            foreach(KeyValuePair<string, object> item in dRow.Dictionary)
            {
                if (item.Key == "id" || item.Key == "iD" || item.Key == "Id" || item.Key == "ID")
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
