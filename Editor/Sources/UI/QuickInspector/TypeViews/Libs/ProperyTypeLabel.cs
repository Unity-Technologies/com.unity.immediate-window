using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class ProperyTypeLabel : VisualElement
    {
        public ProperyTypeLabel(object property)
        {
            AddToClassList("propertyLabelGroup");

            var propertLabel = new TypeInspector(property, new ViewContext {Mode = ViewMode.Value});
            propertLabel.AddToClassList("propertyLabel");
            Add(propertLabel);
            Add(new Span(": "));
        }
    }
}