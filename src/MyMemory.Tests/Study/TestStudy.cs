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
            _mngStudy = new StudyManager()
            {
                IsRandom = false
            };
            DeleteItemsInDB();
        }

        [TestMethod]
        public void TestStudyCorrectAnswers()
        {
            CreateTestDB(_mng);

            var group = CreateGroup(_mng);
            var item1 = CreateItem(_mng, group, "Вопрос 1", "Ответ 1");
            var item2 = CreateItem(_mng, group, "Вопрос 2", "Ответ 2");

            var data = _mngStudy.Start(_userName, group.Id);
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

            var data = _mngStudy.Start(_userName, group.Id);
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

            var data1 = _mngStudy.Start(_userName, group.Id);
            var data2 = _mngStudy.NextStep(data1, "none");
            var data3 = _mngStudy.NextStep(data2, item1.Answer);
            var data4 = _mngStudy.NextStep(data3, "none");
            Assert.AreEqual(item2.Question, data3.Step.Question.Text);

            CustomDateTime.FakeDate = DateTime.Now.AddDays(1);

            data1 = _mngStudy.Start(_userName, group.Id);
            Assert.AreEqual(item1.Question, data1.Step.Question.Text);
        }

        [TestMethod]
        public void TestStudyGroups()
        {
            CreateTestDB(_mng);

            var group1 = CreateGroup(_mng, "Группа 1");
            var group1_1 = CreateGroup(_mng, "Группа 1.1", group1);
            var group1_2 = CreateGroup(_mng, "Группа 1.2", group1);
            var group2 = CreateGroup(_mng, "Группа 2");

            var item1_1_1 = CreateItem(_mng, group1_1, "Вопрос 1.1.1", "Ответ 1.1.1");
            var item1_2_1 = CreateItem(_mng, group1_2, "Вопрос 1.2.1", "Ответ 1.2.1");
            var item1_2_2 = CreateItem(_mng, group1_2, "Вопрос 1.2.2", "Ответ 1.2.2");

            var item2_1 = CreateItem(_mng, group2, "Вопрос 2.1", "Ответ 2.1");
            var item2_2 = CreateItem(_mng, group2, "Вопрос 2.2", "Ответ 2.2");

            VerifySequenceStudy(group1_2.Id, new MemoryItem[] { item1_2_1, item1_2_2 });
            DeleteTasksInDB();
            VerifySequenceStudy(group2.Id, new MemoryItem[] { item2_1, item2_2 });
            DeleteTasksInDB();
            VerifySequenceStudy(group1.Id, new MemoryItem[] { item1_1_1, item1_2_1, item1_2_2 });
            DeleteTasksInDB();
        }

        private void VerifySequenceStudy(int groupId, params MemoryItem[] items)
        {
            var data = _mngStudy.Start(_userName, groupId);

            foreach (var item in items)
            {
                Assert.AreEqual(item.Question, data.Step.Question.Text);
                data = _mngStudy.NextStep(data, item.Answer);
            }

            Assert.IsNotNull(data.Message);
        }
    }
}
