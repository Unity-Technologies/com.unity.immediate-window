using System;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class Interface : VisualElement
    {
        public Interface(Type interfaceType, object obj)
        {
            var parsed = new ParsedAssemblyQualifiedName.ParsedAssemblyQualifiedName(interfaceType.AssemblyQualifiedName, true);
            var parsedLong = new ParsedAssemblyQualifiedName.ParsedAssemblyQualifiedName(interfaceType.AssemblyQualifiedName);

            var interfaceSpan = new Span(parsed.CSharpStyleName.Value, "interface");
            interfaceSpan.tooltip = parsedLong.CSharpStyleName.Value;
            Add(interfaceSpan);
        }
    }
}