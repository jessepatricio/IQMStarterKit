namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateWorkbook : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempWorkbooks",
                c => new
                    {
                        Id = c.Byte(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 1000),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Byte(nullable: false),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedBy = c.Byte(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempWorkbooks");
        }
    }
}
