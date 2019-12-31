using Autofac;
using SEP.Forms;
using System.Linq;
using System.Reflection;

namespace SEP
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FormMain>().As<IFormMain>();

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(SEP)))
                .Where(t => t.Namespace.Contains("Data"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == ("I" + t.Name)));

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(SEP)))
                .Where(t => t.Namespace.Contains("Data"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == ("I" + t.Name)));

            return builder.Build();
        }
    }
}
