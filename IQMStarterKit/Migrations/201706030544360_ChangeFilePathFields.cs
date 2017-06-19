namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangeFilePathFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FilePaths", "ContentType", c => c.String(maxLength: 100));
            AddColumn("dbo.FilePaths", "Content", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FilePaths", "Content");
            DropColumn("dbo.FilePaths", "ContentType");
        }
    }
}
