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

            var mng = new MemoryManager();

            var group = new MemoryGroup("Группа");
            mng.SaveGroup(group);

            var items = mng.GetItems(group);
            Assert.AreEqual(0, items.Length);

            var newItem = new MemoryItem()
            {
                Question = "Вопрос",
                Answer = "Ответ",
                Group = group
            };

            mng.SaveItem(newItem);
            items = mng.GetItems(group);
            Assert.AreEqual(1, items.Length);
            Assert.AreEqual(newItem, items.First());

            var item = items.First();
            item.Question = "Вопрос 2";
            item.Answer = "Вопрос 2";
            mng.SaveItem(item);
            items = mng.GetItems(group);
            Assert.AreEqual(1, items.Length);
            Assert.AreEqual(newItem, items.First());

            mng.DeleteItem(items.First());
            items = mng.GetItems(group);
            Assert.AreEqual(0, items.Length);
        }
    }
}
