using SEP.Authentication;
using SEP.Data.Common;

namespace SEP.Data.Utilities
{
    public interface IQuery
    {
        string CreateTable(string tbName);
        string Delete(string tbName);
        string Delete(string tbName, ISEPDataRow sepRow);
        string Insert(string tbName, ISEPDataRow sepRow);
        string Insert(string tbName, UserAccount u);
        string Select(string tbName);
        string Update(string tbName, ISEPDataRow sepRow);
    }
}