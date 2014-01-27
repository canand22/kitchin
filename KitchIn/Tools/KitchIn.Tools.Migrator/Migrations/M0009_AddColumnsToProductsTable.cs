using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(9)]
    public class M0009_AddColumnsToProductsTable : Migration
    {

        public override void Up()
        {
            this.Create.Column("IngredientName").OnTable("Products").AsString(255).Nullable();
            this.Create.Column("TypeAdd").OnTable("Products").AsString(30).NotNullable();
            this.Create.Column("ModificationDate").OnTable("Products").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            this.Delete.Column("IngredientName").FromTable("Products");
            this.Delete.Column("TypeAdd").FromTable("Products");
            this.Delete.Column("ModificationDate").FromTable("Products");
        }
    }
}