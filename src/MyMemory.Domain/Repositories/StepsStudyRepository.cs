using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class StepsStudyRepository : SimpleRepository<MemoryStepsStudy>
    {
        public StepsStudyRepository(UnitOfWork uow)
            : base(uow, uow.GetStepsStudy())
        { }
    }
}
