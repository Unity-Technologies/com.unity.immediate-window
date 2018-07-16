using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class PropertyUtils
    {
        public static bool ShowPrivate = true;

        public static IEnumerable<PropertyInfo> GetPropertyInfo(object obj)
        {
            var properties = new List<PropertyInfo>();
            var type = obj.GetType();

            // Possibly: PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
            var privateBindingFlags =
                BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.FlattenHierarchy; // Allow private properties
            var publicProperties = type.GetFields().ToList();
            var privateProperties = type.GetFields(privateBindingFlags).ToList();

            foreach (var field in publicProperties)
                properties.Add(new PropertyInfo {Field = field, Property = null, IsPrivate = false, Object = obj});

            foreach (var prop in type.GetProperties())
            {
                if (prop.GetGetMethod() == null)
                    continue;

                // TODO: Technically should not skip them but simply mark them as such in the inspector
                if (prop.GetCustomAttributes().OfType<System.ObsoleteAttribute>().Any())
                    continue;

                // TODO: Technically should at least add those. Just can't show the value (since they take arguments)
                if (prop.GetGetMethod().GetParameters().Any())
                    continue;

                properties.Add(new PropertyInfo {Field = null, Property = prop, IsPrivate = false, Object = obj});
            }

            if (ShowPrivate)
            {
                foreach (var prop in privateProperties)
                    properties.Add(new PropertyInfo {Field = prop, IsPrivate = true, Object = obj});
            }


            return properties;
        }
    }
}