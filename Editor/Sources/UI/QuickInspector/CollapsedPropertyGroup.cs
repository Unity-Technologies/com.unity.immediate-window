using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class CollapsedPropertyGroup : VisualElement
    {
        public CollapsedPropertyGroup(object obj)
        {
            AddToClassList("collapsedPropertyGroup");
            
            Add(new Span("{"));

            var properties = PropertyUtils.GetPropertyInfo(obj).ToList();
            bool isClipped = properties.Count > Config.ShowMaxCollapsedProperties;
            properties = properties.Take(Config.ShowMaxCollapsedProperties).ToList();

            foreach (var property in properties)
            {
                if (property.Property != properties.First().Property)
                    Add(new Span(", "));

                Add(new PropertyValueGroup(property, false));
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