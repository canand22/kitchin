using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(7)]
    public class M0007_AddCategoryColumnToKitchensTable : Migration
    {

        public override void Up()
        {
            this.Create.Column("CategoryId").OnTable("ProductsOnKitchens").AsInt64().Nullable();

            this.Create.ForeignKey("fk_PrOnKitchTab_CategoryId_CategoryTab_Id")
            .FromTable("ProductsOnKitchens").ForeignColumn("CategoryId")
            .ToTable("Categories").PrimaryColumn("Id");
        }

        public override void Down()
        {
            this.Delete.ForeignKey("fk_PrOnKitchTab_CategoryId_CategoryTab_Id").OnTable("ProductsOnKitchens");

            this.Delete.Column("CategoryId").FromTable("ProductsOnKitchens");
        }
    }
}