namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableRutina : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rutina",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 30),
                        Descripcion = c.String(nullable: false, maxLength: 190),
                        GrupoMusculosID = c.Int(nullable: false),
                        Inactividad = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GrupoMusculos", t => t.GrupoMusculosID, cascadeDelete: false)
                .Index(t => t.GrupoMusculosID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Rutina", "GrupoMusculosID", "dbo.GrupoMusculos");
            DropIndex("dbo.Rutina", new[] { "GrupoMusculosID" });
            DropTable("dbo.Rutina");
        }
    }
}