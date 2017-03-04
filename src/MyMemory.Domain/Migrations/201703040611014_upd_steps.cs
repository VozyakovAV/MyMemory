namespace MyMemory.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upd_steps : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.mem_steps_study", newName: "mem_stepsStudy");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.mem_stepsStudy", newName: "mem_steps_study");
        }
    }
}
