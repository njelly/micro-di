using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MicroDI.Attributes;

namespace MicroDI.Core
{
    public static class ReflectionUtility
    {
        private static Dictionary<Type, ConstructorInfo> _constructors = new Dictionary<Type, ConstructorInfo>();
        private static Dictionary<Type, MemberInfo[]> _members = new Dictionary<Type, MemberInfo[]>();
        private static Dictionary<MemberInfo, InjectAttribute> _injectAttributes = new Dictionary<MemberInfo, InjectAttribute>();

        public static ConstructorInfo GetConstructor(Type type)
        {
            if(_constructors.ContainsKey(type))
            {
                return _constructors[type];
            }

            var constructors = type.GetConstructors();
            _constructors[type] = constructors.FirstOrDefault();

            return _constructors[type];
        }

        public static MemberInfo[] GetMembers(Type type)
        {
            if(_members.ContainsKey(type))
            {
                return _members[type];
            }

            var members = from member in type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                          where member.GetCustomAttributes(typeof(InjectAttribute), true).Length > 0
                          select member;

            _members[type] = members.ToArray();

            return _members[type];
        }

        public static InjectAttribute GetInjectAttribute(MemberInfo memberInfo)
        {
            if(_injectAttributes.ContainsKey(memberInfo))
            {
                return _injectAttributes[memberInfo];
            }

            var attrs = from attribute in memberInfo.GetCustomAttributes(typeof(InjectAttribute), true)
                       select attribute;

            _injectAttributes[memberInfo] = (InjectAttribute)attrs.First();
            return _injectAttributes[memberInfo];
        }
    }
}
