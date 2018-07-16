using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class Span : TextElement
    {
        public Span()
        {
            AddToClassList("span");
        }

        public Span(string text, string classnames = "", string tooltip = "") : this()
        {
            Init(text, classnames, tooltip);
        }

        public void Init(string text, string classnames = "", string tooltip = "")
        {
            this.text = text;

            if (!string.IsNullOrEmpty(tooltip))
                this.tooltip = tooltip;

            this.AddClasses(classnames);
        }
    }
}
