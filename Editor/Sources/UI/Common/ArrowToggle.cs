namespace UnityEditor.ImmediateWindow.UI
{
    internal class ArrowToggle : Arrow
    {      
        private bool _Expanded;
        public bool Expanded
        {
            get { return _Expanded; }
            set
            {
                _Expanded = value;

                this.EnableClassToggle("expanded", "collapsed", Expanded);

                if (_Expanded)
                    SetDirection(Direction.Down);
                else
                    SetDirection(Direction.Right);
            }
        }
        
        public ArrowToggle()
        {
            Expanded = false;
        }

        public void Toggle()
        {
            Expanded = !Expanded;
        }
    }
}