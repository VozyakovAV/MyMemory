using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public int? ParentId { get; set; }
        public MemoryGroup Parent { get; set; }

        public ICollection<MemoryGroup> Childs { get; set; }
    }
}
