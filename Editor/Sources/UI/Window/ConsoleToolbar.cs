using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ConsoleToolbar : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<ConsoleToolbar> { }
        private readonly VisualElement root;
        private bool ConsoleMultiline = false;

        public Console Console { get; set; }
        
        public ConsoleToolbar()
        {
            root = Resources.GetTemplate("ConsoleToolbar.uxml");
            Add(root);
            root.StretchToParentSize();
            
            ClearButton.RegisterCallback<MouseDownEvent>(ClearClick);
            ClearButton.RegisterCallback<MouseUpEvent>(ClearStopClick);
            MultilineButton.RegisterCallback<MouseDownEvent>(ConsoleExpandToggle);
            RunButton.RegisterCallback<MouseDownEvent>(RunClick);
            RunButton.RegisterCallback<MouseUpEvent>(ClearRunClick);
            PrivateToggle.OnValueChanged(OnPrivateToggle);

            OnPrivateToggle(null);
            RefreshConsoleState();
        }

        private void ClearRunClick(MouseUpEvent evt)
        {
            RunButton.RemoveFromClassList("pressed");
            Console.CodeEvaluate();
        }

        private void RunClick(MouseDownEvent evt)
        {
            RunButton.AddToClassList("pressed");
        }

        private void ConsoleExpandToggle(MouseDownEvent evt)
        {
            ConsoleMultiline = !ConsoleMultiline;
            RefreshConsoleState();
        }

        void RefreshConsoleState()
        {
            if (ConsoleMultiline)
                MultilineButton.AddToClassList("pressed");
            else
                MultilineButton.RemoveFromClassList("pressed");
            
            if (Console != null)
                Console.SetMode(ConsoleMultiline);
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
        private Label RunButton {get { return root.Q<Label>("run"); }}
        private Label MultilineButton {get { return root.Q<Label>("multiline"); }}
        private Toggle PrivateToggle {get { return root.Q<Toggle>("viewPrivate"); }}
    }
}