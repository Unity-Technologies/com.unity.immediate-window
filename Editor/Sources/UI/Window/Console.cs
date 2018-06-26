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
        private TextField ConsoleInput { get; set; }
        private TextField ConsoleInputMultiLine { get; set; }
        private bool MultiLineMode { get; set; }
        
        public ConsoleOutput ConsoleOutput { get; set; }

        public Console()
        {
            root = Resources.GetTemplate("Console.uxml");
            Add(root);

            ConsoleOutput = new ConsoleOutput();
            ConsoleOutput.name = "console-output";

            ConsoleInput =  new TextField();
            ConsoleInput.name = "console-input";
            ConsoleInput.RegisterCallback<KeyDownEvent>(OnInputKeyPressed);
            ConsoleSingleLine.Add(ConsoleOutput);
            ConsoleSingleLine.Add(ConsoleInput);
            
            ConsoleInputMultiLine = new TextField();
            ConsoleInputMultiLine.multiline = true;
            ConsoleInputMultiLine.name = "console-input-multiline";
            ConsoleMultiLine.Add(ConsoleInputMultiLine);
            
            ConsoleToolbar.Console = this;

            SetMode(false);
        }

        public void SetMode(bool multiline)
        {
            MultiLineMode = multiline;
            
            if (MultiLineMode)
            {
                if (ConsoleSingleLine.Contains(ConsoleOutput))
                    ConsoleSingleLine.Remove(ConsoleOutput);
                
                UIUtils.SetElementDisplay(ConsoleSingleLine, !MultiLineMode);
                UIUtils.SetElementDisplay(ConsoleMultiLine, MultiLineMode);

                ConsoleMultiLine.Add(ConsoleOutput);
                RemoveFromClassList("singleline");
                AddToClassList("multiline");
            }
            else
            {
                if (ConsoleMultiLine.Contains(ConsoleOutput))
                    ConsoleMultiLine.Remove(ConsoleOutput);
                
                UIUtils.SetElementDisplay(ConsoleSingleLine, !MultiLineMode);
                UIUtils.SetElementDisplay(ConsoleMultiLine, MultiLineMode);

                ConsoleSingleLine.Insert(0, ConsoleOutput);
                AddToClassList("singleline");
                RemoveFromClassList("multiline");
            }
        }

        private void OnInputKeyPressed(KeyDownEvent evt)
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
                CodeEvaluate();
            }
            else
            {
                // Autocomplete
            }            
        }
        
        public void CodeEvaluate()
        {
            var code = MultiLineMode ? ConsoleInputMultiLine.text : ConsoleInput.text;
            if (!MultiLineMode)
                ConsoleInput.value = "";
            
            Evaluator.Instance.Evaluate(code);
        }

        private VisualElement ConsoleSingleLine {get { return root.Q<VisualElement>("console-mode-singleline"); }}
        private VisualElement ConsoleMultiLine {get { return root.Q<VisualElement>("console-mode-multiline"); }}
        private ConsoleToolbar ConsoleToolbar {get { return root.Q<ConsoleToolbar>("toolbarContainer"); }}
    }
}
