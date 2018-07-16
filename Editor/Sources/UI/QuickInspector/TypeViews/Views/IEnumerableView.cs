using System.Collections;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class IEnumerableView : VisualElement
    {
        public IEnumerableView(object obj)
        {
            var enumerable = obj as IEnumerable;
            bool isEnumerable = enumerable != null;

            if (isEnumerable)
            {
                var interfaceInspector = new VisualElement();
                interfaceInspector.AddToClassList("propertyValueGroup");

                var label = new ProperyLabel("IEnumerable");
                label.AddToClassList("enumerable");
                interfaceInspector.Add(label);
                interfaceInspector.Add(new IEnumerableInspector(enumerable));
                
                interfaceInspector.RegisterCallback<MouseDownEvent>(evt =>
                {
                    
                });
                Add(interfaceInspector);
            }
        }
    }
}