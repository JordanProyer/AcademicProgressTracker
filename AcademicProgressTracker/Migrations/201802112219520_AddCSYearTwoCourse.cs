namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCSYearTwoCourse : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Modules VALUES (\'Advanced Databases\', \'CS2F16\', 1, 2, 10, 0), (\'Compilers\', \'CS2CO16\', 1, 2, 10, 0), (\'Computer Architecture\', \'CS2CA16\', 1, 2, 10, 0), (\'Databases\', \'CS2DB16\', 1, 2, 10, 0), (\'Enterprise Architecture Modelling\', \'CS2AM16\', 1, 2, 10, 1), (\'Essential Algorithms\', \'CS2EA16\', 1, 2, 10, 0), (\'HCI and Applications\', \'CS2HA16\', 1, 2, 20, 1), (\'Java\', \'CS2JA16\', 1, 2, 20, 0), (\'Neural Networks\', \'CS2NN16\', 1, 2, 10, 1), (\'Operating Systems\', \'CS2OS16\', 1, 2, 10, 0), (\'Robotic Systems\', \'BI2RS16\', 1, 2, 10, 1), (\'Service-Oriented System Applications\', \'CS2SA16\', 1, 2, 10, 1), (\'Systems Design and Project Management\', \'CS2SM16\', 1, 2, 20, 0)");
            Sql("INSERT INTO Courseworks VALUES (\'Coursework 1\', 23, 30), (\'Written Exam\', 23, 70),(\'Coursework 1\', 24, 30),(\'Written Exam\', 24, 70),(\'Online Tests\', 25, 30),(\'Written Exam\', 25, 70),(\'Coursework 1\', 26, 50),(\'Written Exam\', 26, 50),(\'Written Exam\', 27, 100),(\'Coursework 1\', 28, 30),(\'Written Exam\', 28, 70),(\'Coursework 1\', 29, 50),(\'Written Exam\', 29, 50),(\'Coursework 1\', 30, 40),(\'Coursework 2\', 30, 60),(\'Coursework 1\', 31, 100),(\'Coursework 1\', 32, 30),(\'Written Exam\', 32, 70),(\'Coursework 1\', 33, 30),(\'Written Exam\', 33, 70),(\'Coursework 1\', 34, 100),(\'CM Skills\', 35, 25),(\'Project Management Report\', 35, 25),(\'Systems Design Technical Report\', 35, 50)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Modules WHERE CourseId = 1 AND YearId = 2");
            Sql("DELETE FROM Courseworks WHERE ModuleId IN(23,24,25,26,27,28,29,30,31,32,33,34,35)");
        }
    }
}
