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
        MemoryManager _mng;// = new MemoryManager();

        public TestEntities()
        {
            MemoryDbContext.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=_test_MyMemory;Integrated Security=True";
            _mng = new MemoryManager();
        }

        [TestInitialize]
        public void Init()
        {
            
        }

        [TestMethod]
        public void TestCascades()
        {
            DeleteDB();
            //DeleteItemsInDB();

            var group = new MemoryGroup("Группа");
            _mng.SaveGroup(group);

            var item = new MemoryItem()
            {
                Question = "Вопрос",
                Answer = "Ответ",
                Group = group
            };

            //_mng.SaveItem(item);

            _mng.DeleteGroup(group);

            Assert.AreEqual(0, _mng.GetGroups().Length);
            Assert.AreEqual(0, _mng.GetItems().Length);
        }
    }
}
