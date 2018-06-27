using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class PropertyValueGroup : VisualElement
    {
        public PropertyValueGroup(PropertyInfo property, bool showValue = true)
        {
            AddToClassList("propertyValueGroup");
            if (property.IsPrivate)
            {
                AddToClassList("private");  
                RegisterCallback<TooltipEvent>(e => {
                    e.rect = worldBound;
                    e.tooltip = string.Format("Property {0} is private.", property.Property.Name);
                    e.StopImmediatePropagation();
                });
            }
            
            var propertyLabel = new ProperyLabel(property.Property.Name);
            var fieldValue = new ObjectType(property.Property.GetValue(property.Object), showValue);

            Add(propertyLabel);
            Add(fieldValue);
        }
    }
}