using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal static class UIUtils
    {
        private const string DisplayNone = "display-none";

        public static void SetElementDisplay(VisualElement element, bool value)
        {
            if (element == null)
                return;

            if (value)
                element.RemoveFromClassList(DisplayNone);
            else
                element.AddToClassList(DisplayNone);

            element.visible = value;
        }
    }
}
