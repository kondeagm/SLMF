namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableCuestionario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cuestionario",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 15),
                        DisciplinaID = c.Int(nullable: false),
                        Visible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Disciplina", t => t.DisciplinaID)
                .Index(t => t.DisciplinaID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Cuestionario", "DisciplinaID", "dbo.Disciplina");
            DropIndex("dbo.Cuestionario", new[] { "DisciplinaID" });
            DropTable("dbo.Cuestionario");
        }
    }
}