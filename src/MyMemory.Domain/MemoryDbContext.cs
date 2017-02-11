using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryDbContext : DbContext
    {
        public MemoryDbContext() : base("MemoryDbContext") { }

        public DbSet<MemoryGroup> Groups { get; set; }
    }
}
