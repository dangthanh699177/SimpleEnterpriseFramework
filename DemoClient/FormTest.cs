using System;
using SEP.Forms;
using Autofac;
using System.Windows.Forms;

namespace DemoClient
{
    public partial class FormTest
    {
        public FormTest()
        {
            Load();
        }

        private new void Load()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<IFormMain>().Run();
            }

        }
    }
}
