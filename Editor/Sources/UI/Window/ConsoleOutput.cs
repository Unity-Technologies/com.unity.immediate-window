using Microsoft.CodeAnalysis.Scripting;
using UnityEngine.Experimental.UIElements;
using UnityScript.Scripting;
using UnityEditor.ImmediateWindow.Services;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Experimental.UIElements.StyleEnums;
using Evaluator = UnityEditor.ImmediateWindow.Services.Evaluator;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ConsoleOutput : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<ConsoleOutput> { }
        
        private readonly VisualElement root;
        private bool isScrolled = false;
        private ScrollView Content { get; set; }

        public ConsoleOutput()
        {
            root = Resources.GetTemplate("ConsoleOutput.uxml");
            Add(root);
            CreateScrollView();
            
            Evaluator.Instance.OnEvaluationSuccess += OnEvaluationSuccess;
            Evaluator.Instance.OnEvaluationError += OnEvaluationError;
            Evaluator.Instance.OnBeforeEvaluation += OnBeforeEvaluation;
            
            //Content.Add(new QuickInspector(1));
        }

        private void OnBeforeEvaluation(string code)
        {
            isScrolled = Content.scrollOffset.y > Content.verticalScroller.highValue;
        }

        private void OnEvaluationSuccess(string output)
        {
            Content.Add(new OutputItem(output));

            EditorApplication.update += UpdateScroll;
        }

        bool SimpleOutput = false;
        private void OnEvaluationSuccess(object output)
        {
            if (SimpleOutput)
            {
                if (output == null)
                    output = "(no result -- perhaps you ended your statement with a ';' ?)";

                Content.Add(new OutputItem(output.ToString()));                
            }
            
            Content.Add(new QuickInspector(output));

            ScrollToEnd();
        }

        private void ScrollToEnd()
        {
            EditorApplication.update += UpdateScroll;
        }

        private void OnEvaluationError(string output, CompilationErrorException error)
        {
            Content.Add(new OutputItem(output));

            ScrollToEnd();
        }

        void UpdateScroll()
        {
            if (Content.verticalScroller.highValue != 100)
                Content.scrollOffset = new Vector2(0, Content.verticalScroller.highValue);
            
            EditorApplication.update -= UpdateScroll;
        }

        
        void CreateScrollView()
        {
            Content = new ScrollView();
            Content.name = "output-content";
            Content.verticalScroller.slider.pageSize = 10;
            Add(Content);
            Content.stretchContentWidth = true;
        }
        
        public void ClearLog()
        {
            // Currently need to remove the scroll view to clear it since Clear() doesn't reset scroll position.
            Remove(Content);
            CreateScrollView();
        }
    }
}
