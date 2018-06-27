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

            // Possibly: PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
            var privateBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;    // Allow private properties
            var publicProperties = obj.GetType().GetFields().ToList();
            var privateProperties = obj.GetType().GetFields(privateBindingFlags).ToList();
            
            foreach (var prop in publicProperties)
                properties.Add(new PropertyInfo {Property = prop, IsPrivate = false, Object = obj});

            if (ShowPrivate)
            {
                foreach (var prop in privateProperties)
                    properties.Add(new PropertyInfo {Property = prop, IsPrivate = true, Object = obj});
            }

            return properties;
        }
    }
}