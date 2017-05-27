namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createStudentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentActivities",
                c => new
                    {
                        StudentActivityId = c.Byte(nullable: false, identity: true),
                        TempModuleId = c.Byte(nullable: false),
                        TempActivityId = c.Byte(nullable: false),
                        VarkResult = c.String(maxLength: 255),
                        DopeResult = c.String(maxLength: 255),
                        DiscResult = c.String(maxLength: 255),
                        NoMatchedWords = c.Int(nullable: false),
                        Top3PersonalValues = c.String(maxLength: 255),
                        PersonalLeaderShipScore = c.Int(nullable: false),
                        SelfManagementScore = c.Int(nullable: false),
                        AssertiveScore = c.Int(nullable: false),
                        CFDominantFirst = c.String(maxLength: 255),
                        CFDominantSecond = c.String(maxLength: 255),
                        ReviewQuizScore = c.Int(nullable: false),
                        ProgressValue = c.Int(nullable: false),
                        Context = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 100),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StudentActivityId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StudentActivities");
        }
    }
}
