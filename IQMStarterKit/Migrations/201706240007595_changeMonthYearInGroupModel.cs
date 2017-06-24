namespace IQMStarterKit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeMonthYearInGroupModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GroupModels", "MonthIntake", c => c.Int(nullable: false));
            AlterColumn("dbo.GroupModels", "YearIntake", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GroupModels", "YearIntake", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.GroupModels", "MonthIntake", c => c.String(nullable: false, maxLength: 15));
        }
    }
}
