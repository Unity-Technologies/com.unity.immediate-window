using System.Collections;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ExpandedInterfaceGroup : VisualElement
    {
        public ExpandedInterfaceGroup(object obj)
        {
            foreach (var interfaceType in obj.GetType().GetInterfaces())
            {
                Add(new Interface(interfaceType, obj));                
            }
        }
    }
}