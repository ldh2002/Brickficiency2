using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.DependencyInjection;
using WindmillHelix.Brickficiency2.Services.Ldd;

namespace WindmillHelix.Brickficiency2.Services.Tests
{
    [TestClass]
    public class LddMapperTests
    {
        [TestMethod]
        public void TestIndirectMapping()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AppConfigBricklinkCredentialProvider>().AsImplementedInterfaces().SingleInstance();
            var container = DependencyInjectionConfig.Setup(builder);

            var service = container.Resolve<ILddMapperService>();

            Assert.IsNotNull(service);
            var source = new LddPart { DesignId = "50746", Materials = "1" };
            var result = service.MapItem(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.WasSuccessful);
            Assert.IsNotNull(result.Mapped);

            Assert.AreEqual("54200", result.Mapped.ItemId);
        }
    }
}
