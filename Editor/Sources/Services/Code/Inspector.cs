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
            
            var assemblies = GetAllAssemblies();

            foreach (Assembly a in assemblies)
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

        // Get list of all relevant assemblies
        public static IEnumerable<Assembly> GetAllAssemblies()
        {
            AppDomain app = AppDomain.CurrentDomain;
            var assemblies = app.GetAssemblies()
                .Where(assembly => assembly.FullName.ToLower().Contains("unity") /* assembly.FullName.ToLower().Contains("immediate")*/)
                .OrderBy(assembly => assembly.FullName);

            return assemblies;
        }
        
        public static IEnumerable<string> GetAllNamespaces(Assembly assembly)
        {
            return assembly.GetTypes()
                .Select(t => t.Namespace)
                .Distinct()
                .OrderBy(ns => ns);
        }
    }
}
