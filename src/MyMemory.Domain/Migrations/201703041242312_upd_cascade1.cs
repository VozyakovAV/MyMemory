namespace MyMemory.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upd_cascade1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.mem_tasks", "itemID", "dbo.mem_items");
            DropForeignKey("dbo.mem_tasks", "userID", "dbo.mem_users");
            AddForeignKey("dbo.mem_tasks", "itemID", "dbo.mem_items", "id", cascadeDelete: true);
            AddForeignKey("dbo.mem_tasks", "userID", "dbo.mem_users", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.mem_tasks", "userID", "dbo.mem_users");
            DropForeignKey("dbo.mem_tasks", "itemID", "dbo.mem_items");
            AddForeignKey("dbo.mem_tasks", "userID", "dbo.mem_users", "id");
            AddForeignKey("dbo.mem_tasks", "itemID", "dbo.mem_items", "id");
        }
    }
}
