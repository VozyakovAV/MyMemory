using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryItem : BaseObject
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public MemoryGroup Group { get; set; }

        public MemoryItem()
        { }

        public MemoryItem(MemoryGroup group, string question, string answer)
        {
            this.Group = group;
            this.Question = question;
            this.Answer = answer;
        }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", Question, Answer);
        }
    }
}
