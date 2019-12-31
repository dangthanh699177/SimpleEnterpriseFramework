using System.Data.Common;

namespace SEP.Data.Common
{
    public interface ISEPDataProvider
    {
        DbProviderFactory Factory();
        void SetName(string dataProviderName);
    }
}