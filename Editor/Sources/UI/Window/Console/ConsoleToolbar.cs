using UnityEngine.Experimental.UIElements;
using UnityEditor.ImmediateWindow.Services;
using UnityEngine;
 
namespace UnityEditor.ImmediateWindow.UI
{
    internal class ConsoleToolbar : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<ConsoleToolbar> { }
        private readonly VisualElement root;
        private bool MultiLineMode
        {
            get { return State.Instance.MultiLineMode;}
            set { State.Instance.MultiLineMode = value; }
        }
        private bool ShowContext
        {
            get { return State.Instance.ShowContext;}
            set { State.Instance.ShowContext = value; }
        }

        public Console Console { get; set; }
        
        public ConsoleToolbar()
        {
            root = Resources.GetTemplate("ConsoleToolbar.uxml");
            Add(root);
            root.StretchToParentSize();
            
            ClearButton.RegisterCallback<MouseDownEvent>(ClearClick);
            ClearButton.RegisterCallback<MouseUpEvent>(ClearStopClick);
            MultilineButton.RegisterCallback<MouseDownEvent>(ConsoleExpandToggle);
            ContextButton.RegisterCallback<MouseDownEvent>(ContextExpandToggle);
            RunButton.RegisterCallback<MouseDownEvent>(RunClick);
            RunButton.RegisterCallback<MouseUpEvent>(ClearRunClick);
            ResetButton.RegisterCallback<MouseUpEvent>(ResetClick);
            TemplatesDropdown.RegisterCallback<MouseUpEvent>(TemplatesClick);
            PrivateToggle.OnValueChanged(OnPrivateToggle);

            PrivateToggle.value = State.Instance.ShowPrivate;

            OnPrivateToggle(null);
            RefreshConsoleState();
            RefreshContextState();
        }

        private void TemplatesClick(MouseUpEvent evt)
        {
            if (evt.propagationPhase != PropagationPhase.AtTarget)
                return;

            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Simple"), false, obj => CreateTemplate("Simple"), null);
            menu.AddItem(new GUIContent("Function"), false, obj => CreateTemplate("Function"), null);
            menu.AddItem(new GUIContent("Custom Type View"), false, obj => CreateTemplate("Custom Type View"), null);
            menu.AddItem(new GUIContent("Custom Expandable Type View"), false, obj => CreateTemplate("Custom Expandable Type View"), null);
            
            var menuPosition = new Vector2(TemplatesDropdown.layout.xMin, TemplatesDropdown.layout.center.y);
            menuPosition = this.LocalToWorld(menuPosition);
            var menuRect = new Rect(menuPosition, Vector2.zero);
            menu.DropDown(menuRect);
        }

        private void CreateTemplate(string name)
        {
            var code = "";
            if (name == "Simple")
            {
                code = @"using UnityEditor;
using UnityEngine;

public class Test
{
  public void Func()
  {
    Debug.Log(""works!"");
    }
}
var x = new Test();
x.Func();
x
";
            }
            else if (name == "Function")
            {
                code = @"using UnityEditor;
using UnityEngine;

public class Test
{
  public object Func()
  {
    return 123;
  }
}
var x = new Test();
x.Func()
";
            } 
            else if (name == "Custom Type View")
            {
                code = @"using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.Experimental.UIElements;
using UnityEditor.ImmediateWindow.UI;

// NOTE: Currently only works the first time it is ran after any compilation. Need to investigate why.

public class MyType
{
    public string Useful = ""useful property"";
    public string NotUseful = ""not a useful property"";
}

public class MyCustomViewer : ATypeView
{
    public override bool HasView(Type type)
    {
        return type == typeof(MyType);
    }

    public override VisualElement GetView(object obj, ViewContext context)
    {
        var typeview = new Label();
        typeview.text = (obj as MyType).Useful;     // Show only useful properties
        return typeview;
    }
}

// For quick view in the console
var x = new MyType();
x
";
            }
            else if (name == "Custom Expandable Type View")
            {
                code = @"using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.Experimental.UIElements;
using UnityEditor.ImmediateWindow.UI;

// Type view using the standard expandable object system.

// NOTE: Currently only works the first time it is ran after any compilation. Need to investigate why.

public class MyType
{
    public string Useful = ""useful property"";
    public int[] AnArrayProperty = new int[] {1,2,3};
    public string NotUseful = ""not a useful property"";
}

public class MyCustomViewer : ATypeView
{
    public override bool HasView(Type type)
    {
        return type == typeof(MyType);
    }

    public override VisualElement GetView(object obj, ViewContext context)
    {
        // Return the expandable view by default
        if (context.Mode == ViewMode.Default)
            return new ExpandableObject(obj, context);

        var theObj = (obj as MyType);
        if (context.Mode == ViewMode.Collapsed)        // The view to return in the collapsed state of the object (single line)
        {
            // Show only useful properties;
            var content = new VisualElement();
            content.Add(new TypeInspector(theObj.AnArrayProperty, new ViewContext {Mode = ViewMode.Collapsed}));
            content.Add(new Label(theObj.Useful));
            return content;
        }
        if (context.Mode == ViewMode.Expanded)       // The view to return in the expanded state of the object (every line has a property)
            return new ExpandedClassType(obj);;                // Show everything
        if (context.Mode == ViewMode.Value)            // The view to return when only the basic type value is viewed
            return new TypeNameView(obj);
        
        throw new Exception(""Should not get here"");
    }
}

// For quick view in the console
var x = new MyType();
x
";
            }
            
            Console.SetMultilineCode(code);
        }

        private void ResetClick(MouseUpEvent evt)
        {
            Evaluator.Instance.Init();
            Console.ConsoleOutput.ClearLog();
            History.Instance.Clear();        // Only clearing history in case anything went wrong with it as a way to reset to valid state
            ImmediateWindow.CurrentWindow.Context.Reset();
            Console.CurrentCommand = null;
        }

        private void ClearRunClick(MouseUpEvent evt)
        {
            RunButton.RemoveFromClassList("pressed");
            Console.CodeEvaluate();
        }

        private void RunClick(MouseDownEvent evt)
        {
            RunButton.AddToClassList("pressed");
        }

        private void ConsoleExpandToggle(MouseDownEvent evt)
        {
            MultiLineMode = !MultiLineMode;
            RefreshConsoleState();
        }

        private void ContextExpandToggle(MouseDownEvent evt)
        {
            ShowContext = !ShowContext;
            RefreshContextState();
        }

        void RefreshContextState()
        {
            ContextButton.RemoveFromClassList("pressed");
            ImmediateWindow.CurrentWindow.SetSideViewVisibility(ShowContext);
            
            if (ShowContext)
                ContextButton.AddToClassList("pressed");
        }

        void RefreshConsoleState()
        {
            if (MultiLineMode)
                MultilineButton.AddToClassList("pressed");
            else
                MultilineButton.RemoveFromClassList("pressed");
            
            UIUtils.SetElementDisplay(TemplatesDropdown, MultiLineMode);
            
            if (Console != null)
                Console.SetMode(MultiLineMode);
        }

        private void ClearStopClick(MouseUpEvent evt)
        {
            ClearButton.RemoveFromClassList("pressed");
        }

        private void ClearClick(MouseDownEvent evt)
        {
            ClearButton.AddToClassList("pressed");
            Console.ConsoleOutput.ClearLog();
        }

        private void OnPrivateToggle(ChangeEvent<bool> evt)
        {
            PropertyUtils.ShowPrivate = PrivateToggle.value;
            State.Instance.ShowPrivate = PrivateToggle.value;
        }

        private Label TemplatesDropdown {get { return root.Q<Label>("templates"); }}
        private Button ResetButton {get { return root.Q<Button>("reset"); }}
        private Label ClearButton {get { return root.Q<Label>("clear"); }}
        private Label RunButton {get { return root.Q<Label>("run"); }}
        private Label MultilineButton {get { return root.Q<Label>("multiline"); }}
        private Label ContextButton {get { return root.Q<Label>("context"); }}
        private Toggle PrivateToggle {get { return root.Q<Toggle>("viewPrivate"); }}
    }
}