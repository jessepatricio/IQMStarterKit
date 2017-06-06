namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChangeUserID : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.FilePaths");
            AlterColumn("dbo.FilePaths", "FilePathId", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.FilePaths", "FilePathId");
            DropColumn("dbo.FilePaths", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FilePaths", "UserId", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.FilePaths");
            AlterColumn("dbo.FilePaths", "FilePathId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.FilePaths", "FilePathId");
        }
    }
}
