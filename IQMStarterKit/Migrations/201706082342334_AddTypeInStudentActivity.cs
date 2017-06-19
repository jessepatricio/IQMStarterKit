namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTypeInStudentActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentActivities", "Type", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentActivities", "Type");
        }
    }
}
