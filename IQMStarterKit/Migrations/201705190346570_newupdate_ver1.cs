namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class newupdate_ver1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TempModules", "CreatedBy", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.TempModules", "ModifiedBy", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.TempWorkbooks", "CreatedBy", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.TempWorkbooks", "ModifiedBy", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempWorkbooks", "ModifiedBy", c => c.String(nullable: false));
            AlterColumn("dbo.TempWorkbooks", "CreatedBy", c => c.String(nullable: false));
            AlterColumn("dbo.TempModules", "ModifiedBy", c => c.String(nullable: false));
            AlterColumn("dbo.TempModules", "CreatedBy", c => c.String(nullable: false));
        }
    }
}
