namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class tempModuleModelchanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TempActivities", "TempModuleId", "dbo.TempModules");
            DropIndex("dbo.TempActivities", new[] { "TempModuleId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.TempActivities", "TempModuleId");
            AddForeignKey("dbo.TempActivities", "TempModuleId", "dbo.TempModules", "TempModuleId", cascadeDelete: true);
        }
    }
}
