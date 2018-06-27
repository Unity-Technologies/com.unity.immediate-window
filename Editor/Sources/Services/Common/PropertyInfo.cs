using System.Reflection;

namespace UnityEditor.ImmediateWindow.UI
{
    internal struct PropertyInfo
    {
        public FieldInfo Field;
        public System.Reflection.PropertyInfo Property;
        public bool IsPrivate;
        public object Object;
    }
}