using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Wjire.Common
{

    /// <summary>
    /// 利用表达式树实现深拷贝,不支持复杂类型的属性
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
            /*
             * 注释部分以下面的代码作为例子
             * Person p = new Person() { Id = 1, Age = 18, Name = "SCMCC.Salary.Common" };
             * Expression<Func<Person, Student>> exp = source => new Student() { Id = source.Id, Name = source.Name };
             * Func<Person, Student> func = exp.Complie();
             * result = func(p);
             *
             */

            /* 构造表达式树的步骤
             * 1.0 构造 source
             * 2.0 构造 source.Id, source.Name
             * 3.0 构造 Id = source.Id, Name = source.Name
             * 4.0 构造 new Student()
             * 5.0 构造 new Student() { Id = source.Id, Name = source.Name }
             * 6.0 构造 lambda : source => new Student() { Id = source.Id, Name = source.Name }
             */

            Type sourceType = typeof(TSource);
            Type resultType = typeof(TResult);

            //构造入参 source
            ParameterExpression parameter = Expression.Parameter(sourceType, "source");

            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            foreach (PropertyInfo sourceProperty in sourceType.GetProperties())
            {

                //检查是否映射了属性名称
                MapNameAttribute customNameAttribute = sourceProperty.GetCustomAttribute<MapNameAttribute>();
                //MapNameAttribute customNameAttribute = resultProperty.GetCustomAttribute<MapNameAttribute>();
                PropertyInfo resultProperty = resultType.GetProperty(customNameAttribute == null ? sourceProperty.Name : customNameAttribute.Name);

                //过滤返回类型没有的属性
                if (resultProperty == null)
                {
                    continue;
                }

                //过滤返回类型的只读属性和泛型属性
                if (!resultProperty.CanWrite || resultProperty.PropertyType.IsGenericType)
                {
                    continue;
                }

                //过滤类型不一样的属性
                if (resultProperty.PropertyType != sourceProperty.PropertyType)
                {
                    continue;
                }

                //构造 source.Id,source.Name
                MemberExpression property = Expression.Property(parameter, sourceProperty);

                //构造 Id = source.Id, Name = source.Name
                MemberBinding memberBinding = Expression.Bind(resultProperty, property);
                memberBindingList.Add(memberBinding);
            }

            //构造 new Student()
            NewExpression newExpression = Expression.New(resultType);

            //构造 new Student() { Id = source.Id, Name = source.Name }
            MemberInitExpression memberInit = Expression.MemberInit(newExpression, memberBindingList);

            //构造 source => new Student() { Id = source.Id, Name = source.Name }
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
    /// 映射属性名称
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
