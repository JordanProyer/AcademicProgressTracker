namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBoundsToDoubles : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Classifications", "LowerBound", c => c.Double(nullable: false));
            AlterColumn("dbo.Classifications", "UpperBound", c => c.Double(nullable: false));
            Sql("UPDATE Classifications SET UpperBound = 69.99 WHERE Id = 2");
            Sql("UPDATE Classifications SET UpperBound = 59.99 WHERE Id = 3");
            Sql("UPDATE Classifications SET UpperBound = 49.99 WHERE Id = 4");
            Sql("UPDATE Classifications SET UpperBound = 39.99 WHERE Id = 5");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Classifications", "UpperBound", c => c.Int(nullable: false));
            AlterColumn("dbo.Classifications", "LowerBound", c => c.Int(nullable: false));
            Sql("UPDATE Classifications SET UpperBound = 70 WHERE Id = 2");
            Sql("UPDATE Classifications SET UpperBound = 60 WHERE Id = 3");
            Sql("UPDATE Classifications SET UpperBound = 50 WHERE Id = 4");
            Sql("UPDATE Classifications SET UpperBound = 40 WHERE Id = 5");
        }
    }
}
