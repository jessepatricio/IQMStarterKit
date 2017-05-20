namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPageNameToActivities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempActivities", "PageName", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempActivities", "PageName");
        }
    }
}
