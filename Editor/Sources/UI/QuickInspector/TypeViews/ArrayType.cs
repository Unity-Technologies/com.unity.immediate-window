using System;
using System.Collections;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ArrayType : ATypeView
    {
        public override bool HasView(Type type)
        {
            if (type == null) return false;

            return type.IsArray;
        }

        public override VisualElement GetView(object obj, ViewContext context)
        {
            if (context.Mode == ViewMode.Default)
                return new ExpandableObject(obj, context);

            if (context.Mode == ViewMode.Collapsed)
                return new CollapsedArrayGroup(obj as IEnumerable);
            if (context.Mode == ViewMode.Expanded)
                return new ExpandedArrayGroup(obj);
            if (context.Mode == ViewMode.Value)
                return new TypeNameView(obj);
            
            throw new Exception("Should not get here");
        }
    }
}
