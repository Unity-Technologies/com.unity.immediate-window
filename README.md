# Unity Immediate Window

An immediate window we can all believe in!

Original Author and Supreme Lord Of Absoluteness Without Hesitation And Second Guessing: Mathieu Rivest

[Documentation](Documentation/com.unity.immediate-window.md)

# TODO

###### MVP

1. Quick Inspector
1. Assembly
1. Responsive UI (not fixed console size)

###### Tasks
```
- Assembly references (Using namespace)
    + To make typing quicker
    * Show a list of last "usings" (assembly list with a 'using' checkbox)
- Object Inspector
    + To show and inspect values
    * Add pagination for all (...)
    * Editing
        * Allow inline edit of values (eg: click on string and change it right there)
- Save Settings
    + Better experience
    * Save List of 'using'
    * Save typing history up to xxx
- Multiline
    + Refine a method
    * Show line numbers for easier debugging
- Typing history
    + Better experience
    * History list
        * with click to paste, and double-click to call
- Autocomplete
    + Better experience
- Codebase Heat Map
    + Easier and intuitive code exploration
- QA
    * Freeze-proof (while(true) {} shouldn't freeze unity/computer)
```

# Internal

Useful code snippets for testing

```
#r "/Users/mathieur/organizations/unity/projects/hackweek/immediate/projects/ImmediateWindow/Library/ScriptAssemblies/Unity.ImmediateWindow.Editor.dll"

UnityEditor.ImmediateWindow.UI.TestClass.Test("asdsa")
using UnityEditor.ImmediateWindow.UI; TestClass.Test("asdsa")

public class A {public int X {get;set;} public string Y {get {return "yes";}}}; new A()

-- Multiline Output --
class A
{
 public int X = 12;
 public string Name = "Mat";
}
var x = new A();
x

-- Using Namespace --
GameObject.Find("Main Camera")
```
