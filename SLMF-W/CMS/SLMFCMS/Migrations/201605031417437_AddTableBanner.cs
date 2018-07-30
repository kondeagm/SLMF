namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableBanner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banner",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Identificador = c.String(nullable: false, maxLength: 60),
                        FileImage = c.String(maxLength: 250),
                        LinkBanner = c.String(nullable: false, maxLength: 500),
                        Prioridad = c.Int(nullable: false),
                        Visible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.Banner");
        }
    }
}