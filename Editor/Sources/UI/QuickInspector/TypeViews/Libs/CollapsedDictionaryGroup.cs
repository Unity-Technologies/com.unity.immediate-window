using System.Collections;
using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class CollapsedDictionaryGroup : VisualElement
    {
        public CollapsedDictionaryGroup(IDictionary obj)
        {
            AddToClassList("collapsedArrayGroup");
            
            Add(new Span("{"));

            int i = 0;
            foreach (var key in obj.Keys)
            {
                if (i > Config.ShowMaxCollapsedArrayItems)
                    break;
                if (i != 0)
                    Add(new Span(", "));

                var container = new Container("propertyValueGroup");
                var propertyValue = new TypeInspector(key, new ViewContext {Mode = ViewMode.Value});
                container.Add(propertyValue);
                container.Add(new Span(": "));
                var fieldValue = new TypeInspector(obj[key], new ViewContext {Mode = ViewMode.Value});
                container.Add(fieldValue);
                
                Add(container);

                i++;
            }

            if (i > Config.ShowMaxCollapsedArrayItems)
            {
                var expander = new Span(", (...)");
                Add(expander);
            }

            Add(new Span("}"));
        }
    }
}