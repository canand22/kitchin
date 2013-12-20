using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(8)]
    public class M0008_AddBigOvenProductColumnToProductsTable : Migration
    {
        public override void Up()
        {
            this.Create.Column("ProductBigOven").OnTable("Products").AsString().Nullable();
        }

        public override void Down()
        {
            this.Delete.Column("ProductBigOven").FromTable("Products");
        }
    }
}