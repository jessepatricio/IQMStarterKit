namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addFieldsToTempActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempActivities", "ProgressValue", c => c.Int(nullable: false));
            AddColumn("dbo.TempActivities", "Context", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempActivities", "Context");
            DropColumn("dbo.TempActivities", "ProgressValue");
        }
    }
}
