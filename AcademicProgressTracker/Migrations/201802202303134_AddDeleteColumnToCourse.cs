namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeleteColumnToCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Deleted", c => c.Byte(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Deleted");
        }
    }
}
