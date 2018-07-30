namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableNutriente : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Nutriente",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 15),
                        ColorCode = c.String(nullable: false, maxLength: 7),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Porcion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 15),
                        Abreviacion = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.Porcion");
            DropTable("dbo.Nutriente");
        }
    }
}