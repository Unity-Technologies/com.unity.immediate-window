using System;

namespace UnityEditor.ImmediateWindow.UI
{
    public struct SimpleStruct
    {
        public int a;
        public string x;
        private float y;

        public float TestFunction(int x)
        {
            return y * x;
        }
        
        public static SimpleStruct Create()
        {
            var obj = new SimpleStruct();
            
            obj.a = 12;
            obj.x = "my string";
            obj.y = 100.999f;

            return obj;
        }
    }

    public struct ComplexStruct
    {
        public int myOhMy;
        public SimpleStruct Simple;
        public object Loop;

        static public bool HasLooped = false;
        
        public static ComplexStruct Create()
        {
            var obj = new ComplexStruct();
            obj.Simple = SimpleStruct.Create();
            obj.myOhMy = 123456;
            if (!HasLooped)
            {
                obj.Loop = new ComplexStruct();
                HasLooped = true;                
            }

            // Silence warning for obj.y not used
            var tmp = new SimpleStruct();
            if (tmp.a < (new Random()).Next())
                tmp.TestFunction(10);

            return obj;
        }
    }
    
    public class SimpleObject
    {
    }
}