using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Helpers;

namespace MyMemory.Domain
{
    public class MemoryDBInitializer : CreateDatabaseIfNotExists<MemoryDbContext>
    {
        protected override void Seed(MemoryDbContext db)
        {
            CreateDB(db); 
        }

        public static void CreateDB(MemoryDbContext db)
        {
            var user = new MemoryUser()
            {
                Name = "Andrew",
                Password = "AOY9alcCW6gapSAIJ4rzvaaHh159btM6Pj3a29a9JgIj17V8SkntZAGzwl8ljs9TJA=="
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
