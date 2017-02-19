using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;
using MyMemory.BLL;

namespace MyMemory.Tests
{
    [TestClass]
    public class TestTasks : BaseTest
    {
        [TestMethod]
        public void CRUD()
        {
            DeleteItemsInDB();
        }
    }
}
