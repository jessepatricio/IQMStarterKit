namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeMonthYearTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MonthIntake", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "YearIntake", c => c.Int(nullable: false));
            DropColumn("dbo.GroupModels", "MonthIntake");
            DropColumn("dbo.GroupModels", "YearIntake");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupModels", "YearIntake", c => c.Int(nullable: false));
            AddColumn("dbo.GroupModels", "MonthIntake", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "YearIntake");
            DropColumn("dbo.AspNetUsers", "MonthIntake");
        }
    }
}
