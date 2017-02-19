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
        public int Number { get; set; }
        public DateTime Deadline { get; set; }
        public TaskStatus Status { get; set; }

        public MemoryTask()
        { }
    }

    public enum TaskStatus
    {
        None,
        Wrong,
        Success
    }
}
