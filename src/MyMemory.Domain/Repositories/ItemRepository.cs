using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class ItemRepository : IRepository<MemoryItem>
    {
        protected MemoryDbContext DB { get; private set; }

        public ItemRepository(MemoryDbContext db)
        {
            this.DB = db;
        }

        public IQueryable<MemoryItem> GetItems()
        {
            return DB.Items;
        }

        public int Save(MemoryItem item)
        {
            if (item.Id == 0)
            {
                DB.Items.Add(item);
                DB.SaveChanges();
            }
            else
            {
                DB.Entry(item).State = EntityState.Modified;
                DB.SaveChanges();
            }

            return item.Id;
        }

        public bool Delete(MemoryItem item)
        {
            var t = DB.Items.SingleOrDefault(x => x.Id == item.Id);
            if (t != null)
            {
                DB.Items.Remove(item);
                DB.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
