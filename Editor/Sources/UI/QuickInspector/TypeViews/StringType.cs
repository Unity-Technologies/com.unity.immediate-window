using System;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class StringType : ATypeView
    {
        public override bool HasView(Type type)
        {
            if (type == null) return false;

            return type == typeof(string);
        }
        
        public override VisualElement GetView(object obj, ViewContext context)
        {
            var label = string.Format("\"{0}\"", obj.ToString());
            var primitiveLabel = Text.Create(label, true, "isString");
                
            return primitiveLabel;
        }
    }
}