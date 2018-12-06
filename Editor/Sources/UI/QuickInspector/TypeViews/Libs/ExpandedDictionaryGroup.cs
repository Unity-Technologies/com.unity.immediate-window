using System.Collections;
using System.Linq;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    // TODO: The Expanded+Collapsed Object/Array/Dictionary groups are very redundant
    internal class ExpandedDictionaryGroup : VisualElement
    {
        public ExpandedDictionaryGroup(IDictionary obj)
        {
            AddToClassList("expandedArrayGroup");

            var i = 0;
            foreach (var key in obj.Keys)
            {
                if (i > Config.ShowMaxCollapsedArrayItems)
                    break;

                var propertyLabel = new ProperyTypeLabel(key);
                var fieldValue = new TypeInspector(obj[key], new ViewContext {Mode = ViewMode.Default});
                
                var propertyValueGroup = new VisualElement();
                propertyValueGroup.AddToClassList("propertyValueGroup");
                propertyValueGroup.Add(propertyLabel);
                propertyValueGroup.Add(fieldValue);
              
                Add(propertyValueGroup);

                i++;
            }

            if (i > Config.ShowMaxCollapsedArrayItems)
            {
                var expander = new Span("(...)");
                Add(expander);
            }
        }
    }
}