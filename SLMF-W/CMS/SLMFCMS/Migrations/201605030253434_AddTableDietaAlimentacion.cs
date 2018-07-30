namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableDietaAlimentacion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DietaAlimentacion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DietaTemposID = c.Int(nullable: false),
                        Cantidad = c.String(maxLength: 5),
                        PorcionID = c.Int(),
                        AlimentoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Alimento", t => t.AlimentoID, cascadeDelete: false)
                .ForeignKey("dbo.DietaTempos", t => t.DietaTemposID, cascadeDelete: false)
                .ForeignKey("dbo.Porcion", t => t.PorcionID, cascadeDelete: false)
                .Index(t => t.DietaTemposID)
                .Index(t => t.PorcionID)
                .Index(t => t.AlimentoID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.DietaAlimentacion", "PorcionID", "dbo.Porcion");
            DropForeignKey("dbo.DietaAlimentacion", "DietaTemposID", "dbo.DietaTempos");
            DropForeignKey("dbo.DietaAlimentacion", "AlimentoID", "dbo.Alimento");
            DropIndex("dbo.DietaAlimentacion", new[] { "AlimentoID" });
            DropIndex("dbo.DietaAlimentacion", new[] { "PorcionID" });
            DropIndex("dbo.DietaAlimentacion", new[] { "DietaTemposID" });
            DropTable("dbo.DietaAlimentacion");
        }
    }
}