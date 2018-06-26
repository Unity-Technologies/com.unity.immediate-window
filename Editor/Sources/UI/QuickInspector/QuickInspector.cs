using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UnityEditor.ImmediateWindow.UI
{

    internal class Span : TextElement
    {
        public Span()
        {
            AddToClassList("span");            
        }
        
        public Span(string text) : this()
        {
            this.text = text;
        }
    }
    
    internal class ValueInspector : VisualElement
    {
        public enum State
        {
            Collapsed,
            Expanded
        }

        private State _CurrentState = State.Collapsed;
        public State CurrentState
        {
            get { return _CurrentState; }
            set
            {
                RemoveFromClassList("collapsed");
                RemoveFromClassList("expanded");

                _CurrentState = value;

                if (_CurrentState == State.Collapsed)
                    AddToClassList("collapsed");
                else
                    AddToClassList("expanded");
            }
        }
        
        public ValueInspector()
        {
            AddToClassList("value");
            CurrentState = State.Collapsed;
        }
    }

    internal struct Config
    {
        public static int ShowMaxCollapsedProperties = 10;
        public static int ShowMaxBlockProperties = 500;
    }

    internal class ClassType : ValueInspector
    {
        private object CurrentObject { get; set; }
        private bool ShowValue { get; set; }
        
        private VisualElement LabelRow { get; set; }        
        private CollapsedPropertyGroup CollapsedPropertyGroup { get; set; }
        private ExpandedPropertyGroup ExpandedPropertyGroup { get; set; }

        public ClassType(object obj, bool showValue = true)
        {            
            LabelRow = new VisualElement();
            LabelRow.AddToClassList("row");
            Add(LabelRow);
            LabelRow.RegisterCallback<MouseDownEvent>(OnClick);                

            // Secondary objects are not clickable
            if (showValue)
                LabelRow.Add(new Arrow(Arrow.Direction.Right));

            SetObject(obj, showValue);
        }

        private void OnClick(MouseDownEvent evt)
        {
            ToggleExpand();
        }

        void ToggleExpand()
        {
            CurrentState = CurrentState == State.Collapsed ? State.Expanded : State.Collapsed;

            Refresh();
        }

        public void SetObject(object obj, bool showValue = true)
        {
            CurrentObject = obj;
            ShowValue = showValue;

            var typeLabel = new Span(CurrentObject.GetType().Name);
            typeLabel.AddToClassList("typename");
            typeLabel.RegisterCallback<TooltipEvent>(e => {
                e.tooltip = CurrentObject.GetType().FullName;
                e.StopImmediatePropagation();
            });

            LabelRow.Add(typeLabel);
            
            Refresh();
        }
        
        void Refresh()
        {
            if (CurrentState == State.Collapsed)
            {
                if (ExpandedPropertyGroup != null)
                    Remove(ExpandedPropertyGroup);

                if (ShowValue)
                {
                    CollapsedPropertyGroup = new CollapsedPropertyGroup(CurrentObject);
                    LabelRow.Add(CollapsedPropertyGroup);
                }
            }
            else
            {
                if (CollapsedPropertyGroup != null)
                    LabelRow.Remove(CollapsedPropertyGroup);

                ExpandedPropertyGroup = new ExpandedPropertyGroup(CurrentObject);
                Add(ExpandedPropertyGroup);
            }
        }
    }

    internal class ExpandedPropertyGroup : VisualElement
    {
        public ExpandedPropertyGroup(object obj)
        {
            AddToClassList("expandedPropertyGroup");

            var properties = PropertyUtils.GetPropertyInfo(obj).ToList();
            bool isClipped = properties.Count > Config.ShowMaxBlockProperties;
            properties = properties.Take(Config.ShowMaxBlockProperties).ToList();

            foreach (var property in properties)
            {
                Add(new PropertyValueGroup(property, true));
            }

            if (isClipped)
            {
                var expander = new Span("(...)");
                Add(expander);
            }
        }
    }

    internal struct PropertyInfo
    {
        public FieldInfo Property;
        public bool IsPrivate;
        public object Object;
    }
    
    internal class PropertyUtils
    {
        public static bool ShowPrivate = true;
        
        public static IEnumerable<PropertyInfo> GetPropertyInfo(object obj)
        {
            var properties = new List<PropertyInfo>();

            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;    // Allow private properties
            var publicProperties = obj.GetType().GetFields().ToList();
            var privateProperties = obj.GetType().GetFields(bindingFlags).ToList();
            
            foreach (var prop in publicProperties)
                properties.Add(new PropertyInfo {Property = prop, IsPrivate = false, Object = obj});

            if (ShowPrivate)
            {
                foreach (var prop in privateProperties)
                    properties.Add(new PropertyInfo {Property = prop, IsPrivate = true, Object = obj});
            }

            return properties;
        }
    }
    
    internal class CollapsedPropertyGroup : VisualElement
    {
        public CollapsedPropertyGroup(object obj)
        {
            AddToClassList("collapsedPropertyGroup");
            
            Add(new Span("{"));

            var properties = PropertyUtils.GetPropertyInfo(obj).ToList();
            bool isClipped = properties.Count > Config.ShowMaxCollapsedProperties;
            properties = properties.Take(Config.ShowMaxCollapsedProperties).ToList();

            foreach (var property in properties)
            {
                if (property.Property != properties.First().Property)
                    Add(new Span(", "));

                Add(new PropertyValueGroup(property, false));
            }

            if (isClipped)
            {
                var expander = new Span(", (...)");
                Add(expander);
            }

            Add(new Span("}"));            
        }
    }

    internal class PropertyValueGroup : VisualElement
    {
        public PropertyValueGroup(PropertyInfo property, bool showValue = true)
        {
            AddToClassList("propertyValueGroup");
            if (property.IsPrivate)
            {
                AddToClassList("private");  
                RegisterCallback<TooltipEvent>(e => {
                    e.tooltip = string.Format("Property {0} is private.", property.Property.Name);
                    e.StopImmediatePropagation();
                });
            }
            
            var propertyLabel = new ProperyLabel(property.Property.Name);
            var fieldValue = new ObjectType(property.Property.GetValue(property.Object), showValue);

            Add(propertyLabel);
            Add(fieldValue);
        }
    }
    
    internal class ProperyLabel : VisualElement
    {
        public ProperyLabel(string property)
        {
            AddToClassList("propertyLabelGroup");

            var propertLabel = new Span(property);
            propertLabel.AddToClassList("propertyLabel");
            Add(propertLabel);
            Add(new Span(": "));
        }
    }
    
    /**
     * Displays most generic object (will decide what is the proper display type should be)
     */
    internal class ObjectType : VisualElement
    {
        public ObjectType(object obj, bool showValue = true, bool isRoot = false) : base()
        {
            SetObject(obj, showValue, isRoot);
        }        

        public void SetObject(object obj, bool showValue = true, bool isRoot = false)
        {
            if (obj == null)
            {
                Span empty;
                if (isRoot)
                {
                    empty = new Span("empty result");
                    empty.AddToClassList("empty");
                }
                else
                {
                    empty = new Span("null");
                    empty.AddToClassList("isNull");                    
                }
                
                Add(empty);
                return;
            }

            var type = obj.GetType();

            var isSimpleType = type.IsPrimitive || type == typeof(string);

            if (isSimpleType)
            {
                var label = obj.ToString();
                if (type == typeof(string))
                    label = string.Format("\"{0}\"", label);
                
                var primitiveLabel = new Span(label);
                if (type.IsPrimitive)
                    primitiveLabel.AddToClassList("isNumber");
                else if (type == typeof(string))
                    primitiveLabel.AddToClassList("isString");
                
                Add(primitiveLabel);
            }
            else
            {
                Add(new ClassType(obj, showValue));
            }
        }
    }
        
    internal class QuickInspector : VisualElement
    {
        private object Obj { get; set; }
        
        public QuickInspector() {}

        public QuickInspector(object obj)
        {
            SetObject(obj);
            AddToClassList("quickInspector");
        }

        public void SetObject(object obj)
        {
            try
            {
                /*
                if (obj != null && (int)obj == 1)
                    obj = ComplexStruct.Create();
                */
            
                Obj = obj;
                Add(new ObjectType(obj, true, true));
            }
            catch (Exception e)
            {
                Debug.LogError("Error with quick inspector: " + e.Message + "\n\n" + e.StackTrace);
            }
        }
    }
}
