using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class SimpleRepository<T> : IRepository<T> where T: BaseObject
    {
        protected readonly UnitOfWork _uow;
        protected IDbSet<T> _list;

        public SimpleRepository(UnitOfWork uow, IDbSet<T> list)
        {
            this._uow = uow;
            this._list = list;
        }

        public IQueryable<T> GetItems()
        {
            return _list;
        }

        public int Save(T item)
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

        public bool Delete(T item)
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
