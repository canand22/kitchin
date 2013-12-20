using System;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;

namespace KitchIn.Tools.Migrator
{
    using System.Linq;

    /// <summary>
    /// Application main class
    /// </summary>
    public class Program
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
        /// Describes rollback task
        /// </summary>
        private const string TASK_ROLLBACK = "rollback";

        /// <summary>
        /// Describes rollback:all task
        /// </summary>
        private const string TASK_ROLLBACK_ALL = "rollback:all";

        /// <summary>
        /// /// Describes rollback:toversion task
        /// </summary>
        private const string TASK_ROLLBACK_TOVERSION = "rollback:toversion";

        /// <summary>
        /// /// Describes connection name
        /// </summary>
        private const string CONNECTION_NAME = "DbConnection";

        /// <summary>
        /// Convert string to hash-code
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The hash-code
        /// </returns>
        public static Guid GetHashString(string password)
        {
            var bytes = Encoding.Unicode.GetBytes(password);
            var csp = new MD5CryptoServiceProvider();
            var byteHash = csp.ComputeHash(bytes);

            var hash = byteHash.Aggregate(string.Empty, (current, b) => current + string.Format("{0:x2}", b));

            return new Guid(hash);
        }

        /// <summary>
        /// Application main method
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main(string[] args)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings[CONNECTION_NAME].ConnectionString;

                var consoleAnnouncer = new TextWriterAnnouncer(Console.Out)
                {
                    ShowElapsedTime = false,
                    ShowSql = true
                };

                var runnerContext = new RunnerContext(consoleAnnouncer)
                {
                    Database = DATABASE,
                    Connection = connection,
                    Target = typeof(Program).Assembly.Location,
                    PreviewOnly = false,
                    Task = TASK_MIGRATE
                };

                string selectedOption;

                if (args.Length > 0)
                {
                    selectedOption = args[0];
                }
                else
                {
                    Console.WriteLine("ConnectionString: [{0}]\n", connection);

                    Console.WriteLine("Tasks for migrator:");
                    Console.WriteLine("1: \"migrate\" task. (Migrate to the latest version)");
                    Console.WriteLine("2: \"rollback\" task. (Migrate back one version)");
                    Console.WriteLine("3: \"rollback:all\" task. (Migrate back to original state prior to applying migrations)");
                    Console.WriteLine("4: \"rollback:toversion\" task. (Migrate to the specific version)");
                    Console.Write("Enter task number: ");
                    var key = Console.ReadKey();
                    Console.WriteLine();

                    selectedOption = key.KeyChar.ToString(CultureInfo.InvariantCulture);
                }

                switch (selectedOption)
                {
                    case "1":
                        break;
                    case "2":
                        runnerContext.Task = TASK_ROLLBACK;
                        break;
                    case "3":
                        runnerContext.Task = TASK_ROLLBACK_ALL;
                        break;
                    case "4":
                        Console.WriteLine("Enter number of version for migration:");
                        var versionKey = Console.ReadKey();
                        Console.WriteLine();

                        int version;
                        if (int.TryParse(versionKey.KeyChar.ToString(CultureInfo.InvariantCulture), out version))
                        {
                            runnerContext.Task = TASK_ROLLBACK_TOVERSION;
                            runnerContext.Version = version;
                        }

                        break;
                }

                new TaskExecutor(runnerContext).Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
