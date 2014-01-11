using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(2)]
    public class M0002_CreateUsersTable : Migration
    {
        public override void Up()
        {
            this.Create.Table("Users")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("Pk_UserTab_ID").Identity()
                .WithColumn("Email").AsString(255).Nullable()
                .WithColumn("Password").AsString(255).Nullable()
                .WithColumn("SessionId").AsGuid().Nullable()
                .WithColumn("Role").AsInt32().Nullable();
        }

        public override void Down()
        {
            this.Delete.Table("Users");
        }
    }
}