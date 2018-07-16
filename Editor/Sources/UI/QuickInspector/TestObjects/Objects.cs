using System;
using System.Collections.Generic;

namespace UnityEditor.ImmediateWindow.TestObjects
{
    public class SecretStruct
    {
        public int BestValue = 1978;
    }
}

namespace UnityEditor.ImmediateWindow.UI
{
    public class SimpleObject
    {
        public int a;
        public string x;
        private float y;

        public float TestFunction(int x)
        {
            return y * x;
        }
        
        public static SimpleObject Create()
        {
            var obj = new SimpleObject();
            
            obj.a = 12;
            obj.x = "my string";
            obj.y = 100.999f;

            return obj;
        }
    }

    public class ComplexObject
    {
        public int myOhMy;
        public SimpleObject Simple;
        public object Loop;
        public int[] Numbers = new int[] {4, 5, 6};
        public string[] Strings = new string[] {"yes", "works!"};
        public IEnumerable<string> StringsEnum = new List<string> {"yes enum", "works enum!"};
        public object ObjectArray = new object[] {"yes enum", "works enum!", 123, SimpleObject.Create()};

        static public bool HasLooped = false;
        
        public static ComplexObject Create()
        {
            var obj = new ComplexObject();
            obj.Simple = SimpleObject.Create();
            obj.myOhMy = 123456;
            if (!HasLooped)
            {
                obj.Loop = new ComplexObject();
                HasLooped = true;                
            }

            // Silence warning for obj.y not used
            var tmp = new SimpleObject();
            if (tmp.a < (new Random()).Next())
                tmp.TestFunction(10);

            return obj;
        }
    }
}