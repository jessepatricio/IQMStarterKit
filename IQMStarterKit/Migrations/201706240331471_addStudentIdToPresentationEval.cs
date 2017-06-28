namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStudentIdToPresentationEval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PresentationEvaluationModels", "StudentId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PresentationEvaluationModels", "StudentId");
        }
    }
}
