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
    }
}
