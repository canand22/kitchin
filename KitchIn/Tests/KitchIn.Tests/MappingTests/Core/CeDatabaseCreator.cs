using System;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;

using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;

using KitchIn.Tools.Migrator;

namespace KitchIn.Tests.MappingTests.Core
{
    public static class CeDatabaseCreator
    {
        /// <summary>
        /// Describes DataBase type
        /// </summary>
        private const string DATABASE = "SqlServer2008";

        /// <summary>
        /// Describes migrate task
        /// </summary>
        private const string TASK_MIGRATE = "migrate";

        /// <summary>
        /// /// Describes connection name
        /// </summary>
        private const string CONNECTION_NAME = "SmartArchTemplateDb";

        private static readonly string DbPath;

        static CeDatabaseCreator()
        {
            var currLocationPath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).FullName;
            var currDir = new DirectoryInfo(Path.Combine(currLocationPath, "Databases"));
            if (!currDir.Exists)
            {
                currDir.Create();
            }

            DbPath = Path.Combine(currDir.FullName, "PDRce.sdf");
        }

        public static string ConnectionString
        {
            get
            {
                return string.Format(@"Data Source={0}", DbPath);
            }
        }

        public static void Create()
        {
            if (File.Exists(DbPath))
            {
                File.Delete(DbPath);
            }

            var engine = new SqlCeEngine(ConnectionString);
            engine.CreateDatabase();
        }

        public static void ExecuteMigrations()
        {
            var connection = ConfigurationManager.ConnectionStrings[CONNECTION_NAME].ConnectionString;

            var consoleAnnouncer = new TextWriterAnnouncer(Console.Out)
              {
                  ShowElapsedTime = false,
                  ShowSql = false
              };

            var runnerContext = new RunnerContext(consoleAnnouncer)
              {
                  Database = DATABASE,
                  Connection = connection,
                  Target = typeof(Program).Assembly.Location,
                  PreviewOnly = false,
                  Task = TASK_MIGRATE
              };

            new TaskExecutor(runnerContext);
        }
    }
}
