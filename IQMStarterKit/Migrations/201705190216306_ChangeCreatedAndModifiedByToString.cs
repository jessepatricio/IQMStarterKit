namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCreatedAndModifiedByToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TempModules", "CreatedBy", c => c.String(nullable: false));
            AlterColumn("dbo.TempModules", "ModifiedBy", c => c.String(nullable: false));
            AlterColumn("dbo.TempWorkbooks", "CreatedBy", c => c.String(nullable: false));
            AlterColumn("dbo.TempWorkbooks", "ModifiedBy", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempWorkbooks", "ModifiedBy", c => c.Byte(nullable: false));
            AlterColumn("dbo.TempWorkbooks", "CreatedBy", c => c.Byte(nullable: false));
            AlterColumn("dbo.TempModules", "ModifiedBy", c => c.Byte(nullable: false));
            AlterColumn("dbo.TempModules", "CreatedBy", c => c.Byte(nullable: false));
        }
    }
}
