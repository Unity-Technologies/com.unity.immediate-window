using System.Linq;
using UnityEditor.ImmediateWindow.TestObjects;
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
        public object _6 = new SecretStruct();
        public object _7;
        public object _8 = SimpleObject.Create();
        public object _9 = ComplexObject.Create();

        public object _ImmediateWindowReservedGlobal;
    }
}
