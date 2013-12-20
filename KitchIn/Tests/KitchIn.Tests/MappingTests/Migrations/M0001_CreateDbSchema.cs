using FluentMigrator;

namespace KitchIn.Tests.MappingTests.Migrations
{
    /// <summary>
    /// Describes migration: Db schema creating
    /// </summary>
    [Migration(1)]
    public class M0001_CreateDbSchema : Migration
    {
        /// <summary>
        /// Describes create Db script name
        /// </summary>
        private const string CREATE_DBSCHEME_SCRIPT_NAME = "CreateScheme.sql";

        /// <summary>
        /// Describes remove Db script name
        /// </summary>
        private const string REMOVE_DBSCHEME_SCRIPT_NAME = "RemoveScheme.sql";

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            this.Execute.EmbeddedScript(CREATE_DBSCHEME_SCRIPT_NAME);
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            this.Execute.EmbeddedScript(REMOVE_DBSCHEME_SCRIPT_NAME);
        }
    }
}
