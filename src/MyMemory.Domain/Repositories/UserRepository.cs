using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class UserRepository : SimpleRepository<MemoryUser>
    {
        public UserRepository(UnitOfWork uow)
            : base(uow, uow.GetUsers())
        { }
    }
}
