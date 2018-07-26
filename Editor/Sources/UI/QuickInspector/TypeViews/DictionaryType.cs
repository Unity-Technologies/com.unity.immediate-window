using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class DictionaryType : ITypeView
    {
        public string Name { get; } = "Dictionary";

        // Slightly higher then Array/IEnumerable
        public virtual double Priority {get { return 0.1; }}

        public bool HasView(Type type)
        {
            if (type == null) return false;

            return typeof(IDictionary).IsAssignableFrom(type);
        }

        public VisualElement GetView(object obj, ViewContext context)
        {
            if (context.Mode == ViewMode.Default)
                return new ExpandableObject(obj, context);

            if (context.Mode == ViewMode.Collapsed)
                return new CollapsedDictionaryGroup(obj as IDictionary);
            if (context.Mode == ViewMode.Expanded)
                return new ExpandedDictionaryGroup(obj as IDictionary);
            if (context.Mode == ViewMode.Value)
                return new TypeNameView(obj);
            
            throw new Exception("Should not get here");
        }
    }
}
