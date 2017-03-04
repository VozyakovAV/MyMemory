namespace MyMemory.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upd_tasks : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.mem_tasks", "number", "stepNumber");
            DropColumn("dbo.mem_tasks", "status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.mem_tasks", "status", c => c.Int(nullable: false));
            RenameColumn("dbo.mem_tasks", "stepNumber", "number");
        }
    }
}
