using SEP.Data.Common;

namespace SEP.Data.Utilities
{
    public interface ISQLQuery
    {
        string Delete();
        string Delete(ISEPDataRow sepRow);
        string Insert(ISEPDataRow sepRow);
        string Select();
        string Update(ISEPDataRow sepRow);
    }
}