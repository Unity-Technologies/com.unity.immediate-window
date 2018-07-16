namespace UnityEditor.ImmediateWindow.UI
{
    internal class TypeNameView : Span
    {
        public TypeNameView(object obj)
        {
            var type = obj.GetType();
            var parsed = new ParsedAssemblyQualifiedName.ParsedAssemblyQualifiedName(type.AssemblyQualifiedName);
            Init(type.Name, "typename", parsed.CSharpStyleName.Value);
        }
    }
}