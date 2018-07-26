using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    /// <summary>
    /// Visual Structure
    /// 
    ///                            Content
    /// |-----------------------------------------------------|
    ///           CollapsedGroup
    /// |---------------------------------|
    ///                  CollapseViewContainer
    ///                      |----------------|
    ///      Label          CollapseView          Tools
    ///    |-------------|----------------|--------------------|
    ///  ► MyClass          {desc}                      Tools
    /// 
    /// </summary>
    internal class ExtendedExpandable : Expandable
    {
        // Permanent label (optional)
        private Span _Label;
        public Span Label
        {
            get { return _Label;}
            set
            {
                if (Label != null)
                    CollapsedGroup.Remove(Label);
                
                _Label = value;

                if (Label != null)
                {
                    Label.AddToClassList("label");
                    CollapsedGroup.Insert(1, Label);   // Index 1 = after arrow                     
                }
            }
        } 

        public VisualElement CollapseViewContainer { get; protected set; }    // Container is there so content can be added/removed easily

        private VisualElement _CollapsedView;
        public VisualElement CollapsedView
        {
            get { return _CollapsedView;}
            set
            {
                _CollapsedView = value;

                SetCollapseView();
            }
        }

        private VisualElement _ExpandedView;
        public VisualElement ExpandedView
        {
            get { return _ExpandedView;}
            set
            {
                _ExpandedView = value;
                
                ExpandedGroup.Clear();
                ExpandedGroup.Add(ExpandedView);
            }
        }

        private VisualElement _Tools;
        public VisualElement Tools
        {
            get
            {
                // Only Create a Tools element if it is needed
                if (_Tools == null)
                {
                    _Tools = new Container("tools");
                    Content.Add(Tools);
                }

                return _Tools;
            }
        }

        public ExtendedExpandable()
        {
            Label = new Span();
            
            CollapseViewContainer = new Container("collapseView");
            CollapsedGroup.Add(CollapseViewContainer);
            CollapsedView = new VisualElement();

            OnExpandStateChanged += OnExpandState;
        }

        public ExtendedExpandable(string label = "", VisualElement expandedView = null, VisualElement collapsedView = null, bool expanded = false, string classnames = "",
            string labelClassNames = "") : this()
        {
            this.AddClasses(classnames);
            Label.AddClasses(labelClassNames);
            
            Label.text = label;
            if (expandedView != null)
                ExpandedView = expandedView;
            
            if (collapsedView!= null)
                CollapsedView = collapsedView;

            Expanded = expanded;
        }

        private void SetCollapseView()
        {
            CollapseViewContainer.Clear();
            CollapseViewContainer.Add(CollapsedView);
        }

        
        private void OnExpandState(bool expanded)
        {
            if (expanded)
                CollapseViewContainer.Remove(CollapsedView);
            else
                CollapseViewContainer.Add(CollapsedView);
        }
    }
}