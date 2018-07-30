namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableDisciplina : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Disciplina",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 15),
                        Siglas = c.String(nullable: false, maxLength: 3),
                        Slogan = c.String(nullable: false, maxLength: 45),
                        Proposito = c.String(nullable: false, maxLength: 100),
                        FileImage = c.String(maxLength: 250),
                        IconImage = c.String(maxLength: 250),
                        LogoSVG = c.String(),
                        SiglasImage = c.String(maxLength: 250),
                        ColorCode = c.String(nullable: false, maxLength: 7),
                        ImageEntrenar = c.String(maxLength: 250),
                        Visible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.Disciplina");
        }
    }
}