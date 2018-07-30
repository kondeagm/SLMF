namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableProTip : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProTip",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        Descripcion = c.String(nullable: false, maxLength: 250),
                        Autor = c.String(nullable: false, maxLength: 40),
                        VimeoID = c.String(nullable: false, maxLength: 50),
                        FileImage = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.ProTip");
        }
    }
}