using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

using Castle.Windsor;

using NUnit.Framework;

using SharpTestsEx;

using SmartArch.Core.Domain.Base;
using KitchIn.Core.Entities;
using KitchIn.Data.NHibernate;
using KitchIn.ServiceLocation;
using KitchIn.Tests.MappingTests.Core;

namespace KitchIn.Tests.MappingTests
{
    public class CrudTests
    {
        /// <summary>
        /// The repository test
        /// </summary>
        private RepositoryForTests repository;

        /// <summary>
        /// The instances
        /// </summary>
        private IEnumerable<object> instances;

        /// <summary>
        /// Initialize the data base
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var container = new WindsorContainer();
            container.Install(new TestsNHibernateInstaller());
            CeDatabaseCreator.Create();
            CeDatabaseCreator.ExecuteMigrations();

            this.instances = new InstanceCreator<BaseEntity>(Assembly.LoadFrom("KitchIn.Core.dll")).GetInstances();
            this.repository = new RepositoryForTests();
        }

        /// <summary>
        /// Check for new record creation.
        /// </summary>
        /// <param name="type">The entity type</param>
        [Test]
        [TestCase(typeof(User))]
        public void CreateIsCorrect(Type type)
        {
            var instance = this.instances.First(x => x.GetType() == type);
            if (instance == null)
            {
                throw new Exception(string.Format("Type is not derived from {0}", InstanceCreator<BaseEntity>.BaseType));
            }

            Executing.This(() => this.repository.SaveInstance(instance)).Should().NotThrow();
        }

        /// <summary>
        /// Reading record from a database
        /// </summary>
        /// <param name="type"></param>
        [Test]
        [TestCase(typeof(User))]
        public void ReadIsCorrect(Type type)
        {
            Executing.This(() => this.repository.Get(type)).Should().NotThrow();
        }

        /// <summary>
        /// Delete record from a database
        /// </summary>
        /// <param name="type"></param>
        [Test]
        [TestCase(typeof(User))]
        public void DeleteIsCorrect(Type type)
        {
            var instance = this.instances.First(x => x.GetType() == type);
            Executing.This(() => this.repository.Delete(instance)).Should().NotThrow();
        }
    }
}