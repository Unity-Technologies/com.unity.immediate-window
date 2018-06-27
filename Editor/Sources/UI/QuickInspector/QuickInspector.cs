using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UnityEditor.ImmediateWindow.UI
{
    /**
     * Displays most generic object (will decide what is the proper display type should be)
     */

    internal class QuickInspector : VisualElement
    {
        private object Obj { get; set; }
        
        public QuickInspector() {}

        public QuickInspector(object obj)
        {
            SetObject(obj);
            AddToClassList("quickInspector");
        }

        public void SetObject(object obj)
        {
            try
            {
                Obj = obj;
                Add(new ObjectType(obj, true, true));
            }
            catch (Exception e)
            {
                Debug.LogError("Error with quick inspector: " + e.Message + "\n\n" + e.StackTrace);
            }
        }
    }
}
