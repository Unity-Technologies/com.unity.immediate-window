using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    /// <summary>
    /// Basic inspector view for any type. This will use the type finder mechanism which searches for all
    /// classes implementing ITypeView, and will provide the correct view for those types. 
    /// </summary>
    public class TypeInspector : VisualElement
    {
        /// <summary>
        /// Constructor for typeview.
        /// </summary>
        /// <param name="obj">Object to view</param>
        /// <param name="context"></param>
        public TypeInspector(object obj, ViewContext context)
        {
            AddToClassList("typeInspector");
            SetObject(obj, context);
        }        

        internal void SetObject(object obj, ViewContext context)
        {
            if (context.Viewer == null)
            {
                var viewers = TypeRouter.Instance.GetTypeViewer(obj).OrderBy(t => t.Priority);
                if (viewers.Any())
                {
                    context.Viewers = viewers;
                    context.Viewer = viewers.Last();
                }
                else
                {
                    throw new Exception("No type viewer found for object (should never happen).");
                }
            }

            if (context.Viewer == null)
            {
                Debug.LogWarning("No context viewer found for type inspector.");
                return;
            }
            
            
            Add(context.Viewer.GetView(obj, context));
        }
    }
}
