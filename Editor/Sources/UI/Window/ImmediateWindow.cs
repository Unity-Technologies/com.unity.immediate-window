using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.ImmediateWindow.Services;

namespace UnityEditor.ImmediateWindow.UI
{
    public class ImmediateWindow : EditorWindow
    {
        public const string PackagePath = "Packages/com.unity.immediate-window/";
        public const string ResourcesPath = PackagePath + "Editor/Resources/";
        private const string TemplatePath = ResourcesPath + "Templates/ImmediateWindow.uxml";
        private const string DarkStylePath = ResourcesPath + "Styles/Main_Dark.uss";
        private const string LightStylePath = ResourcesPath + "Styles/Main_Light.uss";

        public static ImmediateWindow CurrentWindow;
        
        internal Evaluator Evaluator;
        internal State State;

        public void OnEnable()
        {
            CurrentWindow = this;
          
            SetupAnalytics();
            
            Evaluator.Init(ref Evaluator);
            State.Init(ref State);

            string path = EditorGUIUtility.isProSkin ? DarkStylePath : LightStylePath;
            rootVisualElement.styleSheets.Add(EditorGUIUtility.Load(path) as StyleSheet);

            var windowResource = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplatePath);
            if (windowResource != null)
            {
                var root = windowResource.CloneTree();
                rootVisualElement.Add(root);
                root.StretchToParentSize();
            }
        }
        
        private void SetupAnalytics()
        {
            int maxEventsPerHour = 100;
            int maxNumberOfElementInStruct = 100;
            string vendorKey = "unity.immediate-window";

            EditorAnalytics.RegisterEventWithLimit("evaluatecode", maxEventsPerHour, maxNumberOfElementInStruct, vendorKey);
        }

        public void SetSideViewVisibility(bool visibility)
        {
            UIUtils.SetElementDisplay(SideView, visibility);
        }
        
        internal VisualElement Content { get { return this.rootVisualElement.Q<VisualElement>("immediateWindow"); } }
        internal Context Context { get { return this.rootVisualElement.Q<Context>("context"); } }
        internal VisualElement SideView { get { return this.rootVisualElement.Q<VisualElement>("sideview"); } }
        internal Console Console { get { return this.rootVisualElement.Q<Console>("console"); } }
        
        [MenuItem("Window/Analysis/Immediate Window")]
        public static void ShowPackageManagerWindow()
        {
            var window = GetWindow<ImmediateWindow>(false, "Immediate", true);
            window.minSize = new Vector2(700, 250);
            window.Show();
        }
    }
}
