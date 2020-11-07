using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Wjire.Test
{

    /// <summary>
    /// 利用表达式树实现深拷贝,暂时只支持相同类型的深拷贝，因为公司内部的 MapTo<> 在类型一样的情况下是 浅拷贝。
    /// 目前不支持泛型，不支持可空，不支持不同类型，但可互相转换的属性的拷贝.
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    public static class ExpressionCopy<T> where T : class, new()
    {
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T From(T source)
        {
            return ExpressionCopy<T, T>.From(source);
        }
    }


    /// <summary>
    /// 利用表达式树实现深拷贝,目前不支持集合，不支持泛型，不支持可空，不支持不同类型但可互相转换的属性
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TResult">返回类型</typeparam>
    public static class ExpressionCopy<TSource, TResult> where TSource : class, new() where TResult : class, new()
    {
        private static readonly Func<TSource, TResult> Cache = GetFunc();

        /// <summary>
        /// 利用表达式树深拷贝,并返回对象
        /// </summary>
        /// <returns></returns>
        private static Func<TSource, TResult> GetFunc()
        {
            Type sourceType = typeof(TSource);
            Type resultType = typeof(TResult);

            //构造入参 source
            ParameterExpression parameter = Expression.Parameter(sourceType, "source");

            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (PropertyInfo sourceProperty in sourceType.GetProperties())
            {
                //检查是否映射了属性名称
                MapNameAttribute customNameAttribute = sourceProperty.GetCustomAttribute<MapNameAttribute>();
                PropertyInfo resultProperty = resultType.GetProperty(customNameAttribute == null ? sourceProperty.Name : customNameAttribute.Name);

                //过滤返回类型没有的属性
                if (resultProperty == null)
                {
                    continue;
                }

                //过滤返回类型的只读属性
                if (resultProperty.CanWrite == false)
                {
                    continue;
                }

                //过滤类型不一样的属性
                if (resultProperty.PropertyType != sourceProperty.PropertyType)
                {
                    continue;
                }

                MemberExpression property = Expression.Property(parameter, sourceProperty);
                MemberBinding memberBinding = Expression.Bind(resultProperty, property);
                memberBindingList.Add(memberBinding);
            }
            NewExpression newExpression = Expression.New(resultType);
            MemberInitExpression memberInit = Expression.MemberInit(newExpression, memberBindingList);
            Expression<Func<TSource, TResult>> expression = Expression.Lambda<Func<TSource, TResult>>(memberInit, parameter);
            Func<TSource, TResult> function = expression.Compile();
            return function;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TResult From(TSource source)
        {
            return Cache(source);
        }
    }

    /// <summary>
    /// 应用在 source 对象属性上的映射名称
    /// </summary>
    public class MapNameAttribute : Attribute
    {
        public string Name { get; set; }

        public MapNameAttribute(string name)
        {
            Name = name;
        }
    }
}
