using System;
using System.Reflection;
using KitchIn.BL.Attributes;

namespace KitchIn.BL.Extensions
{
    public static class EnumExtensions
    {
        public static String GetMessage(this Enum item)
        {
            MemberInfo[] info = item.GetType().GetMember(item.ToString());
            if (info == null || info.Length == 0)
            {
                throw new Exception("Enum name can't be accessed as there is no member info");
            }

            object[] attributes = info[0].GetCustomAttributes(typeof(ErrorsAttribute), false);
            if (attributes == null || attributes.Length == 0)
            {
                throw new Exception("Enum name is not defined");
            }
            return ((ErrorsAttribute)attributes[0]).Error;
        }
    }
}
