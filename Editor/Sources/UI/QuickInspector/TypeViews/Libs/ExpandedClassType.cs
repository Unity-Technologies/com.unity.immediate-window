using System.Collections;
using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ExpandedClassType : VisualElement
    {
        public ExpandedClassType(object obj)
        {
            if (obj is IEnumerable)
                Add(new ExtendedExpandable("Interfaces", new IEnumerableView(obj), expanded: false, labelClassNames: "expandedClassLabel"));

            var properties = new ExtendedExpandable("Properties", new ExpandedPropertyGroup(obj), expanded: true, labelClassNames: "expandedClassLabel");
            Add(properties);
            
            if (obj.GetType().GetMethods().Any())
                Add(new ExtendedExpandable("Methods", new ExpandedMethodGroup(obj), labelClassNames: "expandedClassLabel"));
        }
    }
}