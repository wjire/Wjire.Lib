using System;
using System.Reflection;

namespace Wjire.Common
{

    /// <summary>
    /// 枚举扩展方法
    /// </summary>
    public static partial class ObjectExtensions
    {
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            Type type = enumValue.GetType();
            string enumName = Enum.GetName(type, enumValue);
            FieldInfo field = type.GetField(enumName);
            T att = field.GetCustomAttribute<T>(false);
            return att;
        }
    }
}
