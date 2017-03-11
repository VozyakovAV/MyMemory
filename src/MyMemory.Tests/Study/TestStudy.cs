using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;
using MyMemory.BLL;
using Common;

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
        public void TestStudyCorrectAnswers()
        {
            CreateTestDB(_mng);

            var group = CreateGroup(_mng);
            var item1 = CreateItem(_mng, group, "Вопрос 1", "Ответ 1");
            var item2 = CreateItem(_mng, group, "Вопрос 2", "Ответ 2");

            var data = _mngStudy.Start(_userName, 0);
            Assert.AreEqual(item1.Question, data.Step.Question.Text);

            data = _mngStudy.NextStep(data, item1.Answer);
            Assert.IsTrue(data.PrevStep.Answer.IsCorrect);
            Assert.AreEqual(item2.Question, data.Step.Question.Text);

            data = _mngStudy.NextStep(data, item2.Answer);
            Assert.IsTrue(data.PrevStep.Answer.IsCorrect);
            Assert.IsNotNull(data.Message);
        }

        [TestMethod]
        public void TestStudyWrongAnswers()
        {
            CreateTestDB(_mng);

            var group = CreateGroup(_mng);
            var item1 = CreateItem(_mng, group, "Вопрос 1", "Ответ 1");
            var item2 = CreateItem(_mng, group, "Вопрос 2", "Ответ 2");

            var data = _mngStudy.Start(_userName, 0);
            Assert.AreEqual(item1.Question, data.Step.Question.Text);

            data = _mngStudy.NextStep(data, "none");
            Assert.IsFalse(data.PrevStep.Answer.IsCorrect);
            Assert.AreEqual(item1.Question, data.Step.Question.Text);

            data = _mngStudy.NextStep(data, "none");
            Assert.IsFalse(data.PrevStep.Answer.IsCorrect);
            Assert.AreEqual(item1.Question, data.Step.Question.Text);
        }

        [TestMethod]
        public void TestStudyDelay()
        {
            CreateTestDB(_mng);

            var group = CreateGroup(_mng);
            var item1 = CreateItem(_mng, group, "Вопрос 1", "Ответ 1");
            var item2 = CreateItem(_mng, group, "Вопрос 2", "Ответ 2");

            var data1 = _mngStudy.Start(_userName, 0);
            var data2 = _mngStudy.NextStep(data1, "none");
            Assert.AreEqual(item1.Question, data1.Step.Question.Text);

            CustomDateTime.FakeDate = DateTime.Now.AddDays(1);

            data1 = _mngStudy.Start(_userName, 0);
            data2 = _mngStudy.NextStep(data1, "none");
            Assert.AreEqual(item1.Question, data1.Step.Question.Text);
        }
    }
}
