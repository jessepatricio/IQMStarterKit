namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserGroupTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "UserGroupModel_Id", "dbo.UserGroupModels");
            DropIndex("dbo.AspNetUsers", new[] { "UserGroupModel_Id" });
            AddColumn("dbo.UserGroupModels", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserGroupModels", "User_Id");
            AddForeignKey("dbo.UserGroupModels", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "UserGroupModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserGroupModel_Id", c => c.Byte());
            DropForeignKey("dbo.UserGroupModels", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserGroupModels", new[] { "User_Id" });
            DropColumn("dbo.UserGroupModels", "User_Id");
            CreateIndex("dbo.AspNetUsers", "UserGroupModel_Id");
            AddForeignKey("dbo.AspNetUsers", "UserGroupModel_Id", "dbo.UserGroupModels", "Id");
        }
    }
}
