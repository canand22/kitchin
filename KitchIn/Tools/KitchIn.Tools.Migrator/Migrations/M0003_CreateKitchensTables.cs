using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(3)]
    public class M0003_CreateKitchensTables : Migration
    {
        public override void Up()
        {
            this.Create.Table("ProductsOnKitchens")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("Pk_PrOnKitch_ID").Identity()
                .WithColumn("ProductId").AsInt64().Nullable()
                .WithColumn("UserId").AsInt64().Nullable()
                .WithColumn("DateOfPurchase").AsDate().Nullable()
                .WithColumn("Quantity").AsDouble().Nullable()
                .WithColumn("UnitType").AsInt32().Nullable();

            this.Create.ForeignKey("fk_PrOnKitchTab_ProductId_ProdTab_Id")
            .FromTable("ProductsOnKitchens").ForeignColumn("ProductId")
            .ToTable("Products").PrimaryColumn("Id");

            this.Create.ForeignKey("fk_PrOnKitchTab_UserId_KitchTab_Id")
            .FromTable("ProductsOnKitchens").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id");
        }

        public override void Down()
        {
            this.Delete.Table("ProductsOnKitchens");
        }
    }
}