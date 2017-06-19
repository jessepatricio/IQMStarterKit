namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSystemfieldFilePathClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FilePaths", "CreatedDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.FilePaths", "CreatedBy", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FilePaths", "CreatedBy");
            DropColumn("dbo.FilePaths", "CreatedDateTime");
        }
    }
}
