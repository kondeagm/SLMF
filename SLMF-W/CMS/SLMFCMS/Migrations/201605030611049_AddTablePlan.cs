namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTablePlan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Plan",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DisciplinaID = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 15),
                        Leyenda = c.String(nullable: false, maxLength: 35),
                        Descripcion = c.String(nullable: false, maxLength: 150),
                        FileImage = c.String(maxLength: 250),
                        Visible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Disciplina", t => t.DisciplinaID, cascadeDelete: false)
                .Index(t => t.DisciplinaID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Plan", "DisciplinaID", "dbo.Disciplina");
            DropIndex("dbo.Plan", new[] { "DisciplinaID" });
            DropTable("dbo.Plan");
        }
    }
}