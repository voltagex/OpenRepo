using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRepo.Handler;

namespace OpenRepo.Tests
{
    [TestClass]
    public class SimpleTests
    {
        [TestMethod]
        public void TestAddHandler()
        {
            OpenRepo.Handler.TestRegistry registry = new TestRegistry();
            var registration = new HandlerRegistration() { Description = "Unit Test Github for Windows (OpenRepo 1.01 Shim)", ProtocolName = "github-windows", HandlerPath = "fake-handler.exe" };
            registry.SetHandlerForProtocol(registration);

            Assert.IsTrue(registry.OpenURL("github-windows://fake"));
        }
    }
}
