using System;
using CobWeb.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CobWeb.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //StringHelper.WriteIntoMMF()
            //Assert.AreEqual()
            Assert.AreEqual(Test(), "test:1234");
        }
        string Test()
        {
            MemoryMappedHelper.WriteIntoMMF("ok", "test:1234");
            return  MemoryMappedHelper.ReadIntoMMF("ok");
        }
    }
}
