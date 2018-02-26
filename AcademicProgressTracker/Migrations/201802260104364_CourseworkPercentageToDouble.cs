namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseworkPercentageToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courseworks", "Percentage", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courseworks", "Percentage", c => c.Int(nullable: false));
        }
    }
}
