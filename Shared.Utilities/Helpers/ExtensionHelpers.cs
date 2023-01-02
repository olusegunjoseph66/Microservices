using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace Shared.Utilities.Helpers
{
    public static class ExtensionHelpers
    {
        public static string ToDescription(this Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static T? Deserialize<T>(this string jsonString) where T : new()
        {
            return !string.IsNullOrWhiteSpace(jsonString) ? JsonConvert.DeserializeObject<T>(jsonString) : new T();
        }

        public static string Serialize(this object @object)
        {
            return @object != null ? JsonConvert.SerializeObject(@object) : string.Empty;
        }
    }
}
