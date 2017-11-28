using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApplication5;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var t = Program.IsMatchOver(2,3);
           
        }
    }
}
