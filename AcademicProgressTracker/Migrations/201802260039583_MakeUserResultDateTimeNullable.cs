namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeUserResultDateTimeNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserResults", "AddedDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            Sql("UPDATE UserResults SET AddedDateTime = '01/01/1900' WHERE AddedDateTime IS NULL");
            AlterColumn("dbo.UserResults", "AddedDateTime", c => c.DateTime(nullable: false));
        }
    }
}
