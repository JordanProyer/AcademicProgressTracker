namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserModules : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserModules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ModuleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserModules", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserModules", "ModuleId", "dbo.Modules");
            DropIndex("dbo.UserModules", new[] { "ModuleId" });
            DropIndex("dbo.UserModules", new[] { "UserId" });
            DropTable("dbo.UserModules");
        }
    }
}
