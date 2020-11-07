using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Wjire.Test
{

    public static class ExpressionExtensions
    {
        public static T ExpressionDeepCopyTo<T>(this T source) where T : class, new()
        {
            return ExpressionCopy<T>.DeepCopyFrom(source);
        }

        public static TResult ExpressionDeepCopyTo<TSource, TResult>(this TSource source) where TSource : class, new() where TResult : class, new()
        {
            return ExpressionCopy<TSource, TResult>.DeepCopyFrom(source);
        }

        public static void ExpressionCopyPropertyTo<TSource, TTarget>(this TSource source, TTarget target) where TSource : class, new() where TTarget : class, new()
        {
            ExpressionCopy<TSource, TTarget>.PropertyCopy(source, target);
        }
    }


    /// <summary>
    /// 利用表达式树实现深拷贝,暂时只支持相同类型的深拷贝，因为公司内部的 MapTo<>() 在类型一样的情况下是 浅拷贝。
    /// 目前不支持不同类型，但可互相转换的属性的拷贝.
    /// 如果属性是引用类型,则该属性仍然是浅拷贝
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ExpressionCopy<T> where T : class, new()
    {
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepCopyFrom(T source)
        {
            return ExpressionCopy<T, T>.DeepCopyFrom(source);
        }
    }


    /// <summary>
    /// 利用表达式树实现深拷贝,不支持不同类型但可互相转换的属性
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TResult">返回类型</typeparam>
    public static class ExpressionCopy<TSource, TResult> where TSource : class, new() where TResult : class, new()
    {
        #region 深拷贝

        private static readonly Func<TSource, TResult> DeepCopyCache = DeepCopyDelegate();

        private static Func<TSource, TResult> DeepCopyDelegate()
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
                //TODO:这里将来再扩展
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
        /// 深拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TResult DeepCopyFrom(TSource source)
        {
            return source == null ? null : DeepCopyCache(source);
        }

        #endregion

        #region 属性复制

        private static readonly Action<TSource, TResult> PropertyCopyCache = PropertyCopyDelegate();

        private static Action<TSource, TResult> PropertyCopyDelegate()
        {
            Type sourceType = typeof(TSource);
            Type resultType = typeof(TResult);

            //构造入参 source
            ParameterExpression sourceParameter = Expression.Parameter(sourceType, "source");

            //构造入参 target
            ParameterExpression resultParameter = Expression.Parameter(resultType, "target");

            var binaryExpressions = new List<BinaryExpression>();

            foreach (PropertyInfo sourceProperty in sourceType.GetProperties())
            {

                //检查是否映射了属性名称
                MapNameAttribute customNameAttribute = sourceProperty.GetCustomAttribute<MapNameAttribute>();
                PropertyInfo targetProperty = resultType.GetProperty(customNameAttribute == null ? sourceProperty.Name : customNameAttribute.Name);

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
                //TODO:这里将来再扩展
                if (targetProperty.PropertyType != sourceProperty.PropertyType)
                {
                    continue;
                }

                MemberExpression sourceMember = Expression.Property(sourceParameter, sourceProperty);
                MemberExpression targetMember = Expression.Property(resultParameter, targetProperty);
                var memberBinding = Expression.Assign(targetMember, sourceMember);
                binaryExpressions.Add(memberBinding);
            }
            Expression body = Expression.Block(binaryExpressions);
            Expression<Action<TSource, TResult>> expression = Expression.Lambda<Action<TSource, TResult>>(body, sourceParameter, resultParameter);
            Action<TSource, TResult> action = expression.Compile();
            return action;
        }

        /// <summary>
        /// 属性复制
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static void PropertyCopy(TSource source, TResult target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            PropertyCopyCache(source, target);
        }

        #endregion
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
