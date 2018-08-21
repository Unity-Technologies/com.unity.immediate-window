using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class CollapsedPropertyGroup : VisualElement
    {
        public CollapsedPropertyGroup(object obj)
        {
            AddToClassList("collapsedPropertyGroup");
            
            Add(new Span("{"));

            var properties = PropertyUtils.GetPropertyInfo(obj).ToList();
            bool isClipped = properties.Count > Config.ShowMaxCollapsedProperties;
            properties = properties.Take(Config.ShowMaxCollapsedProperties).ToList();

            int i = 0;
            foreach (var property in properties)
            {
                if (i != 0)
                    Add(new Span(", "));

                Add(new PropertyValueGroup(property, new ViewContext {Mode = ViewMode.Value}));
                i++;
            }

            if (isClipped)
            {
                var expander = new Span(", (...)");
                Add(expander);
            }

            Add(new Span("}"));            
        }
    }
}