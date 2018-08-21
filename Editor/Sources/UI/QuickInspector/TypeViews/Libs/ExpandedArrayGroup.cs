using System.Collections;
using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class ExpandedArrayGroup : VisualElement
    {
        public ExpandedArrayGroup(object obj)
        {
            AddToClassList("expandedArrayGroup");

            var objects = ((IEnumerable) obj).Cast<object>().ToArray();

            bool isClipped = objects.Length > Config.ShowMaxArrayProperties;
            var items = objects.Take(Config.ShowMaxBlockProperties).ToList();

            var i = 0;
            foreach (var item in items)
            {                
                var propertyLabel = new ProperyLabel(i.ToString());        // Index
                var fieldValue = new TypeInspector(item, new ViewContext() {Mode = ViewMode.Default});
                
                var propertyValueGroup = new VisualElement();
                propertyValueGroup.AddToClassList("propertyValueGroup");
                propertyValueGroup.Add(propertyLabel);
                propertyValueGroup.Add(fieldValue);
              
                Add(propertyValueGroup);

                i++;
            }

            if (isClipped)
            {
                var expander = new Span("(...)");
                Add(expander);
            }
        }
    }
}