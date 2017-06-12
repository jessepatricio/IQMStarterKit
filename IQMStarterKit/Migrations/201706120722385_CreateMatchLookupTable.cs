namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMatchLookupTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatchLookupModels",
                c => new
                    {
                        MatchId = c.Byte(nullable: false, identity: true),
                        QuestionNumber = c.Int(nullable: false),
                        CorrectAnswer = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.MatchId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MatchLookupModels");
        }
    }
}
