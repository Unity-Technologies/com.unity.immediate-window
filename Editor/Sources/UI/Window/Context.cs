using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class Context : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<Context> { }

        private readonly VisualElement root;
        public ContextInspector ContextInspector { get; set; }

        public Context()
        {
            root = Resources.GetTemplate("Context.uxml");
            Add(root);
            root.StretchToParentSize();
            
            Reset();
        }

        public void Reset()
        {
            if (ContextInspector != null)
                Remove(ContextInspector);
            
            ContextInspector = new ContextInspector();
            Add(ContextInspector);
        }
    }
}