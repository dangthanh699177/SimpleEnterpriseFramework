using SEP.Data.Common;
using System.Data.Common;

namespace SEP.Data.Client
{
    public interface ISEPConnection
    {
        DbConnection CreateConnection(ISEPDataProvider sepDataProvider);
        void SetPath(string newPath);
    }
}