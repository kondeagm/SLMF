namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTableDietaTempos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DietaTempos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DietaID = c.Int(nullable: false),
                        TempoID = c.Int(nullable: false),
                        Hora = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Dieta", t => t.DietaID, cascadeDelete: false)
                .ForeignKey("dbo.Tempo", t => t.TempoID, cascadeDelete: false)
                .Index(t => t.DietaID)
                .Index(t => t.TempoID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.DietaTempos", "TempoID", "dbo.Tempo");
            DropForeignKey("dbo.DietaTempos", "DietaID", "dbo.Dieta");
            DropIndex("dbo.DietaTempos", new[] { "TempoID" });
            DropIndex("dbo.DietaTempos", new[] { "DietaID" });
            DropTable("dbo.DietaTempos");
        }
    }
}