using UnityEngine.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class Arrow : TextElement
    {
        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }
        
        public Arrow(Direction direction = Direction.Right, string defaultClass = "arrow")
        {
            AddToClassList(defaultClass);            
            SetDirection(direction);
        }

        public void SetDirection(Direction direction)
        {
            if (direction == Direction.Left)
                text = "◄";
            else if (direction == Direction.Right)
                text = "►";
            else if (direction == Direction.Up)
                text = "▲";
            else if (direction == Direction.Down)
                text = "▼";
        }
    }
}