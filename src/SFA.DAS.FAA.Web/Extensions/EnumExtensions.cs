using System.ComponentModel;
using static System.Convert;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return enumValue.ToString();

            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return enumValue.ToString();
        }

        public static string StringValue(this Enum argEnum)
        {
            return Convert.ToString(ToInt32(argEnum));
        }
    }
}
