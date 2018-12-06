using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class ExpandedClassType : VisualElement
    {
        public ExpandedClassType(object obj)
        {
            if (obj.GetType().GetInterfaces().Any())
                Add(new ExtendedExpandable("Interfaces", new ExpandedInterfaceGroup(obj), expanded: false, labelClassNames: "expandedClassLabel"));

            var properties = new ExtendedExpandable("Properties", new ExpandedPropertyGroup(obj), expanded: true, labelClassNames: "expandedClassLabel");
            Add(properties);
            
            if (obj.GetType().GetMethods().Any())
                Add(new ExtendedExpandable("Methods", new ExpandedMethodGroup(obj), labelClassNames: "expandedClassLabel"));
        }
    }
}