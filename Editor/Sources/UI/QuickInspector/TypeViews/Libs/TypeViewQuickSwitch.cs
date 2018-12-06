using System.Linq;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class TypeViewQuickSwitch : Label
    {
        private ViewContext Context { get; set; }

        public TypeViewQuickSwitch(object obj, ViewContext context)
        {
            Context = context;
            
            this.AddClasses("icon viewFull");
            RegisterCallback<MouseDownEvent>(OnTypeSwitchClick);
            tooltip = "Click to switch between type views";
            
            Context.OnTypeSwitch += OnTypeSwitch;
            OnTypeSwitch(Context.Viewer);
        }

        private void OnTypeSwitch(ITypeView viewer)
        {
            // First viewer is the default object one
            if (Context.Viewer == Context.Viewers.First())
            {
                RemoveFromClassList("node1");
                AddToClassList("node0");
            }
            else
            {
                RemoveFromClassList("node0");
                AddToClassList("node1");
            }
        }

        private void OnTypeSwitchClick(MouseDownEvent evt)
        {
            if (Context.Viewer == Context.Viewers.First())
                Context.SwitchType(Context.Viewers.Last());
            else
                Context.SwitchType(Context.Viewers.First());
        }
    }
}