using Autofac;
using SEP.Forms;
using System.Linq;
using System.Reflection;

namespace Demo
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<FormMain>().As<IFormMain>();

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(SEP)))
                .Where(t => t.Namespace.Contains("Client"))
                .Where(t => t.Namespace.Contains("Common"))
                .Where(t => t.Namespace.Contains("Utilities"))
                .Where(t => t.Namespace.Contains("Forms"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == ("I" + t.Name)));

            return builder.Build();
        }
    }
}
