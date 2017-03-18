using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;
using MyMemory.BLL;

namespace MyMemory.Tests
{
    public class BaseTest
    {
        public void DeleteDB()
        {
            using (var db = new MemoryDbContext())
            {
                if (db.Database.Exists())
                    db.Database.Delete();
            }
        }

        public void DeleteItemsInDB()
        {
            using (var db = new MemoryDbContext())
            {
                if (db.Database.Exists())
                {
                    var sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
                    var list = db.Database.SqlQuery<string>(sql).ToList();

                    DeleteTableIfExist("mem_stepsStudy", list, db);
                    DeleteTableIfExist("mem_tasks", list, db);
                    DeleteTableIfExist("mem_items", list, db);
                    DeleteTableIfExist("mem_groups", list, db);
                    DeleteTableIfExist("mem_users", list, db);
                }
            }
        }

        public void DeleteTasksInDB()
        {
            using (var db = new MemoryDbContext())
            {
                if (db.Database.Exists())
                {
                    var sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
                    var list = db.Database.SqlQuery<string>(sql).ToList();
                    DeleteTableIfExist("mem_tasks", list, db);
                }
            }
        }

        private void DeleteTableIfExist(string tableName, List<string> tables, MemoryDbContext db)
        {
            if (tables.Contains(tableName))
            {
                var sql = string.Format("DELETE FROM [dbo].[{0}]", tableName);
                db.Database.ExecuteSqlCommand(sql);
            }
        }

        protected MemoryGroup CreateGroup(MemoryManager mng)
        {
            return mng.SaveGroup(NewGroup());
        }

        protected MemoryGroup CreateGroup(MemoryManager mng, string name)
        {
            return mng.SaveGroup(new MemoryGroup(name));
        }

        protected MemoryGroup CreateGroup(MemoryManager mng, string name, MemoryGroup parent)
        {
            return mng.SaveGroup(new MemoryGroup(name, parent));
        }

        protected MemoryGroup NewGroup()
        {
            return new MemoryGroup("Группа");
        }

        protected MemoryItem CreateItem(MemoryManager mng, MemoryGroup group)
        {
            return mng.SaveItem(NewItem(group));
        }

        protected MemoryItem CreateItem(MemoryManager mng, MemoryGroup group, string question, string answer)
        {
            return mng.SaveItem(NewItem(question, answer, group));
        }

        protected MemoryItem NewItem(MemoryGroup group)
        {
            return NewItem("Вопрос", "Ответ", group);
        }

        protected MemoryItem NewItem(string question, string answer, MemoryGroup group)
        {
            return new MemoryItem()
            {
                Question = question,
                Answer = answer,
                Group = group
            };
        }

        protected MemoryUser CreateUser(MemoryManager mng)
        {
            return mng.SaveUser(NewUser());
        }

        protected MemoryUser NewUser()
        {
            return new MemoryUser("Юзер") 
            { 
                Password = "123" 
            };
        }


        protected MemoryTask CreateTask(MemoryManager mng, MemoryUser user, MemoryItem item)
        {
            return mng.SaveTask(NewTask(user, item));
        }

        protected MemoryTask NewTask(MemoryUser user, MemoryItem item)
        {
            return new MemoryTask()
            {
                User = user,
                Item = item,
                StepNumber = 0,
                Deadline = DateTime.Now.AddMinutes(20)
            };
        }

        protected void CreateTestDB(MemoryManager mng)
        {
            var user = new MemoryUser("Andrew", "AOY9alcCW6gapSAIJ4rzvaaHh159btM6Pj3a29a9JgIj17V8SkntZAGzwl8ljs9TJA==");
            mng.SaveUser(user);

            mng.SaveStep(new MemoryStepsStudy(1, PeriodFormat.Min, 20));
            mng.SaveStep(new MemoryStepsStudy(2, PeriodFormat.Hour, 4));
            mng.SaveStep(new MemoryStepsStudy(3, PeriodFormat.Hour, 8));
            mng.SaveStep(new MemoryStepsStudy(4, PeriodFormat.Day, 1));
            mng.SaveStep(new MemoryStepsStudy(5, PeriodFormat.Day, 2));
            mng.SaveStep(new MemoryStepsStudy(6, PeriodFormat.Day, 4));
            mng.SaveStep(new MemoryStepsStudy(7, PeriodFormat.Day, 8));
        }
    }
}
