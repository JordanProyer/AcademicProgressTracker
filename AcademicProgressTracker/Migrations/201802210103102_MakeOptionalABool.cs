namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeOptionalABool : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Modules", "Optional", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Modules", "Optional", c => c.Byte(nullable: false));
        }
    }
}
