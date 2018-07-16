using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ImmediateWindow.Services;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class TypeRouter
    {
        private static TypeRouter instance = new TypeRouter();
        public static TypeRouter Instance { get { return instance; } }

        
        private List<ITypeView> _TypeViewers;
        private IEnumerable<ITypeView> TypeViewers
        {
            get
            {
                if (_TypeViewers == null)
                {
                    _TypeViewers = new List<ITypeView>();
                    
                    var types = Inspector.GetTypesWithInterface(typeof(ITypeView)).Where(t => t.IsClass && !t.IsAbstract);
                    foreach (var type in types)
                        _TypeViewers.Add((ITypeView)Activator.CreateInstance(type));
                }

                return _TypeViewers;
            }
        }
        
        // Get all types that can provide a view for the given type
        public IEnumerable<ITypeView> GetTypeViewer(Type type)
        {
            var typeviews = new List<ITypeView>();
            foreach (var typeviewer in TypeViewers)
            {
                if (typeviewer.HasView(type))
                    typeviews.Add(typeviewer);
            }
            
            return typeviews;
        }

        public IEnumerable<ITypeView> GetTypeViewer(object obj)
        {
            return GetTypeViewer(obj?.GetType());
        }
    }
}