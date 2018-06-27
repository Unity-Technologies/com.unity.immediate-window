using System.Linq;
using UnityEditor.ImmediateWindow.UI;

namespace UnityEditor.ImmediateWindow.Services
{
    public class Globals
    {
        public object _0;
        public object _1;
        public object _2;
        public object _3;
        public object _4;
        public object _5 = Inspector.GetAllStaticInstances().ToArray();
        public object _6;
        public object _7 = new SimpleObject();
        public object _8 = SimpleStruct.Create();
        public object _9 = ComplexStruct.Create();

        public object _ImmediateWindowReservedGlobal;
    }
}
