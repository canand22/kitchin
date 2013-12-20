using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(10)]
    public class M0010_CreateStoresTable  :Migration
    {
        public override void Up()
        {
            this.Create.Table("Stores")
                .WithColumn("Id").AsInt64().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(255).Nullable()
                .WithColumn("Latitude").AsDouble().NotNullable()
                .WithColumn("Longitude").AsDouble().NotNullable();
        }

        public override void Down()
        {
            this.Delete.Table("Stores");
        }
    }
}