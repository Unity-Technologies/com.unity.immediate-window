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
    public class QuickInspector : VisualElement
    {
        private object Obj { get; set; }

        public QuickInspector()
        {
            Init();            
        }

        public QuickInspector(object obj)
        {
            Init();
            SetObject(obj);
        }

        void Init()
        {
            AddToClassList("quickInspector");            
        }

        public void SetObject(object obj)
        {
            try
            {
                Obj = obj;
                Add(new TypeInspector(obj, new ViewContext()));
            }
            catch (Exception e)
            {
                Debug.LogError("Error with quick inspector: " + e.Message + "\n\n" + e.StackTrace);
            }
        }
    }
}
