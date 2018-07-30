namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableProducto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        FileImage = c.String(maxLength: 250),
                        URL = c.String(maxLength: 500),
                        NutrienteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Nutriente", t => t.NutrienteID, cascadeDelete: false)
                .Index(t => t.NutrienteID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Producto", "NutrienteID", "dbo.Nutriente");
            DropIndex("dbo.Producto", new[] { "NutrienteID" });
            DropTable("dbo.Producto");
        }
    }
}