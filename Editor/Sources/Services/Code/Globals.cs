using System.Collections.Generic;
using System.Linq;
using UnityEditor.ImmediateWindow.TestObjects;
using UnityEditor.ImmediateWindow.UI;
using UnityEngine;

namespace UnityEditor.ImmediateWindow.Services
{
    public class Globals
    {
        public object __0;
        public object __1;
        public object __2;
        public object __3;
        public object __4 = GameObject.Find("Main Camera").transform;
        public object __5 = new Dictionary<string, int> { {"my", 10}, {"yessir", 20} };
        public object __6 = new List<string> {"test", "yes"};
        public object __7 = new SecretStruct();
        public object __8 = SimpleObject.Create();
        public object __9 = ComplexObject.Create();

        public object _ImmediateWindowReservedGlobal;
    }
}
