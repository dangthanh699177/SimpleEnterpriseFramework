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

            builder.RegisterType<App>().As<IApp>();
            builder.RegisterType<FormMain>().As<IFormMain>();

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(SEP)))
                .Where(t => t.Namespace.Contains("Client"))
                .Where(t => t.Namespace.Contains("Common"))
                .Where(t => t.Namespace.Contains("Utilities"))
                .Where(t => t.Namespace.Contains("Forms"))
                .Where(t => t.Namespace.Contains("Authentication"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == ("I" + t.Name)));

            return builder.Build();
        }
    }
}
