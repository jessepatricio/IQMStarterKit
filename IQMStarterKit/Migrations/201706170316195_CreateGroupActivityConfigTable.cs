namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateGroupActivityConfigTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupActivityConfigs",
                c => new
                    {
                        GroupActivityConfigId = c.Byte(nullable: false, identity: true),
                        GroupId = c.Byte(nullable: false),
                        TempActivityId = c.Byte(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        Remarks = c.String(maxLength: 1000),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 100),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GroupActivityConfigId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GroupActivityConfigs");
        }
    }
}
