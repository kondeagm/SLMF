namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableMusculo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Musculoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        NombreComun = c.String(nullable: false, maxLength: 15),
                        Descripcion = c.String(nullable: false, maxLength: 190),
                        GrupoMusculosID = c.Int(nullable: false),
                        FileImage = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GrupoMusculos", t => t.GrupoMusculosID, cascadeDelete: false)
                .Index(t => t.GrupoMusculosID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Musculoes", "GrupoMusculosID", "dbo.GrupoMusculos");
            DropIndex("dbo.Musculoes", new[] { "GrupoMusculosID" });
            DropTable("dbo.Musculoes");
        }
    }
}