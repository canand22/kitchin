using FluentMigrator;

using KitchIn.Tools.Migrator;

namespace KitchIn.Tests.MappingTests.Migrations
{
    /// <summary>
    /// Describes migration: Db schema creating
    /// </summary>
    [Migration(2)]
    public class M0002_InsertBaseValues : Migration
    {
        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            this.Insert.IntoTable("Role").Row(
              new
              {
                  Name = "Administrator"
              });

            this.Insert.IntoTable("User").Row(
              new
              {
                  Login = "admin",
                  Password = Program.GetHashString("qwerty"),
                  Email = "admin@a.com"
              });

            this.Insert.IntoTable("RoleUser").Row(
              new
              {
                  Role_id = 1,
                  User_id = 1
              });
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            this.Delete.FromTable("RoleUser");
            this.Delete.FromTable("User");
            this.Delete.FromTable("Role");
        }
    }
}
