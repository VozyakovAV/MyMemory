﻿using System;
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
        public DbSet<MemoryUser> Users { get; set; }
        public DbSet<MemoryTask> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityMemoryGroup = modelBuilder.Entity<MemoryGroup>();
            var entityMemoryItem = modelBuilder.Entity<MemoryItem>();
            var entityMemoryUser = modelBuilder.Entity<MemoryUser>();
            var entityMemoryTask = modelBuilder.Entity<MemoryTask>();

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

            entityMemoryUser.ToTable("mem_users");
            entityMemoryUser.HasKey(x => x.Id);
            entityMemoryUser.Property(x => x.Id).HasColumnName("id");
            entityMemoryUser.Property(x => x.Name).HasColumnName("name").HasMaxLength(20).IsRequired();
            entityMemoryUser.Property(x => x.Password).HasColumnName("password").HasMaxLength(20).IsRequired();

            entityMemoryTask.ToTable("mem_tasks");
            entityMemoryTask.HasKey(x => x.Id);
            entityMemoryTask.Property(x => x.Id).HasColumnName("id");
            entityMemoryTask.Property(x => x.Number).HasColumnName("number").IsRequired();
            entityMemoryTask.Property(x => x.Deadline).HasColumnName("deadline").IsRequired();
            entityMemoryTask.Property(x => x.Status).HasColumnName("status").IsRequired();
            entityMemoryTask.HasRequired(x => x.Item)
                .WithOptional()
                .Map(x => x.MapKey("itemID"))
                .WillCascadeOnDelete(false);
            entityMemoryTask.HasRequired(x => x.User)
                .WithOptional()
                .Map(x => x.MapKey("userID"))
                .WillCascadeOnDelete(false);
        }
    }
}