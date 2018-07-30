namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableUsuario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FacebookID = c.String(nullable: false, maxLength: 50),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Apellidos = c.String(maxLength: 80),
                        Correo = c.String(maxLength: 500),
                        PlanID = c.Int(),
                        FechaRegistro = c.DateTime(nullable: false),
                        FechaInicioPlan = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Plan", t => t.PlanID)
                .Index(t => t.PlanID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Usuario", "PlanID", "dbo.Plan");
            DropIndex("dbo.Usuario", new[] { "PlanID" });
            DropTable("dbo.Usuario");
        }
    }
}