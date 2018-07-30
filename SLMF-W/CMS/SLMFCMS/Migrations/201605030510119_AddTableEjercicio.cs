namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableEjercicio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ejercicio",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 25),
                        AccesorioID = c.Int(),
                        ElementoID = c.Int(),
                        PosicionID = c.Int(),
                        MusculoID = c.Int(nullable: false),
                        VimeoID = c.String(nullable: false, maxLength: 50),
                        FileImage = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Elemento", t => t.ElementoID)
                .ForeignKey("dbo.Musculo", t => t.MusculoID)
                .ForeignKey("dbo.Posicion", t => t.PosicionID)
                .ForeignKey("dbo.Accesorio", t => t.AccesorioID)
                .Index(t => t.AccesorioID)
                .Index(t => t.ElementoID)
                .Index(t => t.PosicionID)
                .Index(t => t.MusculoID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Ejercicio", "AccesorioID", "dbo.Accesorio");
            DropForeignKey("dbo.Ejercicio", "PosicionID", "dbo.Posicion");
            DropForeignKey("dbo.Ejercicio", "MusculoID", "dbo.Musculo");
            DropForeignKey("dbo.Ejercicio", "ElementoID", "dbo.Elemento");
            DropIndex("dbo.Ejercicio", new[] { "MusculoID" });
            DropIndex("dbo.Ejercicio", new[] { "PosicionID" });
            DropIndex("dbo.Ejercicio", new[] { "ElementoID" });
            DropIndex("dbo.Ejercicio", new[] { "AccesorioID" });
            DropTable("dbo.Ejercicio");
        }
    }
}