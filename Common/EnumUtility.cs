using System;

namespace Common
{

    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string des)
        {
            Description = des;
        }

        public string Description { get; set; }
    }




    public static class EnumUtil
    {
        public static string GetDescription(this Enum value, bool nameInstead = true)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
                return null;

            var field = type.GetField(name);
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead)
                return name;
            return attribute?.Description;
        }
    }
}
