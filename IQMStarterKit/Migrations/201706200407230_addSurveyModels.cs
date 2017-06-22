namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSurveyModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PresentationEvaluationModels",
                c => new
                    {
                        PresentationId = c.Byte(nullable: false, identity: true),
                        StudentName = c.String(),
                        Topic = c.String(),
                        Grooming = c.String(),
                        Content = c.String(),
                        Delivery = c.String(),
                        Visual = c.String(),
                        Knowledge = c.String(),
                        Timing = c.String(),
                        Comment = c.String(),
                        TutorName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PresentationId);
            
            CreateTable(
                "dbo.ProgramSurveyModels",
                c => new
                    {
                        ProgramSurveyId = c.Byte(nullable: false, identity: true),
                        P1 = c.Int(nullable: false),
                        P2 = c.Int(nullable: false),
                        P3 = c.Int(nullable: false),
                        P4 = c.Int(nullable: false),
                        P5 = c.Int(nullable: false),
                        P6 = c.Int(nullable: false),
                        P7 = c.Int(nullable: false),
                        POverall = c.String(),
                        PTimeAllocated = c.String(),
                        PClassSize = c.String(),
                        PClassroom = c.String(),
                        PComment = c.String(),
                        CreatedBy = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProgramSurveyId);
            
            CreateTable(
                "dbo.TutorSurveyModels",
                c => new
                    {
                        TutorSurveyId = c.Byte(nullable: false, identity: true),
                        TutorId = c.String(),
                        T1 = c.Int(nullable: false),
                        T2 = c.Int(nullable: false),
                        T3 = c.Int(nullable: false),
                        T4 = c.Int(nullable: false),
                        T5 = c.Int(nullable: false),
                        T6 = c.Int(nullable: false),
                        T7 = c.Int(nullable: false),
                        T8 = c.Int(nullable: false),
                        TOverall = c.String(),
                        TComment = c.String(),
                        CreatedBy = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TutorSurveyId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TutorSurveyModels");
            DropTable("dbo.ProgramSurveyModels");
            DropTable("dbo.PresentationEvaluationModels");
        }
    }
}
