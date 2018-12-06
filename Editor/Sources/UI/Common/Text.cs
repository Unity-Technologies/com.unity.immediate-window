using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    // Text field that can be selected but not modified
    // For some odd reason, if it inhertis from TextField, selection doesn't work
    internal class Text : TextField
    {

        public static TextField Create(string text = null, bool asSpan = true, string classnames = "")
        {
            var field = new TextField();
            if (!string.IsNullOrEmpty(text))
                field.value = text;
            
            field.AddToClassList("text");
            field.AddClasses(classnames);
            
            if (asSpan)
                field.AddToClassList("span");
            
            field.RegisterCallback<KeyDownEvent>(evt =>
            {
                // Don't allow modification of this textfield
                evt.PreventDefault();
                evt.StopImmediatePropagation();
            });

            return field;
        }
    }
}