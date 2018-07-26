using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEditor.ImmediateWindow.Services;
using UnityEditor.Analytics;


namespace UnityEditor.ImmediateWindow.UI
{
    internal class ImmediateWindow : EditorWindow
    {
        public const string PackagePath = "Packages/com.unity.immediate-window/";
        public const string ResourcesPath = PackagePath + "Editor/Resources/";
        private const string TemplatePath = ResourcesPath + "Templates/ImmediateWindow.uxml";
        private const string DarkStylePath = ResourcesPath + "Styles/Main_Dark.uss";
        private const string LightStylePath = ResourcesPath + "Styles/Main_Light.uss";

        public static ImmediateWindow CurrentWindow;
        
        public Evaluator Evaluator;
        public State State;

        public void OnEnable()
        {
            CurrentWindow = this;
          
            SetupAnalytics();
            
            Evaluator.Init(ref Evaluator);
            State.Init(ref State);

            this.GetRootVisualContainer().AddStyleSheetPath(EditorGUIUtility.isProSkin ? DarkStylePath : LightStylePath);

            var windowResource = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplatePath);
            if (windowResource != null)
            {
                var template = windowResource.CloneTree(null);
                this.GetRootVisualContainer().Add(template);
                template.StretchToParentSize();
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
        
        internal VisualElement Content { get { return this.GetRootVisualContainer().Q<VisualElement>("immediateWindow"); } }
        internal Context Context { get { return this.GetRootVisualContainer().Q<Context>("context"); } }
        internal VisualElement SideView { get { return this.GetRootVisualContainer().Q<VisualElement>("sideview"); } }
        public Console Console { get { return this.GetRootVisualContainer().Q<Console>("console"); } }
        
        [MenuItem("Window/Analysis/Immediate Window")]
        internal static void ShowPackageManagerWindow()
        {
            var window = GetWindow<ImmediateWindow>(false, "Immediate", true);
            window.minSize = new Vector2(700, 250);
            window.Show();
        }
    }
}
