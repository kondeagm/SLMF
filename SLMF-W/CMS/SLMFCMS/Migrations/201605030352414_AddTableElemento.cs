namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableElemento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Elemento",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 22),
                        Abreviacion = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.Elemento");
        }
    }
}