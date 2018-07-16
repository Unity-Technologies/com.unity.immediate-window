using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class PropertyValueGroup : VisualElement
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

            var label = property.Field != null ? property.Field.Name : property.Property.Name;

            // These generate exception with a Type. So skip until I know why
            if (label == "GenericParameterAttributes" || label == "GenericParameterPosition" ||
                label == "DeclaringMethod")
                return;
                
            try
            {
                var value = property.Field != null
                    ? property.Field.GetValue(property.Object)
                    : property.Property.GetValue(property.Object, null);
    
                var propertyLabel = new ProperyLabel(label);
                var fieldValue = new TypeInspector(value, context);

                Add(propertyLabel);
                Add(fieldValue);
            }
            catch (Exception error)
            {
                var message = "Could not get property value: " + label + " -- " + error.Message;
                if (error.StackTrace != null)
                    message += "\n\nStack trace: " + error.StackTrace;
                if (error.InnerException != null)
                    message += "\n\n\nInner Exception: " + error.InnerException.Message;
                
                Debug.LogError(message);
            }
        }
    }
}