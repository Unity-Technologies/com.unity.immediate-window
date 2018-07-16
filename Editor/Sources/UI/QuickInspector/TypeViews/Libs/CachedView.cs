using System;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class CachedView
    {
        private Func<VisualElement> ViewFactory { get; set; }

        private VisualElement _View;
        public VisualElement View
        {
            get
            {
                if (_View == null)
                    _View = ViewFactory();

                return _View;
            }
        }

        public CachedView(Func<VisualElement> viewFactory)
        {
            ViewFactory = viewFactory;
        }
    }
}