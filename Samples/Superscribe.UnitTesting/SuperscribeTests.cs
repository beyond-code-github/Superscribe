using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Superscribe.UnitTesting
{
    using Superscribe.Models;
    using Superscribe.Utils;

    [TestClass]
    public class SuperscribeUnitTests
    {
        private RouteWalker<RouteData> routeWalker;
            
        [TestInitialize]
        public void Setup()
        {
            routeWalker = new RouteWalker<RouteData>(ʃ.Base);

            ʃ.Route(ʅ => 
                ʅ / "Hello" / (
                    ʅ / "World"         * (o => "Hello World!")
                  | ʅ / (ʃString)"Name" * (o => "Hello " + o.Parameters.Name)));
        }

        [TestCleanup]
        public void Cleanup()
        {
            ʃ.Reset();
        }
        
        [TestMethod]
        public void Test_Hello_World_Get()
        {
            var routeData = new RouteData();
            routeWalker.WalkRoute("/Hello/World", "Get", routeData);

            Assert.AreEqual("Hello World!", routeData.Response);
        }

        [TestMethod]
        public void Test_Hello_Name_Get()
        {
            var routeData = new RouteData();
            routeWalker.WalkRoute("/Hello/Kathryn", "Get", routeData);

            Assert.AreEqual("Kathryn", routeData.Parameters.Name);
            Assert.AreEqual("Hello Kathryn", routeData.Response);
        }
    }
}
