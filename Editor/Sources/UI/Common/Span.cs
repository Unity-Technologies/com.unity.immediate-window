using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    /// <summary>
    /// Shortcut to create inline VisualElements
    /// </summary>
    public class Span : TextElement
    {
        /// <summary>
        /// Basic span creation
        /// </summary>
        public Span()
        {
            AddToClassList("span");
        }

        /// <summary>
        /// Span creation
        /// </summary>
        /// <param name="text">Text to set your inline element to</param>
        /// <param name="classnames">Whitespace-separated list of classes to add</param>
        /// <param name="tooltip">Tooltip text</param>
        public Span(string text, string classnames = "", string tooltip = "") : this()
        {
            Init(text, classnames, tooltip);
        }

        internal void Init(string text, string classnames = "", string tooltip = "")
        {
            this.text = text;

            if (!string.IsNullOrEmpty(tooltip))
                this.tooltip = tooltip;

            this.AddClasses(classnames);
        }
    }
}
