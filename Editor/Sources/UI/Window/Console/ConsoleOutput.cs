using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Scripting;
using UnityEngine.UIElements;
using UnityEngine;
using Evaluator = UnityEditor.ImmediateWindow.Services.Evaluator;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ConsoleOutput : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<ConsoleOutput> { }
        
        private readonly VisualElement root;
        private bool isScrolled = false;
        public ScrollView Content { get; set; }

        public ConsoleOutput()
        {
            root = Resources.GetTemplate("ConsoleOutput.uxml");
            Add(root);
            ResetScrollView();
            
            Evaluator.Instance.OnEvaluationSuccess += OnEvaluationSuccess;
            Evaluator.Instance.OnEvaluationError += OnEvaluationError;
            Evaluator.Instance.OnBeforeEvaluation += OnBeforeEvaluation;
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

            AddObject(output);
        }

        public void AddObject(object obj)
        {
            Content.Add(new OutputItem(obj));
            ScrollToEnd();            
        }

        // TODO: Doesn't quite work.
        // Ugly hack to try and get it to scroll at the end. Need to check with RmGui
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

        
        public void ResetScrollView(bool transferPreviousContent = false)
        {
            var previous = new List<VisualElement>();
            if (Content != null)
            {
                previous = Content.Children().ToList();
                if (Children().Contains(Content))
                    Remove(Content);
            }

            Content = new ScrollView();
            Content.name = "output-content";
            Content.verticalScroller.slider.pageSize = 10;
            Content.contentContainer.StretchToParentWidth();
            Content.StretchToParentSize();
            Add(Content);

            // Transfer previous scrollview's content. This is a hack because ScrollView
            // Currently doesn't resize properly if put under a new parent.
            if (transferPreviousContent)
            {
                foreach (var child in previous)
                    Content.Add(child);

                ScrollToEnd();
            }
        }
        
        public void ClearLog()
        {
            // Currently need to remove the scroll view to clear it since Clear() doesn't reset scroll position.
            Remove(Content);
            ResetScrollView();
        }
    }
}
