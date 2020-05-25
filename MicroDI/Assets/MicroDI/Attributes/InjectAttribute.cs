using System;
using System.Reflection;

namespace MicroDI.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
        public string Tag { get; set; }
        public MemberInfo MemberInfo { get; set; }

        public InjectAttribute(string tag = null)
        {
            Tag = tag;
        }
    }
}
