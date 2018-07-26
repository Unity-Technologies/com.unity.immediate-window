using System;
using System.Collections.Generic;

namespace UnityEditor.ImmediateWindow.UI
{
    public class ViewContext
    {
        public event Action<ITypeView> OnTypeSwitch = delegate { };

        public ViewMode Mode = ViewMode.Default;
        public object Data = null;                                 // Arbitrary data typeviews can use
        public IEnumerable<ITypeView> Viewers;        // Viewers for this object (order in ascending priority)
        public ITypeView Viewer;                                  // View to use in this context

        public void SwitchType(ITypeView viewer)
        {
            Viewer = viewer;
            OnTypeSwitch(Viewer);
        }
    }
}