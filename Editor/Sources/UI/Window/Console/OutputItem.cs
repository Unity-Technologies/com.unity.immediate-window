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
            
            AddToClassList("output-item");
        }

        public OutputItem(string text) : this()
        {
            var textField = Text.Create(text);
            textField.multiline = true;
            textField.AddToClassList("faded");
            textField.AddToClassList("console-log");
            Add(textField);
        }

        public OutputItem(object obj) : this()
        {
            Add(new QuickInspector(obj));
        }
    }
}
