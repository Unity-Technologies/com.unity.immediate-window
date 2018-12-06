using UnityEditor.ImmediateWindow.Services;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ContextInspector : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<ContextInspector> { }

        private ScrollView Container { get; set; }
        public ContextInspector()
        {
            AddToClassList("context-inspector");
            Refresh();
        }

        private void Refresh()
        {
            Container = new ScrollView();
            Container.contentContainer.StretchToParentWidth();
            Container.StretchToParentSize();
            Container.AddToClassList("context-inspector-scrollview");
            
            foreach (var assembly in Inspector.GetAllAssemblies())
            {
                var inspector = new AssemblyInspector(assembly);
                Container.Add(inspector);
            }
            
            Add(Container);
        }
    }
}