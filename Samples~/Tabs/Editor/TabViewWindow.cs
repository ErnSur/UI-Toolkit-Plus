using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.Tabs
{
    public class TabViewWindow : EditorWindow
    {
        [MenuItem("UIT Plus/Tab View")]
        private static void Open() => GetWindow<TabViewWindow>();
        
        [SerializeField]
        private VisualTreeAsset template;

        private TabView _tabView;

        private void OnEnable()
        {
            titleContent = new GUIContent("Tab View");
        }

        private void CreateGUI()
        {
            template.CloneTree(rootVisualElement);
            _tabView = new TabView(rootVisualElement);
        }
    }
}