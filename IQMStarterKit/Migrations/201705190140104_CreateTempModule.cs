namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTempModule : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TempWorkbooks");
            DropColumn("dbo.TempWorkbooks", "Id");
            CreateTable(
                "dbo.TempModules",
                c => new
                    {
                        TempModuleId = c.Byte(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 1000),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Byte(nullable: false),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.Byte(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TempModuleId);
            
            AddColumn("dbo.TempWorkbooks", "TempWorkbookId", c => c.Byte(nullable: false, identity: true));
            AddPrimaryKey("dbo.TempWorkbooks", "TempWorkbookId");
           
        }
        
        public override void Down()
        {
            AddColumn("dbo.TempWorkbooks", "Id", c => c.Byte(nullable: false, identity: true));
            DropPrimaryKey("dbo.TempWorkbooks");
            DropColumn("dbo.TempWorkbooks", "TempWorkbookId");
            DropTable("dbo.TempModules");
            AddPrimaryKey("dbo.TempWorkbooks", "Id");
        }
    }
}
