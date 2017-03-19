using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

            db.StepsStudy.Add(new MemoryStepsStudy(1, PeriodFormat.Min, 20));
            db.StepsStudy.Add(new MemoryStepsStudy(2, PeriodFormat.Hour, 4));
            db.StepsStudy.Add(new MemoryStepsStudy(3, PeriodFormat.Hour, 8));
            db.StepsStudy.Add(new MemoryStepsStudy(4, PeriodFormat.Day, 1));
            db.StepsStudy.Add(new MemoryStepsStudy(5, PeriodFormat.Day, 2));
            db.StepsStudy.Add(new MemoryStepsStudy(6, PeriodFormat.Day, 4));
            db.StepsStudy.Add(new MemoryStepsStudy(7, PeriodFormat.Day, 8));

            var group1 = new MemoryGroup("Английский");
            db.Groups.Add(group1);
            var group1_1 = new MemoryGroup("Глаголы (топ 100)", group1);
            db.Groups.Add(group1_1);

            AddEnglishWords(db, group1_1, "MyMemory.Domain.Data.EnglishVerbs.txt");

            db.SaveChanges();
        }

        public static void AddGroup(MemoryDbContext db)
        {
            var group = db.Groups.First(x => x.Name == "Английский");
            var group1 = new MemoryGroup("Прилагательные (топ 100)", group);
            db.Groups.Add(group1);

            AddEnglishWords(db, group1, "MyMemory.Domain.Data.EnglishAdjectives.txt");

            db.SaveChanges();
        }

        private static void AddEnglishWords(MemoryDbContext db, MemoryGroup group, string resourceName)
        {
            var items = ParseWords(resourceName);
            items.ForEach(x => x.Group = group);

            foreach (var item in items)
            {
                item.Group = group;
                db.Items.Add(item);
            }
        }

        public static List<MemoryItem> ParseWords(string resourceName)
        {
            var text = MemoryDBInitializer.ReadResource("MyMemory.Domain.Data.EnglishVerbs.txt");
            var mc = Regex.Matches(text, @"(?<a>\w+)\s*—\s*(?<q>\w+)");

            return mc.OfType<Match>()
                .Select(x => new MemoryItem() 
                { 
                    Question = x.Groups["q"].Value.Trim(), 
                    Answer = x.Groups["a"].Value.Trim() 
                })
                .ToList();
        }

        public static string ReadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1251)))
            {
                return reader.ReadToEnd();
            }
        }
            
    }
}
