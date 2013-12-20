using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(3)]
    public class M0003_CreateCategoriesAndProductsTables : Migration
    {
        public override void Up()
        {
            this.Create.Table("Categories")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("Pk_CatTab_ID").Identity()
                .WithColumn("Name").AsString(255).Nullable()
                .WithColumn("Description").AsString(255).Nullable();

            this.Create.Table("Products")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("Pk_ProdTab_ID").Identity()
                .WithColumn("Name").AsString(255).Nullable()
                .WithColumn("Variants").AsString(255).Nullable()
                .WithColumn("CategoryId").AsInt64().Nullable()
                .WithColumn("ExpirationDate").AsString(255).Nullable()
                .WithColumn("IsAddedbyUser").AsBoolean().Nullable();

            this.Create.ForeignKey("fk_ProdTab_CategId_CatTabId_Id")
            .FromTable("Products").ForeignColumn("CategoryId")
            .ToTable("Categories").PrimaryColumn("Id");
        }

        public override void Down()
        {
            this.Delete.Table("Products");
            this.Delete.Table("Categories");
        }
    }
}
