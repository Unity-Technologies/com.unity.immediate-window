using System;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public interface ITypeView
    {
        string Name { get; }                            // Name to be used in the interface
        double Priority { get; }                        // Priority of this type view over others
        
        bool HasView(Type type);
        VisualElement GetView(object obj, ViewContext context);
    }
}