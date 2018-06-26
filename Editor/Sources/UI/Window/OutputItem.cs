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
            Text.value = text;
        }

        private TextField Text {get { return root.Q<TextField>("text"); }}
    }
}
