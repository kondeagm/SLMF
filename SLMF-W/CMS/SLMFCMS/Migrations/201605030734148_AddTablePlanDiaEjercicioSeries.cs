namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTablePlanDiaEjercicioSeries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlanDiaEjercicioSeries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlanDiaEjerciciosID = c.Int(nullable: false),
                        Secuencia = c.Int(nullable: false),
                        Repeticiones = c.String(nullable: false, maxLength: 8),
                        Nivel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PlanDiaEjercicios", t => t.PlanDiaEjerciciosID, cascadeDelete: false)
                .Index(t => t.PlanDiaEjerciciosID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PlanDiaEjercicioSeries", "PlanDiaEjerciciosID", "dbo.PlanDiaEjercicios");
            DropIndex("dbo.PlanDiaEjercicioSeries", new[] { "PlanDiaEjerciciosID" });
            DropTable("dbo.PlanDiaEjercicioSeries");
        }
    }
}