using System.Linq;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class ExpandedPropertyGroup : VisualElement
    {
        public ExpandedPropertyGroup(object obj)
        {
            AddToClassList("expandedPropertyGroup");
            
            //
            // Properties
            var properties = PropertyUtils.GetPropertyInfo(obj).ToList();
            bool isClipped = properties.Count > Config.ShowMaxBlockProperties;
            properties = properties.Take(Config.ShowMaxBlockProperties).ToList();

            foreach (var property in properties)
            {
                Add(new PropertyValueGroup(property, new ViewContext {Mode = ViewMode.Default}));
            }

            if (isClipped)
            {
                var expander = new Span("(...)");
                Add(expander);
            }
        }
    }
}