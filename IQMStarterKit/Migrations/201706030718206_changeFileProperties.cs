namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class changeFileProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentActivities", "FilePath_FilePathId", "dbo.FilePaths");
            DropIndex("dbo.StudentActivities", new[] { "FilePath_FilePathId" });
            DropPrimaryKey("dbo.FilePaths");
            DropColumn("dbo.FilePaths", "FilePathId");
            DropColumn("dbo.StudentActivities", "FilePath_FilePathId");
            AddColumn("dbo.FilePaths", "FileId", c => c.Byte(nullable: false, identity: true));
            AddPrimaryKey("dbo.FilePaths", "FileId");

        }

        public override void Down()
        {
            AddColumn("dbo.StudentActivities", "FilePath_FilePathId", c => c.Byte());
            AddColumn("dbo.FilePaths", "FilePathId", c => c.Byte(nullable: false));
            DropPrimaryKey("dbo.FilePaths");
            DropColumn("dbo.FilePaths", "FileId");
            AddPrimaryKey("dbo.FilePaths", "FilePathId");
            CreateIndex("dbo.StudentActivities", "FilePath_FilePathId");
            AddForeignKey("dbo.StudentActivities", "FilePath_FilePathId", "dbo.FilePaths", "FilePathId");
        }
    }
}
