namespace SLMFCMS.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateTableDisciplina : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Disciplina", "Nombre", c => c.String(nullable: false, maxLength: 18));
        }

        public override void Down()
        {
            AlterColumn("dbo.Disciplina", "Nombre", c => c.String(nullable: false, maxLength: 15));
        }
    }
}