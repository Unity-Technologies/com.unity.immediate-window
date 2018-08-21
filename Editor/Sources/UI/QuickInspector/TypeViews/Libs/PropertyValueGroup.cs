using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class PropertyValueGroup : VisualElement
    {
        public PropertyValueGroup(PropertyInfo property, ViewContext context)
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

            // These generate exception with a Type. So skip until I know why
            if (property.GeneratesExceptions)
                return;

            var propertyLabel = new ProperyLabel(property.Label);
            var fieldValue = new TypeInspector(property.Value, context);

            Add(propertyLabel);
            Add(fieldValue);
        }
    }
}