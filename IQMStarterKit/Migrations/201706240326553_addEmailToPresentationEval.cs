namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmailToPresentationEval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PresentationEvaluationModels", "StudentEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PresentationEvaluationModels", "StudentEmail");
        }
    }
}
