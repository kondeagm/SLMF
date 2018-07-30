namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableUnidadEjercicio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnidadEjercicio",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 15),
                        Abreviacion = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.UnidadEjercicio");
        }
    }
}