using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEditor.ImmediateWindow.Services;
using Random = System.Random;

namespace UnityEditor.ImmediateWindow.UI
{
    public class TestClass 
    {
        public static int Test(string str)
        {    
            Debug.Log("testing: " + str);

            return (new Random()).Next();
        }    
    }
    
    internal class ImmediateWindow : EditorWindow
    {
        public const string PackagePath = "Packages/com.unity.immediate-window/";
        public const string ResourcesPath = PackagePath + "Editor/Resources/";
        private const string TemplatePath = ResourcesPath + "Templates/ImmediateWindow.uxml";
        private const string DarkStylePath = ResourcesPath + "Styles/Main_Dark.uss";
        private const string LightStylePath = ResourcesPath + "Styles/Main_Light.uss";

        private const double targetVersionNumber = 2018.3;
        
        public Evaluator Evaluator;

        public void OnEnable()
        {
            Evaluator.Init(ref Evaluator);

            this.GetRootVisualContainer().AddStyleSheetPath(EditorGUIUtility.isProSkin ? DarkStylePath : LightStylePath);

            var windowResource = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TemplatePath);
            if (windowResource != null)
            {
                var template = windowResource.CloneTree(null);
                this.GetRootVisualContainer().Add(template);
            }
        }

        internal VisualElement Content { get { return this.GetRootVisualContainer().Q<VisualElement>("immediateWindow"); } }
        
        [MenuItem("Window/Debug/Immediate Window", priority = 1500)]
        [MenuItem("Window/Immediate Window",  priority = 0)]
        internal static void ShowPackageManagerWindow()
        {
            var window = GetWindow<ImmediateWindow>(false, "Immediate", true);
            window.minSize = new Vector2(700, 250);
            window.Show();
        }
    }
}
