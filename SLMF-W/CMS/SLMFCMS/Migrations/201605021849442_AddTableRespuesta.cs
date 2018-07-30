namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableRespuesta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Respuesta",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PreguntaID = c.Int(nullable: false),
                        Texto = c.String(nullable: false, maxLength: 15),
                        Clase = c.String(maxLength: 20),
                        Filtro = c.Boolean(nullable: false),
                        LogoSVG = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pregunta", t => t.PreguntaID, cascadeDelete: false)
                .Index(t => t.PreguntaID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Respuesta", "PreguntaID", "dbo.Pregunta");
            DropIndex("dbo.Respuesta", new[] { "PreguntaID" });
            DropTable("dbo.Respuesta");
        }
    }
}