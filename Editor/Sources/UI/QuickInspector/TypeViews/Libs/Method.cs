using System.Linq;
using System.Reflection;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    public class Method : VisualElement
    {
        public Method(MethodInfo info, object obj)
        {
            Add(new Span(info.ReturnType.Name + " ", "return"));
            Add(new Span(info.Name, "name"));

            var parameters = new Container("parameters");
            parameters.Add(new Span("("));
            
            var parameterInfos = info.GetParameters();
            foreach (var arg in parameterInfos)
            {
                var parameter = new Container("parameter");
                parameter.Add(new Span(arg.ParameterType.Name + " ", "parameterType", arg.ParameterType.FullName));
                parameter.Add(new Span(arg.Name, "parameterName"));

                parameters.Add(parameter);

                if (arg != parameterInfos.Last())
                    parameter.Add(new Span(", "));
            }
                        
            parameters.Add(new Span(")"));

            Add(parameters);
        }
    }
}