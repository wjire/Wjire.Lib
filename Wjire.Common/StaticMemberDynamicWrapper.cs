using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Wjire.Common
{
    /// <summary>
    /// dynamic 限制只能访问对象的实例成员,因为dynamic变量必须引用对象.
    /// 该类的作用就是在运行时访问类型的静态成员
    /// </summary>
    public sealed class StaticMemberDynamicWrapper : DynamicObject
    {
        private readonly TypeInfo _type;

        public StaticMemberDynamicWrapper(Type type)
        {
            _type = type.GetTypeInfo();
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _type.DeclaredMembers.Select(s => s.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            var field = FindField(binder.Name);
            if (field != null)
            {
                result = field.GetValue(null);
                return true;
            }

            var prop = FindProperty(binder.Name, true);
            if (prop != null)
            {
                result = prop.GetValue(null, null);
                return true;
            }
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var field = FindField(binder.Name);
            if (field != null)
            {
                field.SetValue(null, value);
                return true;
            }

            var prop = FindProperty(binder.Name, false);
            if (prop != null)
            {
                prop.SetValue(null, value, null);
                return true;
            }
            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var method = FindMethod(binder.Name, args.Select(s => s.GetType()).ToArray());
            if (method == null)
            {
                result = null;
                return false;
            }

            result = method.Invoke(null, args);
            return true;
        }

        private FieldInfo FindField(string name)
        {
            return _type.DeclaredFields.FirstOrDefault(f => f.IsPublic && f.IsStatic && f.Name == name);
        }

        private PropertyInfo FindProperty(string name, bool get)
        {
            if (get)
            {
                return _type.DeclaredProperties.FirstOrDefault(f =>
                    f.Name == name &&
                    f.GetMethod != null &&
                    f.GetMethod.IsPublic &&
                    f.GetMethod.IsStatic);
            }

            return _type.DeclaredProperties.FirstOrDefault(f =>
                f.Name == name &&
                f.GetMethod != null &&
                f.GetMethod.IsPublic &&
                f.GetMethod.IsStatic);
        }

        private MethodInfo FindMethod(string name, Type[] paramTypes)
        {
            return _type.DeclaredMethods.FirstOrDefault(f =>
                f.IsPublic &&
                f.IsStatic &&
                f.Name == name &&
                ParametersMatch(f.GetParameters(), paramTypes));
        }

        private bool ParametersMatch(ParameterInfo[] parameters, Type[] paramTypes)
        {
            if (parameters.Length != paramTypes.Length)
            {
                return false;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != paramTypes[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
