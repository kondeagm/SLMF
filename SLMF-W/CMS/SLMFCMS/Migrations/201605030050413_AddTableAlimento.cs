namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableAlimento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alimento",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 60),
                        NutrienteID = c.Int(nullable: false),
                        Suplemento = c.Boolean(nullable: false),
                        Preparado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Nutriente", t => t.NutrienteID)
                .Index(t => t.NutrienteID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Alimento", "NutrienteID", "dbo.Nutriente");
            DropIndex("dbo.Alimento", new[] { "NutrienteID" });
            DropTable("dbo.Alimento");
        }
    }
}