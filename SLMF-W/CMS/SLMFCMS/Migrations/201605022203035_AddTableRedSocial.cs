namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableRedSocial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RedSocial",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 10),
                        Identificador = c.String(nullable: false, maxLength: 25),
                        URL = c.String(maxLength: 500),
                        APPId = c.String(maxLength: 30),
                        APIKey = c.String(maxLength: 500),
                        NoPost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.RedSocial");
        }
    }
}