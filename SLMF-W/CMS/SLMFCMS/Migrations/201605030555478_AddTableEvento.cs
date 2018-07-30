namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableEvento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Evento",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 25),
                        Nombre = c.String(nullable: false, maxLength: 60),
                        Fecha = c.DateTime(nullable: false),
                        Lugar = c.String(nullable: false, maxLength: 50),
                        Direccion = c.String(nullable: false, maxLength: 150),
                        DisciplinaID = c.Int(nullable: false),
                        URL = c.String(maxLength: 500),
                        Visible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Disciplina", t => t.DisciplinaID, cascadeDelete: false)
                .Index(t => t.DisciplinaID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Evento", "DisciplinaID", "dbo.Disciplina");
            DropIndex("dbo.Evento", new[] { "DisciplinaID" });
            DropTable("dbo.Evento");
        }
    }
}