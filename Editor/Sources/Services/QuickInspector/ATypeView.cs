using System;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    /// <summary>
    /// Common ITypeView Implementation
    /// </summary>
    public abstract class ATypeView : ITypeView
    {
        public string Name => GetType().Name;
        public virtual double Priority {get { return 0; }}
        
        public virtual bool HasView(Type type) {return false;}
        public virtual VisualElement GetView(object obj, ViewContext context) {return new VisualElement();}        
    }
}