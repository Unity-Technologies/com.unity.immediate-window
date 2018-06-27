using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityEditor.ImmediateWindow.Services
{
    internal class Inspector
    {
        public Inspector()
        {
        }

        // Get all instances of 
        public static IEnumerable<object> GetAllStaticInstances()
        {
            var objects = new List<object>();
            
            AppDomain app = AppDomain.CurrentDomain;
            var ass = app.GetAssemblies()
                .Where(assembly => /*assembly.FullName.ToLower().Contains("unity")*/ assembly.FullName.ToLower().Contains("immediate"));

            foreach (Assembly a in ass)
            {
                //Debug.Log("Assembly: " + a.FullName);
                
                var types = a.GetTypes();
                foreach (Type t in types)
                {
                    if (t.IsInterface) continue;
                    if (t.IsAbstract) continue;
                    if (!t.IsClass) continue;

                    var staticProperties = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                    if (!staticProperties.Any()) continue;

                    var content = t.FullName;
                    foreach (var p in staticProperties)
                        content += "\n    " + p.Name + " -- " + p.FieldType.FullName;
                    
                    //Debug.Log(content);
                    
                    objects.Add(t);
                }
            }

            //Debug.Log("Count: " + objects.Count);
            
            return objects;
        }
    }
}