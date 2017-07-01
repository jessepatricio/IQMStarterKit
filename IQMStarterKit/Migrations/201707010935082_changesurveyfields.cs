namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesurveyfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProgramSurveyModels", "POverall", c => c.Int(nullable: false));
            AlterColumn("dbo.ProgramSurveyModels", "PTimeAllocated", c => c.Int(nullable: false));
            AlterColumn("dbo.ProgramSurveyModels", "PClassSize", c => c.Int(nullable: false));
            AlterColumn("dbo.ProgramSurveyModels", "PClassroom", c => c.Int(nullable: false));
            AlterColumn("dbo.TutorSurveyModels", "TOverall", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TutorSurveyModels", "TOverall", c => c.String());
            AlterColumn("dbo.ProgramSurveyModels", "PClassroom", c => c.String());
            AlterColumn("dbo.ProgramSurveyModels", "PClassSize", c => c.String());
            AlterColumn("dbo.ProgramSurveyModels", "PTimeAllocated", c => c.String());
            AlterColumn("dbo.ProgramSurveyModels", "POverall", c => c.String());
        }
    }
}
