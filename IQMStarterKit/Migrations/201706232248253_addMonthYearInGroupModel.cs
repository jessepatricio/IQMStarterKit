namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMonthYearInGroupModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupModels", "MonthIntake", c => c.String(nullable: false, maxLength: 15));
            AddColumn("dbo.GroupModels", "YearIntake", c => c.String(nullable: false, maxLength: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupModels", "YearIntake");
            DropColumn("dbo.GroupModels", "MonthIntake");
        }
    }
}
