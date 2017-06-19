namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateTempActivityAddProgressValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempActivities", "ProgressValue", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempActivities", "ProgressValue");
        }
    }
}
