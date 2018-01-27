namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimeToUserResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserResults", "AddedDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserResults", "AddedDateTime");
        }
    }
}
