using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryItem : BaseObject
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public MemoryGroup Group { get; set; }

        public override string ToString()
        {
            return Question;
        }
    }
}
