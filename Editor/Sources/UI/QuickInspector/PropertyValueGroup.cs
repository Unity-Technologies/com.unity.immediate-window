using System;
using UnityEngine;
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
                    e.tooltip = string.Format("Property {0} is private.", property.Field.Name);
                    e.StopImmediatePropagation();
                });
            }

            var label = property.Field != null ? property.Field.Name : property.Property.Name;

            try
            {
                var value = property.Field != null
                    ? property.Field.GetValue(property.Object)
                    : property.Property.GetValue(property.Object, null);

                var propertyLabel = new ProperyLabel(label);
                var fieldValue = new ObjectType(value, showValue);

                Add(propertyLabel);
                Add(fieldValue);
            }
            catch (Exception error)
            {
                var inner = "";
                if (error.InnerException != null)
                    inner = "\n\n\nInner Exception: " + error.InnerException.Message;
                
                Debug.LogError("Could not get property value: " + label + " -- " + error.Message + inner);
            }
        }
    }
}