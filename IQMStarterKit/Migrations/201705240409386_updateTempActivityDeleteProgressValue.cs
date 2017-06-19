namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateTempActivityDeleteProgressValue : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TempActivities", "ProgressValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TempActivities", "ProgressValue", c => c.Int(nullable: false));
        }
    }
}
