using System.Reflection;
using UnityEditor.ImmediateWindow.Services;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class AssemblyInspector : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<AssemblyInspector> { }

        private Arrow Arrow { get; set; }
        private VisualElement Label { get; set; }
        private VisualElement Container { get; set; }
        private bool Expanded { get; set; }
        private Assembly Assembly { get; set; }
        
        public AssemblyInspector(Assembly assembly)
        {
            Assembly = assembly;
            AddToClassList("assembly");
            
            Label = new VisualElement();
            Label.AddToClassList("row");

            Arrow = new Arrow(Arrow.Direction.Right);
            Label.Add(Arrow);

            var label = new Label(assembly.GetName().Name);
            label.AddToClassList("assembly-label");
            Label.Add(label);

            Add(Label);
            
            Container = new VisualElement();
            Add(Container);
            
            Label.RegisterCallback<MouseDownEvent>(OnLabelClick);
        }

        private void OnLabelClick(MouseDownEvent evt)
        {
            Expanded = !Expanded;
            Arrow.SetDirection(Expanded ? Arrow.Direction.Down : Arrow.Direction.Right);
            
            if (Expanded)
            {
                foreach (var ns in Inspector.GetAllNamespaces(Assembly))
                {
                    var inspector = new NamespaceInspector(ns, Assembly);
                    Container.Add(inspector);
                }
            }
            else
            {
                Container.Clear();
            }
        }
    }
}