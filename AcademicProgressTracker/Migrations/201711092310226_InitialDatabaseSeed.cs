namespace AcademicProgressTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseSeed : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO AspNetRoles VALUES (\'Admin\'), (\'Standard\')");

            Sql("INSERT INTO AspNetUsers VALUES(\'Jordan\', \'Proyer\', \'jordan.proyer@hotmail.co.uk\', 0, \'AHDWrnmy7vjamIEsIp3QtwKvAzck+dKh/uJxvmcVBKo7gVK6pMrzABxhMPsk40Q7NQ==\', \'0f512a8e-c448-4d8e-b446-cfdc5841ff38\', \'07958328293\', 0, 0, NULL, 0, 0, \'Jordan Proyer\')");

            Sql("INSERT INTO AspNetUserRoles VALUES (1, 1)");

            Sql("INSERT INTO Classifications VALUES (\'First Class\', \'1st\', 70, 100), (\'Second Class Division 1\', \'2:1\', 60, 69), " +
                "(\'Second Class Division 2\', \'2:2\', 50, 59), (\'Third Class\', \'3rd\', 40, 49), (\'Fail\', \'Fail\', 0, 39)");

            Sql("INSERT INTO Courses VALUES (\'Computer Science\', \'BSc\', 3)");

            Sql("INSERT INTO Years VALUES(\'Year 1\'), (\'Year 2\'), (\'Year 3\'), (\'Year 4\')");

            Sql("INSERT INTO Modules VALUES (\'Individual Project\', \'CS3IP16\', 1, 3, 40, 0), (\'Social, Legal and Ethical Aspects of Science and Engineering\', \'CS3SL16\', 1, 3, 10, 0),(\'Advanced Computing\', \'CS3AC16\', 1, 3, 10, 1), (\'Computer Networking\', \'CS3CN16\', 1, 3, 10, 1),(\'Image Analysis\', \'CS3IA16 \', 1, 3, 10, 1), (\'Requirements, Domains and Soft Systems\', \'CS3RD16\', 1, 3, 10, 1),(\'Concurrent Systems\', \'CS3CS16\', 1, 3, 10, 1), (\'Virtual Reality\', \'CS3VR16\', 1, 3, 10, 1),(\'Evolutionary Computation\', \'CS3EC16\', 1, 3, 10, 1), (\'Software Quality and Testing\', \'CS3SQ16\', 1, 3, 10, 1),(\'Data Mining\', \'CS3DM16\', 1, 3, 10, 1), (\'Information Security\', \'CS3IS16\', 1, 3, 10, 1),(\'Informatics for E-Enterprise\', \'MM374\', 1, 3, 20, 1)");
            
            Sql("INSERT INTO Courseworks VALUES (\'Report\', 1, 60), (\'Oral assessment and presentation\', 1, 40),(\'Abstract Writing\', 2, 5), (\'Abstact Critique\', 2, 10), (\'Academic paper\', 2, 20), (\'Group Report\', 2, 20), (\'Group Video\', 2, 20), (\'Online Test\', 2, 15), (\'Reflective Piece\', 2, 10),(\'Set Exercise\', 3, 30), (\'Written Exam\', 3, 70),(\'Group Project\', 4, 50), (\'Written Exam\', 4, 50),(\'CourseWork 1\', 5, 15), (\'CourseWork 2\', 5, 15), (\'Written Exam\', 5, 70),(\'Written Exam\', 6, 100),(\'Coursework 1\', 7, 30), (\'Written Exam\', 7, 70),(\'Coursework 1\', 8, 30), (\'Written Exam\', 8, 70),(\'Coursework 1\', 9, 30), (\'Written Exam\', 9, 70),(\'Test Plan\', 10, 10), (\'Report\', 10, 20), (\'Written Exam\', 10, 70),(\'CourseWork 1\', 11, 50), (\'Written Exam\', 11, 50),(\'Coursework 1\', 12, 30), (\'Written Exam\', 12, 70),(\'Essay\', 13, 30), (\'Written Exam\', 13, 70)");                   
        }
        
        public override void Down()
        {
            Sql("DELETE FROM AspNetUserRoles WHERE UserId = 1 DELETE FROM AspNetUsers WHERE Id = 1 DELETE FROM AspNetRoles WHERE Id IN (1, 2) DELETE FROM Classifications WHERE Id IN (1, 2, 3, 4, 5) DELETE FROM Courseworks DELETE FROM Modules DELETE FROM Years DELETE FROM Courses");
        }
    }
}
