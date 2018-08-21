using System;
using System.Reflection;
using UnityEngine;

namespace UnityEditor.ImmediateWindow.UI
{
    public struct PropertyInfo
    {
        public FieldInfo Field;
        public System.Reflection.PropertyInfo Property;
        public bool IsPrivate;
        public object Object;
        
        
        public string Label {get {return Field != null ? Field.Name : Property.Name;}}

        public bool GeneratesExceptions
        {
            get
            {
                // These generate exception with a Type. So skip until I know why
                if (Label == "GenericParameterAttributes" || Label == "GenericParameterPosition" ||
                    Label == "DeclaringMethod")
                    return true;

                return false;
            }
        }

        public object Value
        {
            get
            {
                object value = null;

                if (!GeneratesExceptions)
                {
                    try
                    {
                        value = Field != null
                            ? Field.GetValue(Object)
                            : Property.GetValue(Object, null);
                    }
                    catch (Exception error)
                    {
                        var message = "Could not get property value: " + Label + " -- " + error.Message;
                        if (error.StackTrace != null)
                            message += "\n\nStack trace: " + error.StackTrace;
                        if (error.InnerException != null)
                            message += "\n\n\nInner Exception: " + error.InnerException.Message;

                        Debug.LogError(message);
                    }
                }

                return value;
            }
        }
    }
}