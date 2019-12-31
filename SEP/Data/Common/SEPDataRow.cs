using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEP.Data.Common
{
    /// <summary>
    /// EntitySet mean: a table
    /// </summary>
    public class SEPDataRow : ISEPDataRow
    {
        private Dictionary<string, object> fields = null;
        
        public SEPDataRow()
        {
            this.fields = new Dictionary<string, object>();
        }
        public SEPDataRow(Dictionary<string, object> dict)
        {
            this.fields = dict;
        }
        public SEPDataRow(DataGridView dgvTable)
        {
            this.fields = new Dictionary<string, object>();

            foreach (DataGridViewColumn col in dgvTable.Columns)
            {
                object value = dgvTable.CurrentRow.Cells[col.Index].Value;
                this.fields.Add(col.Name, value);
            }
        }
        
        /// <summary>
        /// Retrieve datatype based on key = propertyName
        /// </summary>
        public object this[string key]
        {
            get
            {
                if (fields.ContainsKey(key))
                    return fields[key];
                return null;
            }
            set
            {
                if (fields.ContainsKey(key))
                    fields[key] = value;
                else
                    fields.Add(key, value);
            }
        }

        /// <summary>
        /// Retrieve item in Dictionary according to index
        /// </summary>
        public KeyValuePair<string, object> this[int index]
        {
            get
            {
                if (index >= fields.Count())
                    throw new IndexOutOfRangeException();
                return fields.ElementAt(index);
            }
        }

        public Dictionary<string, object> Dictionary => this.fields;

        public void Add(string name, object value)
        {
            this.fields.Add(name, value);
        }
    }
}
