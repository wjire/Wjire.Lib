﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wjire.Db.Container
{


    internal class TypeContainer
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> EntityContainer = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> ParameterContainer =
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        internal static readonly ConcurrentDictionary<Type, string> AddSqlContainer = new ConcurrentDictionary<Type, string>();


        /// <summary>
        /// 获取实体属性信息
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static PropertyInfo GetProperty(Type entityType, string name)
        {
            Dictionary<string, PropertyInfo> dic = EntityContainer.GetOrAdd(entityType, t =>
            {
                PropertyInfo[] propertyInfos = t.GetProperties().Where(w => w.CanWrite == true).ToArray();
                return propertyInfos.ToDictionary(item => item.Name.ToLower());
            });
            dic.TryGetValue(name.ToLower(), out PropertyInfo result);
            return result;
        }



        /// <summary>
        /// 获取参数属性信息
        /// </summary>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        internal static PropertyInfo[] GetPropertyInfos(Type parameterType)
        {
            return ParameterContainer.GetOrAdd(parameterType, t => t.GetProperties());
        }
    }
}