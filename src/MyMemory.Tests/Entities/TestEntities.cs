using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;
using MyMemory.BLL;

namespace MyMemory.Tests
{
    [TestClass]
    public class TestEntities : BaseTest
    {
        MemoryManager _mng;

        [TestInitialize]
        public void Init()
        {
            MemoryDbContext.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=_test_MyMemory;Integrated Security=True";
            _mng = new MemoryManager();
            DeleteItemsInDB();
        }

        [TestMethod]
        public void TestCascades()
        {
            var group = CreateGroup(_mng);
            var item = CreateItem(_mng, group);

            _mng.DeleteGroup(group);
            Assert.AreEqual(0, _mng.GetGroups().Length);
            Assert.AreEqual(0, _mng.GetItems().Length);
        }

        [TestMethod]
        public void TestCascades2()
        {
            var user = CreateUser(_mng);
            var group = CreateGroup(_mng);
            var item = CreateItem(_mng, group);
            var task = CreateTask(_mng, user, item);
            _mng.DeleteGroup(group);

            Assert.AreEqual(0, _mng.GetGroups().Length);
            Assert.AreEqual(0, _mng.GetItems().Length);
            Assert.AreEqual(0, _mng.GetTasks().Length);
            Assert.AreEqual(1, _mng.GetUsers().Length);
        }

        [TestMethod]
        public void TestCascades3()
        {
            var user = CreateUser(_mng);
            var group = CreateGroup(_mng);
            var item = CreateItem(_mng, group);
            var task = CreateTask(_mng, user, item);
            _mng.DeleteItem(item);

            Assert.AreEqual(1, _mng.GetGroups().Length);
            Assert.AreEqual(0, _mng.GetItems().Length);
            Assert.AreEqual(0, _mng.GetTasks().Length);
            Assert.AreEqual(1, _mng.GetUsers().Length);
        }

        [TestMethod]
        public void TestCascades4()
        {
            var user = CreateUser(_mng);
            var group = CreateGroup(_mng);
            var item = CreateItem(_mng, group);
            var task = CreateTask(_mng, user, item);
            _mng.DeleteTask(task);

            Assert.AreEqual(1, _mng.GetGroups().Length);
            Assert.AreEqual(1, _mng.GetItems().Length);
            Assert.AreEqual(0, _mng.GetTasks().Length);
            Assert.AreEqual(1, _mng.GetUsers().Length);
        }

        [TestMethod]
        public void TestCascades5()
        {
            var user = CreateUser(_mng);
            var group = CreateGroup(_mng);
            var item = CreateItem(_mng, group);
            var task = CreateTask(_mng, user, item);
            _mng.DeleteUser(user);

            Assert.AreEqual(1, _mng.GetGroups().Length);
            Assert.AreEqual(1, _mng.GetItems().Length);
            Assert.AreEqual(0, _mng.GetTasks().Length);
            Assert.AreEqual(0, _mng.GetUsers().Length);
        }
    }
}
