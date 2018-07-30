namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTablePlanDiaEjercicios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlanDiaEjercicios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlanDiasID = c.Int(nullable: false),
                        Secuencia = c.Int(nullable: false),
                        EjercicioID = c.Int(nullable: false),
                        Series = c.Int(nullable: false),
                        Repeticiones = c.String(nullable: false, maxLength: 8),
                        UnidadEjercicioID = c.Int(nullable: false),
                        Descanso = c.Int(nullable: false),
                        Nivel = c.Int(nullable: false),
                        Nota = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Ejercicio", t => t.EjercicioID, cascadeDelete: false)
                .ForeignKey("dbo.PlanDias", t => t.PlanDiasID, cascadeDelete: false)
                .ForeignKey("dbo.UnidadEjercicio", t => t.UnidadEjercicioID, cascadeDelete: false)
                .Index(t => t.PlanDiasID)
                .Index(t => t.EjercicioID)
                .Index(t => t.UnidadEjercicioID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PlanDiaEjercicios", "UnidadEjercicioID", "dbo.UnidadEjercicio");
            DropForeignKey("dbo.PlanDiaEjercicios", "PlanDiasID", "dbo.PlanDias");
            DropForeignKey("dbo.PlanDiaEjercicios", "EjercicioID", "dbo.Ejercicio");
            DropIndex("dbo.PlanDiaEjercicios", new[] { "UnidadEjercicioID" });
            DropIndex("dbo.PlanDiaEjercicios", new[] { "EjercicioID" });
            DropIndex("dbo.PlanDiaEjercicios", new[] { "PlanDiasID" });
            DropTable("dbo.PlanDiaEjercicios");
        }
    }
}