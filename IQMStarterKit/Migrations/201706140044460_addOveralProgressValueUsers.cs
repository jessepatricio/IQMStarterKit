namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addOveralProgressValueUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "OverallProgress", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "OverallProgress");
        }
    }
}
