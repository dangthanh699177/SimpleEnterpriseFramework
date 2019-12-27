using Microsoft.Data.ConnectionUI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEP.Data.Common
{
    public class SEPConnection
    {
        private string path;

        public SEPConnection(string path)
        {
            this.path = path;
        }

        public DbConnection CreateConnection()
        {
            if (path == String.Empty || path == null)
            {
                throw new Exception("string DataConnectionDialog.ConnectionString is not right!");
            }
            DbConnection dbConn = SEPDataProvider.Factory().CreateConnection();
            dbConn.ConnectionString = path;
            return dbConn;
        }
    }
}
