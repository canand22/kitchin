using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KitchIn.Tests.MappingTests.Core
{
    public class InstanceCreator<T> where T : class
    {
        public InstanceCreator(Assembly assembly)
        {
            BaseType = typeof(T);
            EntitiesTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract);
            CreateInstancesTestValueCollection();
        }

        public static IEnumerable<Type> EntitiesTypes { get; set; }

        public static Type BaseType { get; private set; }

        private static Dictionary<Type, object> InstanceCollection { get; set; }

        private static void CreateInstancesTestValueCollection()
        {
            InstanceCollection = new Dictionary<Type, object>();

            InstanceCollection[typeof(Guid)] = Guid.NewGuid();
            InstanceCollection[typeof(int)] = 50;
            InstanceCollection[typeof(float)] = 20.1;
            InstanceCollection[typeof(double)] = 200.3;
            InstanceCollection[typeof(string)] = "teststring";
            InstanceCollection[typeof(DateTime)] = DateTime.Now;
            InstanceCollection[typeof(bool)] = true;
            InstanceCollection[typeof(byte[])] = new byte[2];
        }

        private object GetPropertyValue(Type propertyType)
        {
            if (EntitiesTypes.Contains(propertyType))
            {
                return this.CreateSimpleInstance(propertyType);
            }

            if (InstanceCollection.ContainsKey(propertyType))
            {
                return InstanceCollection[propertyType];
            }

            return null;
        }

        public object CreateSimpleInstance(Type type, object reference = null)
        {
            var instance = Activator.CreateInstance(type);
            var properties = type.GetProperties().Where(p => p.CanWrite && p.CanRead && p.PropertyType.IsSubclassOf(typeof(T)) || InstanceCollection.Keys.Contains(p.PropertyType));
            foreach (var p in properties)
            {
                object value = null;
                if (p.PropertyType.IsSubclassOf(typeof(T)))
                {
                    var targetPropertyType = p.PropertyType;
                    if (p.PropertyType.IsAbstract)
                    {
                        targetPropertyType = EntitiesTypes.First(t => t.IsSubclassOf(targetPropertyType));
                    }

                    if (reference != null && reference.GetType() == targetPropertyType)
                    {
                        value = reference;
                    }
                    else
                    {
                        value = this.CreateSimpleInstance(targetPropertyType, instance);
                    }
                }

                if (InstanceCollection.ContainsKey(p.PropertyType))
                {
                    value = InstanceCollection[p.PropertyType];
                }

                if (p.Name != "Id")
                {
                    p.SetValue(instance, value, null);
                }
            }

            return instance;
        }

        public void SetReferences(object entity)
        {
            var properties = entity.GetType().GetProperties().Where(p => p.CanWrite && p.CanRead);
            foreach (var p in properties)
            {
                var value = this.GetPropertyValue(p.PropertyType);
                p.SetValue(entity, value, null);
            }
        }

        public IEnumerable<object> GetInstances()
        {
            return EntitiesTypes.Select(x => this.CreateSimpleInstance(x));
        }
    }
}
