using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class Span : TextElement
    {
        public Span()
        {
            AddToClassList("span");            
        }
        
        public Span(string text) : this()
        {
            this.text = text;
        }
    }
}
