namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTablePlanEtiquetas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlanEtiquetas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlanID = c.Int(nullable: false),
                        RespuestaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Plan", t => t.PlanID, cascadeDelete: false)
                .ForeignKey("dbo.Respuesta", t => t.RespuestaID, cascadeDelete: false)
                .Index(t => t.PlanID)
                .Index(t => t.RespuestaID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PlanEtiquetas", "RespuestaID", "dbo.Respuesta");
            DropForeignKey("dbo.PlanEtiquetas", "PlanID", "dbo.Plan");
            DropIndex("dbo.PlanEtiquetas", new[] { "RespuestaID" });
            DropIndex("dbo.PlanEtiquetas", new[] { "PlanID" });
            DropTable("dbo.PlanEtiquetas");
        }
    }
}