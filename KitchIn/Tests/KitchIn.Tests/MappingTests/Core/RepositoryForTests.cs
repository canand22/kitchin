using System;
using System.Collections;

using Microsoft.Practices.ServiceLocation;

using NHibernate;

namespace KitchIn.Tests.MappingTests.Core
{
    public class RepositoryForTests
    {
        public RepositoryForTests()
        {
            this.session = ServiceLocator.Current.GetInstance<ISession>();
            if (this.session == null)
            {
                throw new ArgumentNullException("Session is not registered in IoC.");
            }
        }

        private readonly ISession session;

        public IList Get(Type type)
        {
            return this.session.CreateCriteria(type.Name).List();
        }

        public void SaveInstance(object obj)
        {
            using (var transaction = this.session.BeginTransaction())
            {
                this.session.SaveOrUpdate(obj);
                transaction.Commit();
            }
        }

        public void Delete(object entity)
        {
            using (var transaction = this.session.BeginTransaction())
            {
                this.session.Delete(entity);
                transaction.Commit();
            }
        }
    }
}
