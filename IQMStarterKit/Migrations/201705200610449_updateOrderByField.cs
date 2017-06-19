namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateOrderByField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempActivities", "SortOrder", c => c.Int(nullable: false));
            AddColumn("dbo.TempModules", "SortOrder", c => c.Int(nullable: false));
            DropColumn("dbo.TempActivities", "SortBy");
            DropColumn("dbo.TempModules", "SortBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TempModules", "SortBy", c => c.Int(nullable: false));
            AddColumn("dbo.TempActivities", "SortBy", c => c.Int(nullable: false));
            DropColumn("dbo.TempModules", "SortOrder");
            DropColumn("dbo.TempActivities", "SortOrder");
        }
    }
}
