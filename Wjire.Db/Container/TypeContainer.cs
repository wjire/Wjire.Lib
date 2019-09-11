using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wjire.Db.Container
{
    internal class TypeContainer
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> Container = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();



        internal static PropertyInfo GetProperty(Type type, string name)
        {
            Dictionary<string, PropertyInfo> dic = Container.GetOrAdd(type, t =>
            {
                PropertyInfo[] propertyInfos = type.GetProperties().Where(w => w.CanWrite == true).ToArray();
                return propertyInfos.ToDictionary(item => item.Name);
            });
            dic.TryGetValue(name, out PropertyInfo result);
            return result;
        }
    }
}
