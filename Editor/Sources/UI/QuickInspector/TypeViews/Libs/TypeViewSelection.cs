using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class TypeViewSelection : VisualElement
    {
        public event Action<ITypeView> OnTypeSwitch = delegate { };
        
        private ViewContext Context { get; set; }
        private ITypeView CurrentViewer { get; set; }
        private VisualElement ViewSwitch { get; set; }
        
        public TypeViewSelection(object obj, ViewContext context)
        {
            Context = context;
            CurrentViewer = context.Viewer;
            
            // Don't show type selector if there is only one type
            if (context.Viewers.Count() <= 1)
                return;
            
            ViewSwitch = new Label();
            ViewSwitch.AddClasses("icon node1 viewFull");
            
            ViewSwitch.RegisterCallback<MouseDownEvent>(OnTypeSwitchClick);
            
            Add(ViewSwitch);
        }

        private void OnTypeSwitchClick(MouseDownEvent evt)
        {
            if (Context.Viewer == CurrentViewer)
            {
                CurrentViewer = Context.Viewers.First();
                ViewSwitch.RemoveFromClassList("node1");
                ViewSwitch.AddToClassList("node0");
            }
            else
            {
                CurrentViewer = Context.Viewers.Last();
                ViewSwitch.RemoveFromClassList("node0");
                ViewSwitch.AddToClassList("node1");
            }

            OnTypeSwitch(CurrentViewer);
            
            evt.StopImmediatePropagation();
        }
    }
}