using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyMemory.Domain
{
    public class MemoryDbContext : DbContext
    {
        static MemoryDbContext()
        {
            ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MemoryDbContext;Integrated Security=True";
            Database.SetInitializer<MemoryDbContext>(new MemoryDBInitializer());
        }

        public static string ConnectionString { get; set; }
        
        public MemoryDbContext()
            : base(ConnectionString) 
        { }

        public DbSet<MemoryGroup> Groups { get; set; }
        public DbSet<MemoryItem> Items { get; set; }
        public DbSet<MemoryUser> Users { get; set; }
        public DbSet<MemoryTask> Tasks { get; set; }
        public DbSet<MemoryStepsStudy> StepsStudy { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityMemoryGroup = modelBuilder.Entity<MemoryGroup>();
            var entityMemoryItem = modelBuilder.Entity<MemoryItem>();
            var entityMemoryUser = modelBuilder.Entity<MemoryUser>();
            var entityMemoryTask = modelBuilder.Entity<MemoryTask>();
            var entityMemoryStepsStudy = modelBuilder.Entity<MemoryStepsStudy>();

            entityMemoryGroup.ToTable("mem_groups");
            entityMemoryGroup.HasKey(x => x.Id);
            entityMemoryGroup.Property(x => x.Id).HasColumnName("id");
            entityMemoryGroup.Property(x => x.Name).HasColumnName("name").HasMaxLength(256).IsRequired();
            entityMemoryGroup.HasOptional(x => x.Parent)
                .WithMany(x => x.Childs)
                .Map(x => x.MapKey("parentID"))
                .WillCascadeOnDelete(false); // если поставить каскадность, то будет ошибка при создании базы ?? TODO: надо проверить

            entityMemoryItem.ToTable("mem_items");
            entityMemoryItem.HasKey(x => x.Id);
            entityMemoryItem.Property(x => x.Id).HasColumnName("id");
            entityMemoryItem.Property(x => x.Question).HasColumnName("question").HasMaxLength(512).IsRequired();
            entityMemoryItem.Property(x => x.Answer).HasColumnName("answer").HasMaxLength(512).IsRequired();
            entityMemoryItem.HasRequired(x => x.Group)
                .WithMany(x => x.Items)
                .Map(x => x.MapKey("groupID"))
                .WillCascadeOnDelete(true);

            entityMemoryUser.ToTable("mem_users");
            entityMemoryUser.HasKey(x => x.Id);
            entityMemoryUser.Property(x => x.Id).HasColumnName("id");
            entityMemoryUser.Property(x => x.Name).HasColumnName("name").HasMaxLength(20).IsRequired();
            entityMemoryUser.Property(x => x.Password).HasColumnName("password").HasMaxLength(128).IsRequired();

            entityMemoryTask.ToTable("mem_tasks");
            entityMemoryTask.HasKey(x => x.Id);
            entityMemoryTask.Property(x => x.Id).HasColumnName("id");
            entityMemoryTask.Property(x => x.StepNumber).HasColumnName("stepNumber").IsRequired();
            entityMemoryTask.Property(x => x.Deadline).HasColumnName("deadline").IsRequired();
            entityMemoryTask.HasRequired(x => x.Item)
                .WithMany()
                .Map(x => x.MapKey("itemID"))
                .WillCascadeOnDelete(true);
            entityMemoryTask.HasRequired(x => x.User)
                .WithMany()
                .Map(x => x.MapKey("userID"))
                .WillCascadeOnDelete(true);

            entityMemoryStepsStudy.ToTable("mem_stepsStudy");
            entityMemoryStepsStudy.HasKey(x => x.Id);
            entityMemoryStepsStudy.Property(x => x.Id).HasColumnName("id");
            entityMemoryStepsStudy.Property(x => x.Number).HasColumnName("number").IsRequired();
            entityMemoryStepsStudy.Property(x => x.Format).HasColumnName("format").IsRequired();
            entityMemoryStepsStudy.Property(x => x.Period).HasColumnName("period").IsRequired();
        }
    }
}
