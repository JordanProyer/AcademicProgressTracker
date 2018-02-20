namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeletedColumnToCourse : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE Courses ADD Deleted int CONSTRAINT DF_Courses_Deleted DEFAULT(0) not null");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE Courses DROP CONSTRAINT DF_Courses_Deleted");
            Sql("ALTER TABLE Courses DROP COLUMN Deleted");
        }
    }
}
