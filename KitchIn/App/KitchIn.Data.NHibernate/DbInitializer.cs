using System;

using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using KitchIn.Data.NHibernate.Mappings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

using SmartArch.Core.Domain.Base;

using KitchIn.Core;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate
{
    /// <summary>
    /// Represents class for <c>NHibernate</c> initializing.
    /// </summary>
    public class DbInitializer
    {
        /// <summary>
        /// Represents a connection key for accessing to DB.
        /// </summary>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbInitializer"/> class.
        /// </summary>
        /// <param name="connectionString">The connection key.</param>
        public DbInitializer(string connectionString)
        {
            this.ConnectionString = connectionString;

            this.Initialize();
        }

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>The factory.</value>
        public static ISessionFactory Factory { get; protected set; }

        /// <summary>
        /// Builds the factory.
        /// </summary>
        /// <returns>The session factory.</returns>
        private ISessionFactory BuildFactory()
        {
            Configuration cfg = this.GetConfiguration();
            return cfg.BuildSessionFactory();
        }

        /// <summary>
        /// Gets the <c>NHibernate</c> configuration.
        /// </summary>
        /// <returns>The <c>NHibernate</c> configuration.</returns>
        private Configuration GetConfiguration()
        {
            var config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                .ConnectionString(this.ConnectionString))
                .Mappings(m => m.AutoMappings.Add(CreateAutomappings))
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildConfiguration();

            return config;
        }

        private static AutoPersistenceModel CreateAutomappings()
        {
            return AutoMap.AssemblyOf<User>(t => t.BaseType == typeof(BaseEntity))
                .UseOverridesFromAssemblyOf<StoreMappingOverride>()
                .UseOverridesFromAssemblyOf<UserMappingOverride>()
                .UseOverridesFromAssemblyOf<CategoryMappingOverride>()
                .UseOverridesFromAssemblyOf<ProductMappingOverride>()
                .UseOverridesFromAssemblyOf<ProductsOnKitchenMappingOverride>()
                .UseOverridesFromAssemblyOf<FavoriteRecipeMappingOverride>()
                .IgnoreBase<BaseEntity>();
        }

        /// <summary>
        /// Opens the session.
        /// </summary>
        /// <returns>The <c>NHibernate</c> session.</returns>
        public virtual ISession OpenSession()
        {
            return Factory.OpenSession();
        }

        /// <summary>
        /// Initializes the session factory.
        /// </summary>
        public void Initialize()
        {
            try
            {
                Factory = this.BuildFactory();
                LogWriter.WriteInfo("NHibernate was configurated successfully!");
            }
            catch (Exception ex)
            {
                LogWriter.WriteError("NHibernate configuration failed: " + ex);
            }
        }
    }
}
