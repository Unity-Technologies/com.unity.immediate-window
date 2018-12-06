using System;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ObjectType : ITypeView
    {
        public string Name { get; } = "Object";
        public double Priority {get { return -1; }}    // Lower priority then standard

        public bool HasView(Type type)
        {
            if (type == null) return false;

            return true;
        }

        public VisualElement GetView(object obj, ViewContext context)
        {
            if (context.Mode == ViewMode.Default)
                return new ExpandableObject(obj, context);

            if (context.Mode == ViewMode.Collapsed)
                return new CollapsedPropertyGroup(obj);
            if (context.Mode == ViewMode.Expanded)
                return new ExpandedClassType(obj);
            if (context.Mode == ViewMode.Value)
                return new TypeNameView(obj);
            
            throw new Exception("Should not get here");
        }
    }
}
