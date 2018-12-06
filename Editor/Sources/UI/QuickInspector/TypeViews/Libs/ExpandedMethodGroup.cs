using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ExpandedMethodGroup : VisualElement
    {
        public ExpandedMethodGroup(object obj)
        {
            foreach (var method in obj.GetType().GetMethods())
            {
                Add(new Method(method, obj));
            }
        }
    }
}