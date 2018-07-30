namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTablePosicion : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Musculoes", newName: "Musculo");
            CreateTable(
                "dbo.Posicion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        Abreviacion = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.Posicion");
            RenameTable(name: "dbo.Musculo", newName: "Musculoes");
        }
    }
}