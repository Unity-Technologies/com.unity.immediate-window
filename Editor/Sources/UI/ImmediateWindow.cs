using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace UnityEditor.PackageManager.UI
{
    internal class ImmediateWindow : EditorWindow
    {
        public const string PackagePath = "Packages/com.unity.immediate-window/";
        public const string ResourcesPath = PackagePath + "Editor/Resources/";
        private const string TemplatePath = ResourcesPath + "Templates/ImmediateWindow.uxml";
        private const string DarkStylePath = ResourcesPath + "Styles/Main_Dark.uss";
        private const string LightStylePath = ResourcesPath + "Styles/Main_Light.uss";

        private const double targetVersionNumber = 2018.3;

        async public void OnEnable()
        {
          int result = await CSharpScript.EvaluateAsync<int>("1 + 2");
          Debug.Log("Result: " + result);
          /*
            //PackageCollection.InitInstance(ref Collection);

            this.GetRootVisualContainer().AddStyleSheetPath(EditorGUIUtility.isProSkin ? DarkStylePath : LightStylePath);

            var windowResource = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplatePath);
            if (windowResource != null)
            {
                var template = windowResource.CloneTree(null);
                this.GetRootVisualContainer().Add(template);
                template.StretchToParentSize();

                PackageList.OnSelected += OnPackageSelected;
                PackageList.OnLoaded += OnPackagesLoaded;
                PackageList.OnFocusChange += OnListFocusChange;
                
                PackageManagerToolbar.SearchToolbar.OnSearchChange += OnSearchChange;
                PackageManagerToolbar.SearchToolbar.OnFocusChange += OnToolbarFocusChange;

                // Disable filter while fetching first results
                if (!PackageCollection.Instance.LatestListPackages.Any())
                    PackageManagerToolbar.SetEnabled(false);
                else
                    PackageList.SelectLastSelection();
            }
            */
        }

        public void OnDisable()
        {
        }
        
        public void OnDestroy()
        {
        }

        //internal Alert ErrorBanner { get { return this.GetRootVisualContainer().Q<Alert>("errorBanner"); } }
        
        [MenuItem("Window/Debug/Immediate Window", priority = 1500)]
        internal static void ShowPackageManagerWindow()
        {
            var window = GetWindow<ImmediateWindow>(false, "Immediate", true);
            window.minSize = new Vector2(700, 250);
            window.Show();
        }
    }
}
