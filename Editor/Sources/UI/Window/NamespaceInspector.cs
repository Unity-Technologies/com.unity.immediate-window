using UnityEditor.ImmediateWindow.Services;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class NamespaceInspector : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<NamespaceInspector> { }

        private Label Label { get; set; }
        private string Namespace { get; set; }
        private bool IsUsing { get; set; }
        
        public NamespaceInspector(string ns)
        {
            Namespace = ns;
            AddToClassList("namespace");

            Label = new Label(ns);
            Label.AddToClassList("namespace-label");
            Add(Label);
            
            RegisterCallback<MouseDownEvent>(OnClick);
            tooltip = $"Click to have namespace {ns} be used in your current context";
        }

        // Add namespace to execution context
        private async void OnClick(MouseDownEvent evt)
        {
            if (IsUsing)
            {
                Debug.Log("Cannot remove a namespace. Reset context if you want to clear them.");
                return;                
            }
            
            var error = await Evaluator.Instance.AddNamespace(Namespace);
            if (error != null)
                return;
                    
            Label.text = $"✓ {Namespace}";
            IsUsing = true;
        }
    }
}