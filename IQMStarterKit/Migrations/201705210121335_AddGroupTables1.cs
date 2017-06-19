namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddGroupTables1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupModels",
                c => new
                    {
                        GroupId = c.Byte(nullable: false, identity: true),
                        GroupName = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 1000),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 100),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupModels", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            AddColumn("dbo.AspNetUsers", "UserGroupModel_Id", c => c.Byte());
            CreateIndex("dbo.AspNetUsers", "UserGroupModel_Id");
            AddForeignKey("dbo.AspNetUsers", "UserGroupModel_Id", "dbo.UserGroupModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserGroupModel_Id", "dbo.UserGroupModels");
            DropForeignKey("dbo.UserGroupModels", "GroupId", "dbo.GroupModels");
            DropIndex("dbo.AspNetUsers", new[] { "UserGroupModel_Id" });
            DropIndex("dbo.UserGroupModels", new[] { "GroupId" });
            DropColumn("dbo.AspNetUsers", "UserGroupModel_Id");
            DropTable("dbo.UserGroupModels");
            DropTable("dbo.GroupModels");
        }
    }
}
