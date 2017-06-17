namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIsRemovedFieldInConfig : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GroupActivityConfigs", "IsRemoved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupActivityConfigs", "IsRemoved", c => c.Boolean(nullable: false));
        }
    }
}
