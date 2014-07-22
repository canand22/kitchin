using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(4)]
    public class M0004_AddNameColumnToKitchensTable : Migration
    {
        public override void Up()
        {
            this.Create.Column("Name").OnTable("ProductsOnKitchens").AsString(255).Nullable();
        }

        public override void Down()
        {
            this.Delete.Column("Name").FromTable("ProductsOnKitchens");
        }
    }
}