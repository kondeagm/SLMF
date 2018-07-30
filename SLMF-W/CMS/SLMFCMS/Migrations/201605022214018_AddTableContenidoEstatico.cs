namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableContenidoEstatico : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContenidoEstatico",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RedSocialID = c.Int(nullable: false),
                        Identificador = c.String(nullable: false, maxLength: 300),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RedSocial", t => t.RedSocialID)
                .Index(t => t.RedSocialID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.ContenidoEstatico", "RedSocialID", "dbo.RedSocial");
            DropIndex("dbo.ContenidoEstatico", new[] { "RedSocialID" });
            DropTable("dbo.ContenidoEstatico");
        }
    }
}