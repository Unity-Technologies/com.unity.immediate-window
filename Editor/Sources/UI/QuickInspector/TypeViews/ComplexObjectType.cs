using System;
using System.Numerics;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    // For debugging multi-views only
    internal class ComplexObjectType1 : ITypeView
    {
        public string Name { get; } = "Complex 1";
        public virtual double Priority {get { return -2; }}

        public bool HasView(Type type)
        {
            if (type == null) return false;
            
            return type == typeof(ComplexObject);
        }
        
        public VisualElement GetView(object obj, ViewContext context)
        {
            return new NullType().GetView(obj, context);
        }
    }
    
    // For debugging multi-views only
    internal class ComplexObjectType2 : ITypeView
    {
        public string Name { get; } = "Complex 2";
        public virtual double Priority {get { return -3; }}

        public bool HasView(Type type)
        {
            if (type == null) return false;
            
            return type == typeof(ComplexObject);
        }
        
        public VisualElement GetView(object obj, ViewContext context)
        {
            return new StringType().GetView(obj, context);
        }
    }
    
}