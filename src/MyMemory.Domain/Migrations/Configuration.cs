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
            /*context.StepsStudy.Add(new MemoryStepsStudy(1, PeriodFormat.Min, 20));
            context.StepsStudy.Add(new MemoryStepsStudy(2, PeriodFormat.Hour, 4));
            context.StepsStudy.Add(new MemoryStepsStudy(3, PeriodFormat.Hour, 8));
            context.StepsStudy.Add(new MemoryStepsStudy(4, PeriodFormat.Day, 1));
            context.StepsStudy.Add(new MemoryStepsStudy(5, PeriodFormat.Day, 2));*/

            context.SaveChanges();
        }
    }
}
