using System;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class NullType : ATypeView
    {
        public override double Priority {get { return 0; }}

        public override bool HasView(Type type)
        {
            return type == null;
        }

        public override VisualElement GetView(object obj, ViewContext context)
        {
            return Text.Create("null", true, "isNull");
        }
    }
}