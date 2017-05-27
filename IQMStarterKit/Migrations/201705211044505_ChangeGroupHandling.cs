namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGroupHandling : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGroupModels", "GroupId", "dbo.GroupModels");
            DropForeignKey("dbo.UserGroupModels", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserGroupModels", new[] { "GroupId" });
            DropIndex("dbo.UserGroupModels", new[] { "User_Id" });
            AddColumn("dbo.AspNetUsers", "GroupId", c => c.Byte(nullable: false));
            DropTable("dbo.UserGroupModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserGroupModels",
                c => new
                    {
                        Id = c.Byte(nullable: false, identity: true),
                        GroupId = c.Byte(nullable: false),
                        UserId = c.Byte(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 100),
                        IsRemoved = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.AspNetUsers", "GroupId");
            CreateIndex("dbo.UserGroupModels", "User_Id");
            CreateIndex("dbo.UserGroupModels", "GroupId");
            AddForeignKey("dbo.UserGroupModels", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserGroupModels", "GroupId", "dbo.GroupModels", "GroupId", cascadeDelete: true);
        }
    }
}
