using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryTask : BaseObject
    {
        public MemoryItem Item { get; set; }
        public MemoryUser User { get; set; }
        public int StepNumber { get; set; }
        public DateTime Deadline { get; set; }

        public MemoryTask()
        { }

        public MemoryTask(MemoryUser user, MemoryItem item, int stepNumber, DateTime deadline)
        {
            this.User = user;
            this.Item = item;
            this.StepNumber = stepNumber;
            this.Deadline = deadline;
        }
    }
}
