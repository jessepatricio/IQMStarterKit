namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class create_activities_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempActivities",
                c => new
                    {
                        TempActivityId = c.Byte(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 1000),
                        SortBy = c.Int(nullable: false),
                        TempModuleId = c.Byte(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 100),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TempActivityId)
                .ForeignKey("dbo.TempModules", t => t.TempModuleId, cascadeDelete: true)
                .Index(t => t.TempModuleId);
            
            AddColumn("dbo.TempModules", "SortBy", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempActivities", "TempModuleId", "dbo.TempModules");
            DropIndex("dbo.TempActivities", new[] { "TempModuleId" });
            DropColumn("dbo.TempModules", "SortBy");
            DropTable("dbo.TempActivities");
        }
    }
}
