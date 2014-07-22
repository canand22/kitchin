using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(1)]
    public class M0001_CreateStoresAndCategoriesAndProductsTables : Migration
    {
        public override void Up()
        {
            this.Create.Table("Stores")
                .WithColumn("Id").AsInt64().PrimaryKey().NotNullable().Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Latitude").AsDouble().Nullable()
                .WithColumn("Longitude").AsDouble().Nullable();
            
            this.Create.Table("Categories")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("Pk_CatTab_ID").Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Description").AsString(255).Nullable();

            this.Create.Table("Products")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("Pk_ProdTab_ID").Identity()
                .WithColumn("UpcCode").AsString(255).Nullable()
                .WithColumn("ShortName").AsString(255).NotNullable()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("CategoryId").AsInt64().NotNullable()
                .WithColumn("StoreId").AsInt64().NotNullable();

            this.Create.ForeignKey("fk_ProdTab_CategId_CatTabId_Id")
            .FromTable("Products").ForeignColumn("CategoryId")
            .ToTable("Categories").PrimaryColumn("Id");

            this.Create.ForeignKey("fk_ProdTab_StorId_StorTabId_Id")
            .FromTable("Products").ForeignColumn("StoreId")
            .ToTable("Stores").PrimaryColumn("Id");
        }

        public override void Down()
        {
            this.Delete.Table("Stores");
            this.Delete.Table("Products");
            this.Delete.Table("Categories");
        }
    }
}
