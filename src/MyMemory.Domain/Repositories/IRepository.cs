using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetItems();
        T Save(T item);
        bool Delete(T item);
    }
}
