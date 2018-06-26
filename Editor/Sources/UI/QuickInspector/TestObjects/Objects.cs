namespace UnityEditor.ImmediateWindow.UI
{
    internal struct SimpleStruct
    {
        public int a;
        public string x;
        private float y;

        public static SimpleStruct Create()
        {
            var obj = new SimpleStruct();
            
            obj.a = 12;
            obj.x = "my string";
            obj.y = 100.999f;

            return obj;
        }
    }

    internal struct ComplexStruct
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

            return obj;
        }
    }
    
    internal class SimpleObject
    {
    }
}