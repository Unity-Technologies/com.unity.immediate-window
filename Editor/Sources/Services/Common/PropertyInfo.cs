using System.Reflection;

namespace UnityEditor.ImmediateWindow.UI
{
    internal struct PropertyInfo
    {
        public FieldInfo Property;
        public bool IsPrivate;
        public object Object;
    }
}