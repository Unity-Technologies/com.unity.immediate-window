using UnityEditor.ImmediateWindow.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class ClassView : VisualElement
    {
        private ExtendedExpandable Content { get; set; }
        
        private object CurrentObject { get; set; }
        private ViewContext Context { get; set; }
        
        private Span PinnedItem { get; set; }
        private string PinnedSymbol { get; set; }
        private bool Pinned { get; set; }

        
        public ClassView(object obj, ViewContext context)
        {
            var viewFull = new Label();
            viewFull.AddClasses("icon node1 viewFull");
            var views = new Container("viewModes");
                
            Content = new ExtendedExpandable();
            Content.Tools.Add(views);

            Add(Content);
    
            PinnedItem = new Span();
            PinnedItem.name = "pinned-symbol";
            PinnedItem.RegisterCallback<TooltipEvent>(e => {
                e.rect = PinnedItem.worldBound;
                e.tooltip = string.Format("Use {0} to refer to this element", PinnedSymbol);
                e.StopImmediatePropagation();
            });

            SetObject(obj, context);

            Content.OnExpandStateChanged += OnExpandState;
        }
        
        public void SetObject(object obj, ViewContext context)
        {
            CurrentObject = obj;
            Context = context;

            Content.Label = new TypeNameView(CurrentObject);
            Content.Label.RegisterCallback<MouseDownEvent>(OnTypeClick);

            OnExpandState(Content.Expanded);
        }

        private void OnExpandState(bool expanded)
        {
            // Prevent infinite recursion by only creating expanded element on demande
            // (since properties can be self-referential)
            if (expanded)
            {
                Content.CollapsedView = new VisualElement();
                Content.ExpandedView = new ExpandedClassType(CurrentObject);
            }
            else
            {
                Content.ExpandedView = new VisualElement();
                Content.CollapsedView = new CollapsedPropertyGroup(CurrentObject);                
            }
        }

        async void OnTypeClick(MouseDownEvent evt)
        {
            Debug.Log("Type clicked!!");
            PinnedSymbol = await Evaluator.Instance.AddToNextGlobal(CurrentObject, () =>
            {
                PinnedSymbol = "";
                PinnedItem.text = "";
            });

            PinnedItem.text = string.Format("== {0}", PinnedSymbol);
            Pinned = true;
        }
    }
}