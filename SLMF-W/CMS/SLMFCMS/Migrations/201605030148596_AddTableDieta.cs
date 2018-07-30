namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableDieta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dieta",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 25),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        FileImage = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.Dieta");
        }
    }
}