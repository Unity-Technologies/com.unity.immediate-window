using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEditor.ImmediateWindow.Services;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ImmediateWindow : EditorWindow
    {
        public const string PackagePath = "Packages/com.unity.immediate-window/";
        public const string ResourcesPath = PackagePath + "Editor/Resources/";
        private const string TemplatePath = ResourcesPath + "Templates/ImmediateWindow.uxml";
        private const string DarkStylePath = ResourcesPath + "Styles/Main_Dark.uss";
        private const string LightStylePath = ResourcesPath + "Styles/Main_Light.uss";

        private const double targetVersionNumber = 2018.3;
        
        public Evaluator Evaluator;
        public History History;

        public void OnEnable()
        {
            Evaluator.Init(ref Evaluator);
            History.Init(ref History);

            this.GetRootVisualContainer().AddStyleSheetPath(EditorGUIUtility.isProSkin ? DarkStylePath : LightStylePath);

            var windowResource = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplatePath);
            if (windowResource != null)
            {
                var template = windowResource.CloneTree(null);
                this.GetRootVisualContainer().Add(template);
                template.StretchToParentSize();
            }
        }

        internal VisualElement Content { get { return this.GetRootVisualContainer().Q<VisualElement>("immediateWindow"); } }
        
        [MenuItem("Window/Debug/Immediate Window", priority = 1500)]
        internal static void ShowPackageManagerWindow()
        {
            var window = GetWindow<ImmediateWindow>(false, "Immediate", true);
            window.minSize = new Vector2(700, 250);
            window.Show();
        }
    }
}
