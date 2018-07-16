using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class Container : VisualElement
    {
        public Container(string classnames = "container")
        {
            this.AddClasses(classnames);
        }
    }
}