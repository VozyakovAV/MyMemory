using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryDBInitializer : CreateDatabaseIfNotExists<MemoryDbContext>
    {
        protected override void Seed(MemoryDbContext db)
        {
            var user = new MemoryUser()
            {
                Name = "Andrew",
                Password = "1237"
            };
            db.Users.Add(user);

            var group = new MemoryGroup("Группа");
            db.Groups.Add(group);

            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var a = random.Next(1, 10);
                var b = random.Next(1, 10);

                var item = new MemoryItem()
                {
                    Group = group,
                    Question = string.Format("{0} + {1}", a, b),
                    Answer = (a + b).ToString()
                };

                db.Items.Add(item);
            }

            db.SaveChanges();
        }
    }
}
