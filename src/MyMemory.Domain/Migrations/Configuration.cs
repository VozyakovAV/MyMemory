namespace MyMemory.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<MyMemory.Domain.MemoryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MyMemory.Domain.MemoryDbContext context)
        {
            //MemoryDBInitializer.CreateDB(context);
        }
    }
}
