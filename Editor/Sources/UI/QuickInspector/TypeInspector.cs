using System;
using System.Collections;
using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class TypeInspector : VisualElement
    {
        public TypeInspector(object obj, ViewContext context)
        {
            AddToClassList("typeInspector");
            SetObject(obj, context);
        }        

        public void SetObject(object obj, ViewContext context)
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

            Add(context.Viewer.GetView(obj, context));
        }
    }
}
