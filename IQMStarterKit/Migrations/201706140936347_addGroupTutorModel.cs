namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addGroupTutorModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupTutorModels",
                c => new
                    {
                        GroupTutorId = c.Byte(nullable: false, identity: true),
                        GroupId = c.Byte(nullable: false),
                        TutorId = c.String(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 100),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GroupTutorId);
            
            DropColumn("dbo.GroupModels", "TutorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupModels", "TutorId", c => c.String(maxLength: 100));
            DropTable("dbo.GroupTutorModels");
        }
    }
}
