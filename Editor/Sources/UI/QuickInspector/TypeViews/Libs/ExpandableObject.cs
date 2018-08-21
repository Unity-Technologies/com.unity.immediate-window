using System.Collections;
using System.ComponentModel;
using UnityEditor.ImmediateWindow.Services;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class ExpandableObject : VisualElement
    {
        private ExtendedExpandable Content { get; set; }
        
        private object CurrentObject { get; set; }
        private ViewContext Context { get; set; }
        private CachedView ExpandedView { get; set; }
        private CachedView CollapsedView { get; set; }
        
        public ExpandableObject(object obj, ViewContext context)
        {
            CurrentObject = obj;
            Context = context;
            ExpandedView = new CachedView(() => new TypeInspector(obj, new ViewContext {Mode = ViewMode.Expanded}));
            CollapsedView =  new CachedView(() => new TypeInspector(obj, new ViewContext {Mode = ViewMode.Collapsed}));

            var views = new Container("viewModes");
            var typeViewSelection = new TypeViewSelection(obj, context);
            context.OnTypeSwitch += OnTypeSwitch;
            views.Add(typeViewSelection);

            Content = new ExtendedExpandable();
            Content.Tools.Add(views);
            Content.Label = new TypeNameView(CurrentObject);
            Content.OnExpandStateChanged += expanded => Refresh();
            Add(Content);
    
            Refresh();
        }

        private void OnTypeSwitch(ITypeView viewer)
        {
            ExpandedView = new CachedView(() => new TypeInspector(CurrentObject, new ViewContext {Mode = ViewMode.Expanded, Viewer = viewer}));
            CollapsedView =  new CachedView(() => new TypeInspector(CurrentObject, new ViewContext {Mode = ViewMode.Collapsed, Viewer = viewer}));            

            Refresh();        // Rebuild with new forced type
        }

        private void Refresh()
        {
            // Prevent infinite recursion by only creating expanded element on demande
            // (since properties can be self-referential)
            if (Content.Expanded)
            {
                Content.CollapsedView = new VisualElement();
                Content.ExpandedView = ExpandedView.View;
            }
            else
            {
                Content.ExpandedView = new VisualElement();
                Content.CollapsedView = CollapsedView.View;                
            }
        }
    }
}
