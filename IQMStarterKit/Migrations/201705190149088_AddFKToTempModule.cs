namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKToTempModule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempModules", "TempWorkbookId", c => c.Byte(nullable: false));
            CreateIndex("dbo.TempModules", "TempWorkbookId");
            AddForeignKey("dbo.TempModules", "TempWorkbookId", "dbo.TempWorkbooks", "TempWorkbookId", cascadeDelete: true);

           
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempModules", "TempWorkbookId", "dbo.TempWorkbooks");
            DropIndex("dbo.TempModules", new[] { "TempWorkbookId" });
            DropColumn("dbo.TempModules", "TempWorkbookId");
        }
    }
}
