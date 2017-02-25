namespace MyMemory.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePassword : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.mem_groups",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 256),
                        parentID = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.mem_groups", t => t.parentID)
                .Index(t => t.parentID);
            
            CreateTable(
                "dbo.mem_items",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        question = c.String(nullable: false, maxLength: 512),
                        answer = c.String(nullable: false, maxLength: 512),
                        groupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.mem_groups", t => t.groupID)
                .Index(t => t.groupID);
            
            CreateTable(
                "dbo.mem_tasks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        number = c.Int(nullable: false),
                        deadline = c.DateTime(nullable: false),
                        status = c.Int(nullable: false),
                        itemID = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.mem_items", t => t.itemID)
                .ForeignKey("dbo.mem_users", t => t.userID)
                .Index(t => t.itemID)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.mem_users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 20),
                        password = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.mem_tasks", "userID", "dbo.mem_users");
            DropForeignKey("dbo.mem_tasks", "itemID", "dbo.mem_items");
            DropForeignKey("dbo.mem_groups", "parentID", "dbo.mem_groups");
            DropForeignKey("dbo.mem_items", "groupID", "dbo.mem_groups");
            DropIndex("dbo.mem_tasks", new[] { "userID" });
            DropIndex("dbo.mem_tasks", new[] { "itemID" });
            DropIndex("dbo.mem_items", new[] { "groupID" });
            DropIndex("dbo.mem_groups", new[] { "parentID" });
            DropTable("dbo.mem_users");
            DropTable("dbo.mem_tasks");
            DropTable("dbo.mem_items");
            DropTable("dbo.mem_groups");
        }
    }
}
