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

            var mng = new GroupManager();
            var mngItem = new ItemManager();

            var group = new MemoryGroup("Группа");
            mng.SaveGroup(group);

            var items = mngItem.GetItems(group);
            Assert.AreEqual(0, items.Length);

            var newItem = new MemoryItem()
            {
                Question = "Вопрос",
                Answer = "Ответ",
                Group = group
            };

            mngItem.SaveItem(newItem);
            items = mngItem.GetItems(group);
            Assert.AreEqual(1, items.Length);
            Assert.AreEqual(newItem, items.First());

            var item = items.First();
            item.Question = "Вопрос 2";
            item.Answer = "Вопрос 2";
            mngItem.SaveItem(item);
            items = mngItem.GetItems(group);
            Assert.AreEqual(1, items.Length);
            Assert.AreEqual(newItem, items.First());

            mngItem.DeleteItem(items.First());
            items = mngItem.GetItems(group);
            Assert.AreEqual(0, items.Length);
        }
    }
}
