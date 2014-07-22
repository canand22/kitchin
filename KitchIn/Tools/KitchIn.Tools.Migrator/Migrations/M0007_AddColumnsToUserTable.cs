using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(7)]
    public class M0007_AddColumnsToUserTable : Migration
    {
        public override void Up()
        {
            this.Create.Column("FirstName").OnTable("Users").AsString(255).Nullable();
            this.Create.Column("LastName").OnTable("Users").AsString(255).Nullable();
        }

        public override void Down()
        {
            this.Delete.Column("FirstName").FromTable("Users");
            this.Delete.Column("LastName").FromTable("Users");
        }
    }
}