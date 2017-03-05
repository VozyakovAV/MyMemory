using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;
using MyMemory.BLL;

namespace MyMemory.Tests
{
    [TestClass]
    public class TestStudy : BaseTest
    {
        MemoryManager _mng;
        StudyManager _mngStudy;

        [TestInitialize]
        public void Init()
        {
            MemoryDbContext.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=_test_MyMemory;Integrated Security=True";
            _mng = new MemoryManager();
            _mngStudy = new StudyManager();
            DeleteItemsInDB();
        }

        [TestMethod]
        public void TestCascades()
        {
            var mngItem = new RepeatTasksManager();
            var item = mngItem.GetNextItem();
            Assert.IsNull(item);

        }
    }
}
