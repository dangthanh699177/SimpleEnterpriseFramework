using SEP.Data.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SEP.Data.Common
{
    public class SEPDataAdapter : ISEPDataAdapter
    {
        private ISEPDataProvider sepDP = null;
        private DbConnection dbConn = null;
        private string query = string.Empty;

        public SEPDataAdapter(ISEPDataProvider sepDP)
        {
            this.sepDP = sepDP;
        }
        public SEPDataAdapter(ISEPConnection sepConn, ISEPDataProvider sepDP)
        {
            this.sepDP = sepDP;
            this.dbConn = sepConn.CreateConnection(sepDP);
        }
        public SEPDataAdapter(string query, ISEPConnection sepConn, ISEPDataProvider sepDP)
        {
            this.query = query;
            this.sepDP = sepDP;
            this.dbConn = sepConn.CreateConnection(sepDP);
        }
        
        public DbDataAdapter CreateDataAdapter()
        {
            return this.sepDP == null
                ? throw new System.Exception("SEPDataProvider is not instantialize!")
                : this.sepDP.Factory().CreateDataAdapter();
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
            cmd.CommandText = this.query;

            DataTable table = new DataTable();
            DbDataAdapter adapter = this.CreateDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            dbConn.Close();
            return table;
        }
        
    }
}
