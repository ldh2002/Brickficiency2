using Autofac;
using System.Linq;
using System.Reflection;

namespace WindmillHelix.Brickficiency2.ExternalApi.DependencyInjection
{
    public class ExternalApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(f => f.Name.EndsWith("Service") || f.Name.EndsWith("Factory") || f.Name.EndsWith("Api"))
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}