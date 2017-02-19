using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class ItemRepository : SimpleRepository<MemoryItem>
    {
        public ItemRepository(UnitOfWork uow)
            : base(uow, uow.GetItems())
        { }
    }
}
