using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ObjectType : VisualElement
    {
        public ObjectType(object obj, bool showValue = true, bool isRoot = false) : base()
        {
            SetObject(obj, showValue, isRoot);
        }        

        public void SetObject(object obj, bool showValue = true, bool isRoot = false)
        {
            if (obj == null)
            {
                Span empty;
                if (isRoot)
                {
                    empty = new Span("empty result");
                    empty.AddToClassList("empty");
                }
                else
                {
                    empty = new Span("null");
                    empty.AddToClassList("isNull");                    
                }
                
                Add(empty);
                return;
            }

            var type = obj.GetType();

            var isSimpleType = type.IsPrimitive || type == typeof(string);

            if (isSimpleType)
            {
                var label = obj.ToString();
                if (type == typeof(string))
                    label = string.Format("\"{0}\"", label);
                
                var primitiveLabel = new Span(label);
                if (type.IsPrimitive)
                    primitiveLabel.AddToClassList("isNumber");
                else if (type == typeof(string))
                    primitiveLabel.AddToClassList("isString");
                
                Add(primitiveLabel);
            }
            else
            {
                Add(new ClassType(obj, showValue));
            }
        }
    }
}