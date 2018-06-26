using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ConsoleToolbar : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<ConsoleToolbar> { }
        private readonly VisualElement root;

        public Console Console { get; set; }
        
        public ConsoleToolbar()
        {
            root = Resources.GetTemplate("ConsoleToolbar.uxml");
            Add(root);
            root.StretchToParentSize();
            
            ClearButton.RegisterCallback<MouseDownEvent>(ClearClick);
            ClearButton.RegisterCallback<MouseUpEvent>(ClearStopClick);
            PrivateToggle.OnValueChanged(OnPrivateToggle);

            OnPrivateToggle(null);
        }

        private void ClearStopClick(MouseUpEvent evt)
        {
            ClearButton.RemoveFromClassList("pressed");
        }

        private void ClearClick(MouseDownEvent evt)
        {
            ClearButton.AddToClassList("pressed");
            Console.ConsoleOutput.ClearLog();
        }

        private void OnPrivateToggle(ChangeEvent<bool> evt)
        {
            PropertyUtils.ShowPrivate = PrivateToggle.value;
        }

        private Label ClearButton {get { return root.Q<Label>("clear"); }}
        private Toggle PrivateToggle {get { return root.Q<Toggle>("viewPrivate"); }}
    }
}