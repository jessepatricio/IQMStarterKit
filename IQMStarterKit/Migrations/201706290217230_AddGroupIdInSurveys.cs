namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupIdInSurveys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProgramSurveyModels", "GroupId", c => c.Byte(nullable: false));
            AddColumn("dbo.TutorSurveyModels", "GroupId", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TutorSurveyModels", "GroupId");
            DropColumn("dbo.ProgramSurveyModels", "GroupId");
        }
    }
}
