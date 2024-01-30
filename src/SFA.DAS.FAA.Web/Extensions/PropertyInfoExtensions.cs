using System.Reflection;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static T? GetAttribute<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            var attributes = propertyInfo.GetCustomAttributes(true);
            return attributes
                .Cast<Attribute>()
                .FirstOrDefault(a => a.GetType().IsEquivalentTo(typeof(T)))
                as T;
        }
    }
}
