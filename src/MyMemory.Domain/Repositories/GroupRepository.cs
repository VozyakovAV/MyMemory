using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class GroupRepository : IRepository<MemoryGroup>
    {
        private readonly UnitOfWork _uow;

        public GroupRepository(UnitOfWork uow)
        {
            this._uow = uow;
        }

        public IQueryable<MemoryGroup> GetItems()
        {
            return _uow.GetGroups();
        }

        public int Save(MemoryGroup item)
        {
            if (item.Id == 0)
            {
                _uow.GetGroups().Add(item);
            }
            else
            {
                _uow._context.Entry(item).State = EntityState.Modified;
            }

            return item.Id;
        }

        public bool Delete(MemoryGroup item)
        {
            var t = _uow.GetGroups().SingleOrDefault(x => x.Id == item.Id);
            if (t != null)
            {
                _uow.GetGroups().Remove(item);
                return true;
            }
            return false;
        }
    }
}
