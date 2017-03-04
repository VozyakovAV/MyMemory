namespace MyMemory.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upd_cascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.mem_items", "groupID", "dbo.mem_groups");
            AddForeignKey("dbo.mem_items", "groupID", "dbo.mem_groups", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.mem_items", "groupID", "dbo.mem_groups");
            AddForeignKey("dbo.mem_items", "groupID", "dbo.mem_groups", "id");
        }
    }
}
