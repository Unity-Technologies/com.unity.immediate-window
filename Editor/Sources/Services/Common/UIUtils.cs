using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal static class UIUtils
    {
        public static void SetElementDisplay(VisualElement element, bool value)
        {
            if (element == null)
                return;

            element.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            element.style.visibility = value ? Visibility.Visible : Visibility.Hidden;
        }
    }
}

