namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addTutoFieldToGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupModels", "TutorId", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupModels", "TutorId");
        }
    }
}
