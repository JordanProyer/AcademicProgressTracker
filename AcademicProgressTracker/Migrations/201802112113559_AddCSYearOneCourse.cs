namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCSYearOneCourse : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Modules VALUES (\'Programming\', \'CS1PR16\', 1, 1, 20, 0), (\'Applications of Computer Science\', \'CS1AC16\', 1, 1, 20, 0),(\'Fundamentals of Computer Science\', \'CS1FC16\', 1, 1, 20, 0), (\'Mathematics of Computer Science\', \'CS1MA16\', 1, 1, 20, 0),(\'Software Engineering\', \'CS1SE16 \', 1, 1, 20, 0), (\'Student Enterprise\', \'MM1F10\', 1, 1, 20, 1), (\'Probability and  Statistics\', \'ST1PS\', 1, 1, 20, 1),(\'Codes and Code Breaking\', \'MA115\', 1, 1, 20, 1),(\'Institution Wide Language Programme\', \'LA1XX1\', 1, 1, 20, 1)");
            Sql("INSERT INTO Courseworks VALUES (\'AI Labs\', 15, 7.5), (\'CV Labs\', 15, 7.5),(\'Robot Labs\', 15, 7.5), (\'Online test\', 15, 7.5),(\'Written Exam\', 15, 70),(\'Practicals Average\', 14, 35),(\'Class Test\', 14, 7), (\'Final Project\', 14, 28),(\'Written Exam\', 14, 70),(\'Online tests\', 16, 10),(\'Lab Report 1 Autumn term\', 16, 5), (\'Lab Report 2 Spring term\', 16, 15),(\'Written Exam\', 16, 70), (\'Coursework 1\', 17, 10), (\'Coursework 2\', 17, 10), (\'Coursework 3\', 17, 10), (\'Coursework 4\', 17, 10), (\'Written Exam\', 17, 60),(\'Coursework 1\', 18, 40),(\'Written Exam\', 18, 60),(\'Team Match\', 19, 5),(\'Team Presentation\', 19, 20),(\'Team Enterprise Report\', 19, 40),(\'Reflective Questionnaire\', 19, 5),(\'Written Exam\', 19, 30),(\'Report\', 20, 15),(\'Oral Assessment and Presentation\', 20, 5),(\'Set Exercise\', 20, 10),(\'Written Exam\', 20, 70),(\'Coursework 1\', 21, 20),(\'Written Exam\', 21, 80),(\'Coursework 1\', 22, 30),(\'Written Exam\', 22, 70)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Modules WHERE CourseId = 1 AND YearId = 1");
            Sql("DELETE FROM Courseworks WHERE ModuleId IN(14,15,16,17,18,19,20,21,22)");
        }
    }
}
