using UnityEngine.Experimental.UIElements;
using UnityScript.Scripting;
using UnityEditor.ImmediateWindow.Services;
using UnityEngine;
using Evaluator = UnityEditor.ImmediateWindow.Services.Evaluator;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class Console : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<Console> { }
        private readonly VisualElement root;
        
        public Console()
        {
            root = Resources.GetTemplate("Console.uxml");
            Add(root);

            ConsoleInput.RegisterCallback<KeyDownEvent>(CodeEvaluate);
            ConsoleToolbar.Console = this;
        }
        
        private void CodeEvaluate(KeyDownEvent evt)
        {
            var doEvaluate = false;
            switch (evt.keyCode)
            {
                case KeyCode.Escape:
                    break;
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    doEvaluate = true;
                    break;
            }
            
            if (doEvaluate)
            {
                Evaluator.Instance.Evaluate(ConsoleInput.text);
            }
            else
            {
                // Autocomplete
            }
        }

        public TextField ConsoleInput {get { return root.Q<TextField>("console-input"); }}
        public ConsoleOutput ConsoleOutput {get { return root.Q<ConsoleOutput>("console-output"); }}
        private ConsoleToolbar ConsoleToolbar {get { return root.Q<ConsoleToolbar>("toolbarContainer"); }}
    }
}
