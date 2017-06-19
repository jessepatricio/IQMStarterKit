namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTutorNameInGroupTutorTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupTutorModels", "TutorName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupTutorModels", "TutorName");
        }
    }
}
