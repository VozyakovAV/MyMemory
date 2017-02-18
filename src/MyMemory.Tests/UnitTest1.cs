using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMemory.Domain;

namespace MyMemory.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new MemoryDbContext())
            {
                var group = new MemoryGroup() { Name = "First" };
                db.Groups.Add(group);
                db.SaveChanges();

                var groups = db.Groups.ToList();
            }
        }

        [TestMethod]
        public void TestMethod30()
        {
            using (var db = new MemoryDbContext())
            {
                var group = new MemoryGroup() { Name = "Group 1" };
                db.Groups.Add(group);

                var group1 = new MemoryGroup() { Name = "Group 2" };
                db.Groups.Add(group1);
                var group2 = new MemoryGroup() { Name = "Group 3" };
                db.Groups.Add(group2);

                group1.Parent = group;
                group2.Parent = group;

                db.SaveChanges();

                var groups = db.Groups.ToList();
            }
        }

        [TestMethod]
        public void TestItems()
        {
            using (var db = new MemoryDbContext())
            {
                var group = new MemoryGroup() { Name = "Group 1" };
                db.Groups.Add(group);

                var group1 = new MemoryGroup() { Name = "Group 2" };
                db.Groups.Add(group1);
                var group2 = new MemoryGroup() { Name = "Group 3" };
                db.Groups.Add(group2);

                group1.Parent = group;
                group2.Parent = group;

                var item = new MemoryItem()
                {
                    Group = group1,
                    Question = "Вопрос 1",
                    Answer = "Ответ 1"
                };
                db.Items.Add(item);

                var item2 = new MemoryItem()
                {
                    Group = group1,
                    Question = "Вопрос 2",
                    Answer = "Ответ 2"
                };
                db.Items.Add(item2);

                db.SaveChanges();

                var groups = db.Groups.ToList();
            }
        }
    }
}
