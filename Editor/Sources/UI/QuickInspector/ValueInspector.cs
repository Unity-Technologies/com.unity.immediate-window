using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ValueInspector : VisualElement
    {
        public enum State
        {
            Collapsed,
            Expanded
        }

        private State _CurrentState = State.Collapsed;
        public State CurrentState
        {
            get { return _CurrentState; }
            set
            {
                RemoveFromClassList("collapsed");
                RemoveFromClassList("expanded");

                _CurrentState = value;

                if (_CurrentState == State.Collapsed)
                    AddToClassList("collapsed");
                else
                    AddToClassList("expanded");
            }
        }
        
        public ValueInspector()
        {
            AddToClassList("value");
            CurrentState = State.Collapsed;
        }
    }
}