using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEditor.ImmediateWindow.Services {
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypesSafe(this Assembly assembly, bool logErrors = false)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                if (logErrors)
                {
                    Debug.LogWarning("Can't load assembly '" + assembly.GetName() + "'. Problematic types follow.");
                    var message = $"Could not load types for assembly (some types in this assembly might refer to assemblies that are not referenced): {assembly.FullName}\n";
                    foreach (TypeLoadException except in e.LoaderExceptions.Cast<TypeLoadException>())
                        message += $"\n   -- Loader exception for type {except.TypeName}: {except.Message}";
                }

                return new Type[0];
            }
        }
        
        public static IEnumerable<Type> GetLoadableTypes(Assembly assembly) {
            if (assembly == null) throw new ArgumentNullException("assembly");
            
            try {
                return assembly.GetTypes();
            } catch (ReflectionTypeLoadException e) {
                return e.Types.Where(t => t != null);
            }
        }
    }
}