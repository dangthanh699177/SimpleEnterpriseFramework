using SEP.Data.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SEP.Data.Common
{
    public class SEPCommand : ISEPCommand
    {
        private string query = String.Empty;
        private DbConnection dbConn = null;
        private ISEPDataProvider sepDP = null;

        public SEPCommand() { }

        public SEPCommand(string query, ISEPConnection sepConn, ISEPDataProvider sepDP)
        {
            this.query = query;
            this.sepDP = sepDP;
            this.dbConn = sepConn.CreateConnection(sepDP);
        }
        public async Task<int> Insert()
        {
            using (DbCommand cmd = this.dbConn.CreateCommand())
            {
                if (dbConn.State == ConnectionState.Closed)
                {
                    this.dbConn.Open();
                }

                cmd.CommandText = this.query;

                DbDataAdapter adapter = new SEPDataAdapter(this.sepDP).CreateDataAdapter();
                adapter.InsertCommand = cmd;
                int x = await adapter.InsertCommand.ExecuteNonQueryAsync();

                this.dbConn.Close();
                return x;
            }
        }
        public async Task<int> Update()
        {
            using (DbCommand cmd = this.dbConn.CreateCommand())
            {
                if (dbConn.State == ConnectionState.Closed)
                {
                    this.dbConn.Open();
                }

                cmd.CommandText = this.query;
                DbDataAdapter adapter = new SEPDataAdapter(this.sepDP).CreateDataAdapter();
                adapter.UpdateCommand = cmd;
                int x = await adapter.UpdateCommand.ExecuteNonQueryAsync();

                this.dbConn.Close();
                return x;
            }
        }
        public async Task<int> Delete()
        {
            using (DbCommand cmd = this.dbConn.CreateCommand())
            {
                if (dbConn.State == ConnectionState.Closed)
                {
                    this.dbConn.Open();
                }

                cmd.CommandText = this.query;

                DbDataAdapter adapter = new SEPDataAdapter(this.sepDP).CreateDataAdapter();
                adapter.DeleteCommand = cmd;
                int x = await adapter.DeleteCommand.ExecuteNonQueryAsync();

                this.dbConn.Close();
                return x;
            }
        }
        
    }
}
