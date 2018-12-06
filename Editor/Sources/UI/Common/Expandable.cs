using System;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class Expandable : VisualElement
    {
        public event Action<bool> OnExpandStateChanged = delegate { };

        public bool Expanded
        {
            get { return Arrow.Expanded; }
            set
            {
                this.EnableClassToggle("expanded", "collapsed", value);

                Arrow.Expanded = value;

                if (value)
                    Add(ExpandedGroup);
                else
                    if (Contains(ExpandedGroup))
                        Remove(ExpandedGroup);

                OnExpandStateChanged(value);
            }
        }

        protected ArrowToggle Arrow { get; set; }
        protected VisualElement Content { get; set; }
        public VisualElement CollapsedGroup { get; protected  set; }
        public VisualElement ExpandedGroup { get; protected set; }
        
        public Expandable(bool expanded = false)
        {
            AddToClassList("expandable");

            Arrow = new ArrowToggle();

            Content = new Container("collapseRow");
            Add(Content);
                
            CollapsedGroup = new Container("collapsedGroup");
            CollapsedGroup.RegisterCallback<MouseDownEvent>(OnClick);                
            CollapsedGroup.Add(Arrow);
            Content.Add(CollapsedGroup);

            ExpandedGroup = new Container("expandedGroup");            
            Add(ExpandedGroup);
            
            Expanded = expanded;
        }

        protected void OnClick(MouseDownEvent evt)
        {
            Toggle();
        }

        public void Toggle()
        {
            Expanded = !Expanded;
        }
    }
}
