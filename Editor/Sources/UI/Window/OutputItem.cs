using UnityEngine.Experimental.UIElements;
using UnityScript.Scripting;
using UnityEditor.ImmediateWindow.Services;
using UnityEngine;
using Evaluator = UnityEditor.ImmediateWindow.Services.Evaluator;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class OutputItem : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<ConsoleOutput> { }
        
        private readonly VisualElement root;
        
        public OutputItem()
        {
            root = Resources.GetTemplate("OutputItem.uxml");
            Add(root);
            SetEnabled(false);
            
            AddToClassList("output-item");
        }

        public OutputItem(string text) : this()
        {
            var textField = new TextField();
            textField.multiline = true;
            textField.name = "text";
            textField.value = text;
            Add(textField);
        }

        public OutputItem(object obj) : this()
        {
            Add(new QuickInspector(obj));
        }
    }
}
