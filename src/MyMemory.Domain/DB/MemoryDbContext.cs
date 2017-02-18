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
        public DbSet<MemoryItem> Items { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityMemoryGroup = modelBuilder.Entity<MemoryGroup>();
            var entityMemoryItem = modelBuilder.Entity<MemoryItem>();

            entityMemoryGroup.ToTable("mem_groups");
            entityMemoryGroup.HasKey(x => x.Id);
            entityMemoryGroup.Property(x => x.Id).HasColumnName("id");
            entityMemoryGroup.Property(x => x.Name).HasColumnName("name").HasMaxLength(256).IsRequired();
            entityMemoryGroup.HasOptional(x => x.Parent)
                .WithMany(x => x.Childs)
                .Map(x => x.MapKey("groupID"))
                .WillCascadeOnDelete(false); // если поставить каскадность, то будет ошибка при создании базы ?? TODO: надо проверить

            entityMemoryItem.ToTable("mem_items");
            entityMemoryItem.HasKey(x => x.Id);
            entityMemoryItem.Property(x => x.Id).HasColumnName("id");
            entityMemoryItem.Property(x => x.Question).HasColumnName("question").HasMaxLength(512).IsRequired();
            entityMemoryItem.Property(x => x.Answer).HasColumnName("answer").HasMaxLength(512).IsRequired();
            entityMemoryItem.HasRequired(x => x.Group)
                .WithMany(x => x.Items)
                .Map(x => x.MapKey("groupID"))
                .WillCascadeOnDelete(false); // если поставить каскадность, то будет ошибка при создании базы ?? TODO: надо проверить
        }
    }
}
