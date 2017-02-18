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
        private IDbSet<MemoryGroup> _list;

        public GroupRepository(UnitOfWork uow)
        {
            this._uow = uow;
            this._list = _uow.GetGroups();
        }

        public IQueryable<MemoryGroup> GetItems()
        {
            return _list;
        }

        public int Save(MemoryGroup item)
        {
            if (item.Id == 0)
            {
                _list.Add(item);
            }
            else
            {
                _uow.Entry(item).State = EntityState.Modified;
            }

            return item.Id;
        }

        public bool Delete(MemoryGroup item)
        {
            var t = _list.SingleOrDefault(x => x.Id == item.Id);
            if (t != null)
            {
                _list.Remove(item);
                return true;
            }
            return false;
        }
    }
}
