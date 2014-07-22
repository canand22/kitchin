using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(8)]
    public class M0008_CreateCategoryInStoresTable : Migration
    {
        public override void Up()
        {
            this.Create.Table("CategoryInStores")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("Pk_CatInStores_ID").Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("StoreId").AsInt64().NotNullable();

            this.Create.ForeignKey("fk_CatInStoresTab_StoreId_StoreTab_Id")
            .FromTable("CategoryInStores").ForeignColumn("StoreId")
            .ToTable("Stores").PrimaryColumn("Id");
        }

        public override void Down()
        {
            this.Delete.Table("CategoryInStores");
        }
    }
}