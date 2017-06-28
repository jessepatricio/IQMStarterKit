namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTutorIdToPresentationEval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PresentationEvaluationModels", "TutorId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PresentationEvaluationModels", "TutorId");
        }
    }
}
