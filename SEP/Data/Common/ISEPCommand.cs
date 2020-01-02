using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SEP.Data.Common
{
    public interface ISEPCommand
    {
        Task<int> Delete(string commandText);
        List<ISEPDataRow> ExecuteReader(string commandText);
        List<string> GetListTableName();
        DataTable GetTable(string commandText);
        Task<int> Insert(string commandText);
        Task<int> Update(string commandText);
        bool CheckExistsTable(string commandText);
    }
}