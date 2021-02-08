using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Wjire.Common
{

    /// <summary>
    /// 利用表达式树实现属性复制
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public static class CopyProperties<T> where T : class, new()
    {
        /// <summary>
        /// 属性复制
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T From(T source)
        {
            return ExpressionCopy<T, T>.From(source);
        }
    }

    /// <summary>
    /// 利用表达式树实现属性复制
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">返回类型</typeparam>
    public static class CopyProperties<TSource, TTarget> where TSource : class, new() where TTarget : class, new()
    {
        private static readonly Action<TSource, TTarget> Cache = GetAction();

        /// <summary>
        /// 利用表达式树深拷贝,并返回对象
        /// </summary>
        /// <returns></returns>
        private static Action<TSource, TTarget> GetAction()
        {
            /*
             * 注释部分以下面的代码作为例子
             *
             * Expression<Action<Person, Student>> exp = (source,target) => { target.Id = source.Id, target.Name = source.Name };
             * Action<Person, Student> action = exp.Compile();
             *
             * var s = new Person();
             * var t = new Student();
             * action(s,t);
             *
             */
            
            Type sourceType = typeof(TSource);
            Type targetType = typeof(TTarget);

            //构造入参 source
            ParameterExpression sourceParameter = Expression.Parameter(sourceType, "source");

            //构造入参 target
            ParameterExpression targetParameter = Expression.Parameter(targetType, "target");

            var binaryExpressions = new List<BinaryExpression>();

            foreach (PropertyInfo sourceProperty in sourceType.GetProperties())
            {

                //检查是否映射了属性名称
                MapNameAttribute customNameAttribute = sourceProperty.GetCustomAttribute<MapNameAttribute>();
                PropertyInfo targetProperty = targetType.GetProperty(customNameAttribute == null ? sourceProperty.Name : customNameAttribute.Name);

                //过滤返回类型没有的属性
                if (targetProperty == null)
                {
                    continue;
                }

                //过滤返回类型的只读属性
                if (targetProperty.CanWrite == false)
                {
                    continue;
                }

                //过滤类型不一样的属性
                if (targetProperty.PropertyType != sourceProperty.PropertyType)
                {
                    continue;
                }

                //构造 source.Id
                MemberExpression sourceMember = Expression.Property(sourceParameter, sourceProperty);

                //构造 target.Id
                MemberExpression targetMember = Expression.Property(targetParameter, targetProperty);

                //构造 target.Id = source.Id
                var memberBinding = Expression.Assign(targetMember, sourceMember);

                binaryExpressions.Add(memberBinding);
            }

            //构造 { target.Id = source.Id, target.Name = source.Name }
            Expression body = Expression.Block(binaryExpressions);

            ////构造 (source,target) => { target.Id = source.Id, target.Name = source.Name };
            Expression<Action<TSource, TTarget>> expression = Expression.Lambda<Action<TSource, TTarget>>(body, sourceParameter, targetParameter);

            Action<TSource, TTarget> action = expression.Compile();
            return action;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static void From(TSource source, TTarget target)
        {
            Cache(source, target);
        }
    }
}
