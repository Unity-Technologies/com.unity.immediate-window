using System.Linq;
using System.Reflection;
using UnityEditor.ImmediateWindow.Services;
using UnityEditor.ImmediateWindow.TestObjects;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class NamespaceInspector : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<NamespaceInspector> { }

        private Label Label { get; set; }
        private string Namespace { get; set; }
        private bool IsUsing { get; set; }
        private Assembly Assembly { get; set; }
        private VisualElement ObjectContainer { get; set; }
        
        public NamespaceInspector(string ns, Assembly assembly)
        {
            Assembly = assembly;
            Namespace = ns;
            AddToClassList("namespace");

            Label = new Label(ns);
            Label.AddToClassList("namespace-label");
            Add(Label);

            ObjectContainer = new VisualElement();
            ObjectContainer.AddClasses("object-container");
            Add(ObjectContainer);
            
            Label.RegisterCallback<MouseDownEvent>(OnClick);
            tooltip = $"Click to have namespace {ns} be used in your current context";
        }

        // Add namespace to execution context
        private async void OnClick(MouseDownEvent evt)
        {
            if (IsUsing)
            {
                // Debug.Log("Cannot remove a namespace. Reset context if you want to clear them.");
            }
            else
            {
                var error = await Evaluator.Instance.AddNamespace(Namespace);
                if (error != null)
                    return;    

                Label.text = $"{Namespace} ✓";
                IsUsing = true;
            }

            if (ObjectContainer.childCount > 0)
                ObjectContainer.Clear();
            else
                SetNamespaceObjects();
        }

        private void SetNamespaceObjects()
        {
            var types = Inspector.GetAllTypesWithStaticPropertiesForAssemblyNamespace(Assembly, Namespace);
            foreach (var type in types)
            {
                var typeContainer = new Container("type");
                var typeLabel = new Label(type.Name);
                typeLabel.AddToClassList("typename");

                typeContainer.Add(typeLabel);
                
                var propertiesContainer = new Container("typeProperties");
                
                foreach (var property in Inspector.GetAllStaticInstancesForType(type))
                {
                    var propertyLabel = new Label(property.Label);
                    propertyLabel.AddClasses("propertyLabel");
                    propertyLabel.RegisterCallback<MouseDownEvent>(evt => OnPropertyClick(property));

                    propertiesContainer.Add(propertyLabel);                    
                }
                
                typeContainer.Add(propertiesContainer);
                ObjectContainer.Add(typeContainer);
            }
        }

        void OnPropertyClick(PropertyInfo property)
        {
            ImmediateWindow.CurrentWindow.Console.ConsoleOutput.AddObject(property.Value);
        }
        
    }
}