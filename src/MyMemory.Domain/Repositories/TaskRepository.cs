﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class TaskRepository : SimpleRepository<MemoryTask>
    {
        public TaskRepository(UnitOfWork uow)
            : base(uow, uow.GetTasks())
        { }
    }
}
