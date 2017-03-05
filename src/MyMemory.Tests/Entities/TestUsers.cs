using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;
using MyMemory.BLL;

namespace MyMemory.Tests
{
    [TestClass]
    public class TestUsers : BaseTest
    {
        [TestMethod]
        public void CRUD()
        {
            DeleteItemsInDB();

            var mng = new MemoryManager();
            var users = mng.GetUsers();
            Assert.AreEqual(0, users.Length);

            var user = new MemoryUser("Андрей")
            {
                Password = "123"
            };
            mng.SaveUser(user);
            
            users = mng.GetUsers();
            Assert.AreEqual(1, users.Length);
            Assert.AreEqual(user, users.First());

            mng.DeleteUser(user);
            users = mng.GetUsers();
            Assert.AreEqual(0, users.Length);
        }
    }
}
