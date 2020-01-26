using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.ExternalApi.Tests
{
    [TestClass]
    public class BricklinkLoginTests
    {
        [TestMethod]
        public void TestValidLogin()
        {
            var api = new BricklinkLoginApi();
            var cookies = new CookieContainer();

            Assert.IsNotNull(AppConfig.BricklinkUserName);
            Assert.IsNotNull(AppConfig.BricklinkPassword);

            var wasLoggedIn = api.Login(cookies, AppConfig.BricklinkUserName, AppConfig.BricklinkPassword);
            Assert.IsTrue(wasLoggedIn);
        }

        [TestMethod]
        public void TestInvalidLogin()
        {
            var api = new BricklinkLoginApi();
            var cookies = new CookieContainer();

            var userName = "SomeInvalidUserName90210";
            var password = Guid.NewGuid().ToString();

            var wasLoggedIn = api.Login(cookies, userName, password);
            Assert.IsFalse(wasLoggedIn);
        }
    }
}
