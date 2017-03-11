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
        private MemoryManager _mng;
        private StudyManager _mngStudy;
        private string _userName = "Andrew";

        [TestInitialize]
        public void Init()
        {
            MemoryDbContext.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=_test_MyMemory;Integrated Security=True";
            _mng = new MemoryManager();
            _mngStudy = new StudyManager();
            DeleteItemsInDB();
        }

        [TestMethod]
        public void TestStudySimple()
        {
            CreateTestDB(_mng);

            var group = CreateGroup(_mng);
            var item1 = CreateItem(_mng, group, "Вопрос 1", "Ответ 1");
            var item2 = CreateItem(_mng, group, "Вопрос 2", "Ответ 2");


            var data = _mngStudy.Start(_userName, 0);
            Assert.AreEqual(item1.Id, data.Step.Question.ItemId);

            data = _mngStudy.NextStep(data, item1.Answer);
            Assert.IsTrue(data.PrevStep.Answer.IsCorrect);
            Assert.AreEqual(item2.Id, data.Step.Question.ItemId);

            data = _mngStudy.NextStep(data, item2.Answer);
            Assert.IsTrue(data.PrevStep.Answer.IsCorrect);
            Assert.IsNotNull(data.Message);
        }

        
    }
}
