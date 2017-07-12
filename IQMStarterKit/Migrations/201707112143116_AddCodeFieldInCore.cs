namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodeFieldInCore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempActivities", "Code", c => c.String(maxLength: 3));
            AddColumn("dbo.TempModules", "Code", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempModules", "Code");
            DropColumn("dbo.TempActivities", "Code");
        }
    }
}
