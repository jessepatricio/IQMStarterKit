namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFileInStudentActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentActivities", "FilePath_FileId", c => c.Byte());
            CreateIndex("dbo.StudentActivities", "FilePath_FileId");
            AddForeignKey("dbo.StudentActivities", "FilePath_FileId", "dbo.FilePaths", "FileId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentActivities", "FilePath_FileId", "dbo.FilePaths");
            DropIndex("dbo.StudentActivities", new[] { "FilePath_FileId" });
            DropColumn("dbo.StudentActivities", "FilePath_FileId");
        }
    }
}
