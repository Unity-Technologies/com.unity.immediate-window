using System;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ValueType : ATypeView
    {
        public override bool HasView(Type type)
        {
            if (type == null) return false;
            
            return type.IsPrimitive;
        }
        
        public override VisualElement GetView(object obj, ViewContext context)
        {
            var primitiveLabel = Text.Create(obj.ToString(), true, "isNumber");                
            return primitiveLabel;
        }
    }
}