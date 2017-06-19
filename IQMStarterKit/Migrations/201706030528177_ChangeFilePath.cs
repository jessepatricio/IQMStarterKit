namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangeFilePath : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FilePaths", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.FilePaths", new[] { "User_Id" });
            AddColumn("dbo.FilePaths", "StudentActivityId", c => c.Byte(nullable: false));
            AddColumn("dbo.StudentActivities", "FilePath_FilePathId", c => c.Byte());
            CreateIndex("dbo.StudentActivities", "FilePath_FilePathId");
            AddForeignKey("dbo.StudentActivities", "FilePath_FilePathId", "dbo.FilePaths", "FilePathId");
            DropColumn("dbo.FilePaths", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FilePaths", "User_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.StudentActivities", "FilePath_FilePathId", "dbo.FilePaths");
            DropIndex("dbo.StudentActivities", new[] { "FilePath_FilePathId" });
            DropColumn("dbo.StudentActivities", "FilePath_FilePathId");
            DropColumn("dbo.FilePaths", "StudentActivityId");
            CreateIndex("dbo.FilePaths", "User_Id");
            AddForeignKey("dbo.FilePaths", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
