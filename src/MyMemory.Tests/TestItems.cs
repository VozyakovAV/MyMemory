using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;
using MyMemory.BLL;

namespace MyMemory.Tests
{
    [TestClass]
    public class TestItems : BaseTest
    {
        [TestMethod]
        public void CRUD()
        {
            DeleteItemsInDB();

            /*var mng = new GroupManager();
            var groups = mng.GetGroups();
            Assert.AreEqual(0, groups.Length);

            var newGroup = new MemoryGroup("Группа");
            mng.SaveGroup(newGroup);

            groups = mng.GetGroups();
            Assert.AreEqual(1, groups.Length);
            Assert.AreEqual(newGroup, groups.First());

            newGroup.Name = "Группа 2";
            mng.SaveGroup(newGroup);

            groups = mng.GetGroups();
            Assert.AreEqual(1, groups.Length);
            Assert.AreEqual(newGroup, groups.First());

            mng.DeleteGroup(newGroup);
            groups = mng.GetGroups();
            Assert.AreEqual(0, groups.Length);*/
        }
    }
}
