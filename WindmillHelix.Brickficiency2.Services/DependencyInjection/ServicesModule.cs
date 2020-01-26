using Autofac;
using System.Linq;
using System.Reflection;
using WindmillHelix.Brickficiency2.Services.Calculator;

namespace WindmillHelix.Brickficiency2.Services.DependencyInjection
{
    public class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CalculatorService>().AsSelf();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(f => f.Name.EndsWith("Service") || f.Name.EndsWith("Factory"))
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(f => f.Name.EndsWith("StepRunner"))
                   .AsSelf();
        }
    }
}
