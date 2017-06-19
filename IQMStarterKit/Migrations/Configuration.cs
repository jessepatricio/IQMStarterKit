namespace IQMStarterKit.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<IQMStarterKit.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IQMStarterKit.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //var tempworkbook = new TempWorkbook();
            //tempworkbook.Title = "IQM Module";
            //tempworkbook.Description = "PDP Module";
            //tempworkbook.Version = "1.0";
            //tempworkbook.CreatedBy = "2b2cc8e9-4fa6-4f51-a109-11a9dbd7b151";
            //tempworkbook.ModifiedBy = "2b2cc8e9-4fa6-4f51-a109-11a9dbd7b151";
            //tempworkbook.CreatedDateTime = DateTime.UtcNow;
            //tempworkbook.ModifiedDateTime = DateTime.UtcNow;

            //context.TempWorkbooks.Add(tempworkbook);
          



            //  Sql("INSERT INTO TempWorkBooks (Title, Description, Version, CreatedDateTime, CreatedBy, ModifiedDateTime, ModifiedBy) " +
            //"Values ('IQMWorkBook','PDP Workbook', '1.0', CAST('19/5/2017 14:00:00' AS DateTime), '2b2cc8e9-4fa6-4f51-a109-11a9dbd7b151', CAST('19/5/2017 14:00:00' AS DateTime), '2b2cc8e9-4fa6-4f51-a109-11a9dbd7b151');");


        }
    }
}
