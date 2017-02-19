using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMemory.Domain;

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

                    DeleteTableIfExist("mem_tasks", list, db);
                    DeleteTableIfExist("mem_items", list, db);
                    DeleteTableIfExist("mem_groups", list, db);
                    DeleteTableIfExist("mem_users", list, db);
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
    }
}
