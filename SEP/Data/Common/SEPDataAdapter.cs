using SEP.Data.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SEP.Data.Common
{
    public class SEPDataAdapter : ISEPDataAdapter
    {
        private static ISEPDataProvider provider;
        private static SEPDataAdapter instance;

        private SEPDataAdapter(ISEPDataProvider sepProvider)
        {
            provider = sepProvider;
        }

        public static SEPDataAdapter Instance(ISEPDataProvider sepProvider)
        {
            if (instance == null)
            {
                instance = new SEPDataAdapter(sepProvider);
            }
            return instance;
        }
        
        public DbDataAdapter CreateDataAdapter()
        {
            return provider == null
                ? throw new System.Exception("SEPDataProvider is not instantialize!")
                : provider.Factory().CreateDataAdapter();
        }

    }
}
