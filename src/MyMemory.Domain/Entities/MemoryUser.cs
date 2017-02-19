﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryUser : BaseObject
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public MemoryUser()
        { }

        public MemoryUser(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
