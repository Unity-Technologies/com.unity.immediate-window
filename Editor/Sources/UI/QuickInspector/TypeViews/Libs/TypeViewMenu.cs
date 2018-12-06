using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class TypeViewMenu : VisualElement
    {
        private ViewContext Context { get; set; }
        private Button Label { get; set; }

        public TypeViewMenu(object obj, ViewContext context)
        {
            Context = context;

            Label = new Button();
            Label.AddClasses("compact");
            SetButtonLabel();
            Label.RegisterCallback<MouseUpEvent>(TypeViewersClick);
            Label.tooltip = "Click to switch between type views";
            Add(Label);
            
            Context.OnTypeSwitch += OnTypeSwitch;
        }

        private void OnTypeSwitch(ITypeView viewer)
        {
            SetButtonLabel();
        }

        private void SetButtonLabel()
        {
            Label.text = Context.Viewer.Name;
        }

        private void TypeViewersClick(MouseUpEvent evt)
        {
            var menu = new GenericMenu();
            foreach (var viewer in Context.Viewers)
            {
                menu.AddItem(new GUIContent(viewer.Name), false, obj => Context.SwitchType(viewer), null);                
            }
            
            var menuPosition = new Vector2(Label.layout.xMin, Label.layout.center.y);
            menuPosition = this.LocalToWorld(menuPosition);
            var menuRect = new Rect(menuPosition, Vector2.zero);
            menu.DropDown(menuRect);
        }
    }
}