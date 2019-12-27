using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEP.Data.Common
{
    public class SEPDataAdapter
    {
        private DbConnection dbConn;
        private string cmdText;

        public SEPDataAdapter()
        {
            this.cmdText = string.Empty;
            this.dbConn = null;
        }

        public SEPDataAdapter(SEPConnection connection)
        {
            this.cmdText = string.Empty;
            this.dbConn = connection.CreateConnection();
        }

        public SEPDataAdapter(string commandText, SEPConnection connection)
        {
            this.cmdText = commandText;
            this.dbConn = connection.CreateConnection();
        }
        
        public DbDataAdapter CreateDataAdapter()
        {
            return SEPDataProvider.Factory().CreateDataAdapter();
        }

        public List<string> GetListTableName()
        {
            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            DataTable dataTable = dbConn.GetSchema("Tables");
            List<string> listName = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                listName.Add((string)row[2]);
            }

            dbConn.Close();
            return listName;
        }

        public DataTable GetTable()
        {
            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            DbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = this.cmdText;

            DataTable table = new DataTable();
            DbDataAdapter adapter = this.CreateDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            dbConn.Close();
            return table;
        }
        
    }
}
