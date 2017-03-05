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

            db.StepsStudy.Add(new MemoryStepsStudy(1, PeriodFormat.Min, 20));
            db.StepsStudy.Add(new MemoryStepsStudy(2, PeriodFormat.Hour, 4));
            db.StepsStudy.Add(new MemoryStepsStudy(3, PeriodFormat.Hour, 8));
            db.StepsStudy.Add(new MemoryStepsStudy(4, PeriodFormat.Day, 1));
            db.StepsStudy.Add(new MemoryStepsStudy(5, PeriodFormat.Day, 2));
            db.StepsStudy.Add(new MemoryStepsStudy(6, PeriodFormat.Day, 4));
            db.StepsStudy.Add(new MemoryStepsStudy(7, PeriodFormat.Day, 8));

            db.SaveChanges();
        }
    }
}
