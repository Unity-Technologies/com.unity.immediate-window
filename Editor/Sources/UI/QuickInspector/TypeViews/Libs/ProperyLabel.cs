using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ProperyLabel : VisualElement
    {
        public ProperyLabel(string property)
        {
            AddToClassList("propertyLabelGroup");

            var propertLabel = new Span(property);
            propertLabel.AddToClassList("propertyLabel");
            Add(propertLabel);
            Add(new Span(": "));
        }
    }
}