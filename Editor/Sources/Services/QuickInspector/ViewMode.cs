namespace UnityEditor.ImmediateWindow.UI
{
    public enum ViewMode
    {
        Default,                // Default view (eg: Expandable with arrow for objects)
        
        Value,                  // View shown when object is displayed as a value in a collapsed state (should be the shortest possible view to save space)
        
        Collapsed,            // View shown when object is collapsed
        Expanded            // View shown when object is expanded
    }
}
