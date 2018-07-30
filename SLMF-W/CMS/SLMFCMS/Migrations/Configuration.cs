namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SLMFCMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "SLMFCMS.Models.ApplicationDbContext";
        }

        protected override void Seed(SLMFCMS.Models.ApplicationDbContext context)
        {
        }
    }
}