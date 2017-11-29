namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMarkToDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserResults", "Mark", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserResults", "Mark", c => c.Int());
        }
    }
}
