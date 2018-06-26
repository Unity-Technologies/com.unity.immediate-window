# Unity Immediate Window

An immediate window we can all believe in!

Original Author and Supreme Lord Of Absoluteness Without Hesitation And Second Guessing: Mathieu Rivest


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
    * Add all properties
        * getters/setters (expandable for value)
        * Arrays
            * With pagination for large arrays)
        * Methods
    * Add pagination for all (...)
- Save Settings
    + Better experience
    * Save List of 'using'
    * Save typing history (up to xxx)
- Multiline
    + Refine a method
    * Show line numbers for easier debugging
- Typing history
    + Better experience
    * Keyboard up/down usage
    * History list
        * with click to paste, and double-click to call
- Autocomplete
    + Better experience
- Codebase Heat Map
    + Easier and intuitive code exploration
- Use cases
    * Can I do RMGUI?
    * Can I do IMGUI?
- In-memory state
    * Allow 'with continuations' statement in console
    * Reset in-memory state in multiline mode
 - Remove Visual Scripting dependency
    * Need to separate roslyn into a separate package and have visual scripting use this also
 - UI
    * Responsive (currently console size is locked)
    * Reduce margin size for expanded properties
        * (currently goes after the property name but should go under like in browsers)
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
```