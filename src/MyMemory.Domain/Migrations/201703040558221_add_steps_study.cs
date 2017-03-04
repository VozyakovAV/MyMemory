namespace MyMemory.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_steps_study : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.mem_steps_study",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        number = c.Int(nullable: false),
                        format = c.Int(nullable: false),
                        period = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.mem_steps_study");
        }
    }
}
