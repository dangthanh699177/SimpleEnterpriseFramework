
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
    public class SEPCommand
    {
        private string cmdText;
        private DbConnection dbConn;

        public SEPCommand()
        {
            this.cmdText = string.Empty;
            this.dbConn = null;
        }

        public SEPCommand(string commandText, SEPConnection connection)
        {
            this.cmdText = commandText;
            this.dbConn = connection.CreateConnection();
        }
        public async Task<int> Insert()
        {
            using (DbCommand cmd = this.dbConn.CreateCommand())
            {
                if (dbConn.State == ConnectionState.Closed)
                {
                    this.dbConn.Open();
                }

                cmd.CommandText = this.cmdText;

                DbDataAdapter adapter = new SEPDataAdapter().CreateDataAdapter();
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

                cmd.CommandText = this.cmdText;

                DbDataAdapter adapter = new SEPDataAdapter().CreateDataAdapter();
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

                cmd.CommandText = this.cmdText;

                DbDataAdapter adapter = new SEPDataAdapter().CreateDataAdapter();
                adapter.DeleteCommand = cmd;
                int x = await adapter.DeleteCommand.ExecuteNonQueryAsync();

                this.dbConn.Close();
                return x;
            }
        }
        
    }
}
