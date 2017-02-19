using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class GroupRepository : SimpleRepository<MemoryGroup>
    {
        public GroupRepository(UnitOfWork uow)
            : base(uow, uow.GetGroups())
        { }
    }
}
