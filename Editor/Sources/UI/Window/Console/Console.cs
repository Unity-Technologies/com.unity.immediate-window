using UnityEngine.UIElements;
using UnityEditor.ImmediateWindow.Services;
using UnityEngine;
using Evaluator = UnityEditor.ImmediateWindow.Services.Evaluator;
using UnityEditor.Analytics;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class Console : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<Console> { }
        
        private readonly VisualElement root;
        private TextField ConsoleInput { get; set; }
        private TextField ConsoleInputMultiLine { get; set; }
        private bool MultiLineMode
        {
            get { return State.Instance.MultiLineMode;}
            set { State.Instance.MultiLineMode = value; }
        }
        public Command CurrentCommand { get; set; }
        
        public ConsoleOutput ConsoleOutput { get; set; }
        public ConsoleOutput ConsoleOutputMultiline { get; set; }

        /// <summary>
        /// Todo: separate in different class multiline/singleline
        /// </summary>
        public Console()
        {
            root = Resources.GetTemplate("Console.uxml");
            Add(root);
            root.StretchToParentSize();

            ConsoleOutput = new ConsoleOutput();
            ConsoleOutput.name = "console-output";

            ConsoleInput =  new TextField();
            ConsoleInput.multiline = false;
            ConsoleInput.name = "console-input";
            ConsoleInput.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (!IsKeyEventUp(evt.keyCode))
                    OnInputKeyPressed(evt.keyCode, evt.character);
            });
            // Currently a bug where KeyDown event is not sent for up/down arrow
            ConsoleInput.RegisterCallback<KeyUpEvent>(evt =>
            {
                if (IsKeyEventUp(evt.keyCode))
                    OnInputKeyPressed(evt.keyCode, evt.character);
            });
            ConsoleSingleLine.Add(ConsoleOutput);
            ConsoleSingleLine.Add(ConsoleInput);
            
            ConsoleInputMultiLine = new TextField();
            ConsoleInputMultiLine.multiline = true;
            ConsoleInputMultiLine.name = "console-input-multiline";
            ConsoleMultiLine.Add(ConsoleInputMultiLine);
            ConsoleInputMultiLine.RegisterCallback<KeyDownEvent>(OnMultiLineInputKeyPressed);
            ConsoleInputMultiLine.RegisterCallback<FocusOutEvent>(OnMultiLineFocusOut);
            
            ConsoleToolbar.Console = this;

            SetMode(MultiLineMode);
        }

        private bool IsKeyEventUp(KeyCode keyCode)
        {
            return keyCode == KeyCode.UpArrow || keyCode == KeyCode.DownArrow;
        }

        private void OnMultiLineFocusOut(FocusOutEvent evt)
        {
            SaveMultiLineState();
        }

        public void SetMode(bool multiline)
        {
            var changed = MultiLineMode == multiline;
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

                SetMultilineCode(State.Instance.ScriptCode, false);
            }
            else
            {
                // Make sure we store current script since we always restore it from state 
                // when changing mode
                if (changed)
                    SaveMultiLineState();

                if (ConsoleMultiLine.Contains(ConsoleOutput))
                    ConsoleMultiLine.Remove(ConsoleOutput);
                
                UIUtils.SetElementDisplay(ConsoleSingleLine, !MultiLineMode);
                UIUtils.SetElementDisplay(ConsoleMultiLine, MultiLineMode);

                ConsoleSingleLine.Insert(0, ConsoleOutput);
                AddToClassList("singleline");
                RemoveFromClassList("multiline");
            }
            
            ConsoleOutput.ResetScrollView(true);
        }

        private void OnMultiLineInputKeyPressed(KeyDownEvent evt)
        {
            var doEvaluate = false;
            switch (evt.keyCode)
            {
                case KeyCode.Escape:
                    break;
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                {
                    if (evt.ctrlKey || evt.commandKey)
                    {
                        doEvaluate = true;
                        evt.PreventDefault();
                        evt.StopImmediatePropagation();
                    }
                    break;                    
                }
            }
            
            if (doEvaluate)
                CodeEvaluate();
        }

        private void OnInputKeyPressed(KeyCode keyCode, char character)
        {
            var doEvaluate = false;
            switch (keyCode)
            {
                case KeyCode.UpArrow:
                {
                    PreviousCommand();
                    break;
                }
                case KeyCode.DownArrow:
                {
                    NextCommand();
                    break;
                }
                case KeyCode.Escape:
                    break;
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    doEvaluate = true;
                    break;
            }

            if (character == '\n')
            {
                doEvaluate = true;
                var textinput = ConsoleInput.Q<VisualElement>("unity-text-input");
                if (textinput != null)
                    textinput.Focus();
            }

            if (doEvaluate)
                CodeEvaluate();
            
            Evaluator.Instance.GetAutocomplete(ConsoleInput.text);
        }

        public void PreviousCommand()
        {
            CurrentCommand = History.Instance.PreviousCommand(CurrentCommand);
            if (CurrentCommand != null)
            {
                ConsoleInput.value = CurrentCommand.code;
            }
        }

        public void NextCommand()
        {
            CurrentCommand = History.Instance.NextCommand(CurrentCommand);
            if (CurrentCommand != null)
                ConsoleInput.value = CurrentCommand.code;
            else
                ConsoleInput.value = ""; // Clear when reach the end
        }

        public void CodeEvaluate()
        {
            var code = MultiLineMode ? ConsoleInputMultiLine.text : ConsoleInput.text;
            if (!MultiLineMode)
            {
                CurrentCommand = null;
                History.Instance.AddCommand(code);
                ConsoleInput.value = "";
            }
            else
            {
                SaveMultiLineState();    // Save last code to survive domain reloads
            }

            EditorAnalytics.SendEventWithLimit("evaluatecode", new ImmediateWindowAnalytics {MultilineMode = MultiLineMode});
            
            Evaluator.Instance.Evaluate(code);
        }

        public void SetMultilineCode(string code, bool save = true)
        {
            ConsoleInputMultiLine.value = code;

            if (save)
                SaveMultiLineState();
        }

        public void SaveMultiLineState()
        {
            State.Instance.ScriptCode = ConsoleInputMultiLine.value ;
        }

        private VisualElement ConsoleSingleLine {get { return root.Q<VisualElement>("console-mode-singleline"); }}
        private VisualElement ConsoleMultiLine {get { return root.Q<VisualElement>("console-mode-multiline"); }}
        private ConsoleToolbar ConsoleToolbar {get { return root.Q<ConsoleToolbar>("toolbarContainer"); }}
    }
}
