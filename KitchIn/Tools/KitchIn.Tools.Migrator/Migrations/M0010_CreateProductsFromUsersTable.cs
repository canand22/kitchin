using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(10)]
    public class M0010_CreateProductsFromUsersTable : Migration
    {
        public override void Up()
        {
            this.Create.Table("ProductsFromUsers")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("Pk_ProdFrUsTab_ID").Identity()
                .WithColumn("UpcCode").AsString(255).Nullable()
                .WithColumn("ShortName").AsString(255).Nullable()
                .WithColumn("Name").AsString(255).Nullable()
                .WithColumn("IngredientName").AsString(255).Nullable()
                .WithColumn("CategoryId").AsInt64().Nullable()
                .WithColumn("StoreId").AsInt64().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("ExpirationDate").AsInt32().Nullable()
                .WithColumn("Date").AsDateTime().NotNullable();


            this.Create.ForeignKey("fk_ProdFrUsTab_CategId_CatTab_Id")
            .FromTable("ProductsFromUsers").ForeignColumn("CategoryId")
            .ToTable("Categories").PrimaryColumn("Id");

            this.Create.ForeignKey("fk_ProdFrUsTab_StorId_StorTab_Id")
            .FromTable("ProductsFromUsers").ForeignColumn("StoreId")
            .ToTable("Stores").PrimaryColumn("Id");

            this.Create.ForeignKey("fk_ProdFrUsTab_UserId_UsersTab_Id")
            .FromTable("ProductsFromUsers").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id");
        }

        public override void Down()
        {
            this.Delete.Table("ProductsFromUsers");
        }
    }
}