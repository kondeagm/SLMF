namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTablePregunta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pregunta",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CuestionarioID = c.Int(nullable: false),
                        Texto = c.String(nullable: false, maxLength: 10),
                        Descripcion = c.String(nullable: false, maxLength: 20),
                        Clase = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cuestionario", t => t.CuestionarioID, cascadeDelete: false)
                .Index(t => t.CuestionarioID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Pregunta", "CuestionarioID", "dbo.Cuestionario");
            DropIndex("dbo.Pregunta", new[] { "CuestionarioID" });
            DropTable("dbo.Pregunta");
        }
    }
}