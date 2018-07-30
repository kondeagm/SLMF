namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTablePlanDias : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlanDias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlanID = c.Int(nullable: false),
                        Dia = c.Int(nullable: false),
                        RutinaID = c.Int(nullable: false),
                        DietaID = c.Int(nullable: false),
                        ProTipID = c.Int(nullable: false),
                        Descanso = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Dieta", t => t.DietaID, cascadeDelete: false)
                .ForeignKey("dbo.Plan", t => t.PlanID, cascadeDelete: false)
                .ForeignKey("dbo.ProTip", t => t.ProTipID, cascadeDelete: false)
                .ForeignKey("dbo.Rutina", t => t.RutinaID, cascadeDelete: false)
                .Index(t => t.PlanID)
                .Index(t => t.RutinaID)
                .Index(t => t.DietaID)
                .Index(t => t.ProTipID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PlanDias", "RutinaID", "dbo.Rutina");
            DropForeignKey("dbo.PlanDias", "ProTipID", "dbo.ProTip");
            DropForeignKey("dbo.PlanDias", "PlanID", "dbo.Plan");
            DropForeignKey("dbo.PlanDias", "DietaID", "dbo.Dieta");
            DropIndex("dbo.PlanDias", new[] { "ProTipID" });
            DropIndex("dbo.PlanDias", new[] { "DietaID" });
            DropIndex("dbo.PlanDias", new[] { "RutinaID" });
            DropIndex("dbo.PlanDias", new[] { "PlanID" });
            DropTable("dbo.PlanDias");
        }
    }
}