using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ExpandedPropertyGroup : VisualElement
    {
        public ExpandedPropertyGroup(object obj)
        {
            AddToClassList("expandedPropertyGroup");

            var properties = PropertyUtils.GetPropertyInfo(obj).ToList();
            bool isClipped = properties.Count > Config.ShowMaxBlockProperties;
            properties = properties.Take(Config.ShowMaxBlockProperties).ToList();

            foreach (var property in properties)
            {
                Add(new PropertyValueGroup(property, true));
            }

            if (isClipped)
            {
                var expander = new Span("(...)");
                Add(expander);
            }
        }
    }
}