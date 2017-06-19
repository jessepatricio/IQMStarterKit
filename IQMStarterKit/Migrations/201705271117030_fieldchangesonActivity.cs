namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class fieldchangesonActivity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TempActivities", "ProgressValue");
            DropColumn("dbo.TempActivities", "Context");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TempActivities", "Context", c => c.String());
            AddColumn("dbo.TempActivities", "ProgressValue", c => c.Int(nullable: false));
        }
    }
}
