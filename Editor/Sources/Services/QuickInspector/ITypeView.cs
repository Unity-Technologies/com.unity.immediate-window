using System;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public interface ITypeView
    {
        double Priority { get; }                        // Priority of this type view over others
        
        bool HasView(Type type);
        VisualElement GetView(object obj, ViewContext context);
    }
}