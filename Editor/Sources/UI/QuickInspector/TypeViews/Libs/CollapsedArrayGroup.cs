using System.Collections;
using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class CollapsedArrayGroup : VisualElement
    {
        public CollapsedArrayGroup(IEnumerable obj)
        {
            AddToClassList("collapsedArrayGroup");
            
            Add(new Span("["));

            var objects = obj.Cast<object>().ToArray();

            bool isClipped = objects.Length > Config.ShowMaxCollapsedArrayItems;
            var items = objects.Take(Config.ShowMaxCollapsedArrayItems).ToList();

            int i = 0;
            foreach (var item in items)
            {
                if (i != 0)
                    Add(new Span(", "));

                var fieldValue = new TypeInspector(item, new ViewContext {Mode = ViewMode.Value});
                Add(fieldValue);
                i++;
            }

            if (isClipped)
            {
                var expander = new Span(", (...)");
                Add(expander);
            }

            Add(new Span("]"));
        }
    }
}