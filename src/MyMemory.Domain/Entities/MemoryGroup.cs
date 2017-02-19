using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryGroup : BaseObject
    {
        public string Name { get; set; }
        public MemoryGroup Parent { get; set; }
        public ICollection<MemoryGroup> Childs { get; set; }
        public ICollection<MemoryItem> Items { get; set; }

        public MemoryGroup() { }

        public MemoryGroup(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
