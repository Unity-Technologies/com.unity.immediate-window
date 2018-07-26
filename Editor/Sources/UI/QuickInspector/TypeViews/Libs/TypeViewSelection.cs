using System;
using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class TypeViewSelection : VisualElement
    {
        private ViewContext Context { get; set; }
        
        public TypeViewSelection(object obj, ViewContext context)
        {
            Context = context;
            
            // Don't show type selector if there is only one type
            if (context.Viewers.Count() > 1)
            {
                if (Context.Viewers.Count() > 2)
                    Add(new TypeViewMenu(obj, context));
            
                Add(new TypeViewQuickSwitch(obj, context));                
            }
        }
    }
}