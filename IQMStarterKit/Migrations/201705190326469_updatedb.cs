namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatedb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempWorkbooks", "Version", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempWorkbooks", "Version");
        }
    }
}
