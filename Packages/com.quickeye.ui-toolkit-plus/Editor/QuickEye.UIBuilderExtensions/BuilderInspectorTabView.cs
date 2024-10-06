namespace QuickEye.UIBuilderExtensions
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    internal class BuilderInspectorTabView : VisualElement
    {
        private readonly VisualElement _inspector;

        private InspectorTab AttributesTab;
        private InspectorTab StyleSheetsTab;
        private InspectorTab StyleTab;
        
        
        public BuilderInspectorTabView(VisualElement inspector)
        {
            _inspector = inspector;
            var styleSheet = Resources.Load<StyleSheet>("builder-inspector");
            _inspector.styleSheets.Add(styleSheet);

            AttributesTab = new InspectorTab("Attributes")
            {
                name = "inspector-attributes-tab", FindContent = FindAttributesFoldout
            };
            StyleSheetsTab = new InspectorTab("Style Sheets")
            {
                name = "inspector-stylesheets-tab", FindContent = FindStyleSheetsFoldout
            };
            StyleTab = new InspectorTab("Style")
            {
                name = "inspector-style-tab", FindContent = FindStylesFoldout
            };

            var tabView = new TabView();
            tabView.activeTabChanged += OnTabChanged;
            tabView.Add(AttributesTab);
            tabView.Add(StyleSheetsTab);
            tabView.Add(StyleTab);
            
            foreach (var tabContent in GetAllTabContents())
            {
                if(tabContent != null)
                    tabContent.ToggleDisplayStyle(false);
            }
            Add(tabView);
            CreateStyleTabs();
        }

        private void CreateStyleTabs()
        {
            var stylesFoldout = FindStylesFoldout();
            if(stylesFoldout == null)
                return;
            var tabView = new TabView();
            tabView.AddToClassList("style-category-tab-view");
            tabView.style.flexShrink = 1;
            tabView.Q(className:TabView.headerContainerClassName).style.flexDirection = FlexDirection.Column;

            var styleFoldouts = stylesFoldout.Query(classes: "unity-builder-inspector__style-category-foldout").Build();
            foreach (var foldout in styleFoldouts)
            {
                var title = foldout.Q("unity-header")?.Q<Toggle>()?.Q<Label>()?.text ?? "Unknown";
                var tab = new InspectorTab(title)
                {
                    name = foldout.name, FindContent = () => foldout
                };
                foldout.ToggleDisplayStyle(false);
                tabView.Add(tab);
            }
            tabView.activeTabChanged += OnTabChanged;


            var container = stylesFoldout.Q("unity-content");
            container.style.flexDirection = FlexDirection.Row;
            container.Insert(0, tabView);
            
            //Add(tabView);
        }

        private void OnTabChanged(Tab oldTab, Tab newTab)
        {
            if(oldTab is InspectorTab oldInspectorTab)
            {
                oldInspectorTab.FindContent()?.ToggleDisplayStyle(false);
            }
            if(newTab is InspectorTab newInspectorTab)
            {
                newInspectorTab.FindContent()?.ToggleDisplayStyle(true);
            }
            Debug.Log($"Tab changed: {oldTab?.name}, {newTab?.name}");
        }
        
        private IEnumerable<VisualElement> GetAllTabContents()
        {
            yield return FindAttributesFoldout();
            yield return FindStyleSheetsFoldout();
            yield return FindStylesFoldout();
        }
        
        private VisualElement FindAttributesFoldout() => _inspector.Q("inspector-attributes-foldout");
        private VisualElement FindStyleSheetsFoldout() => _inspector.Q("inspector-inherited-styles-foldout");
        private VisualElement FindStylesFoldout() => _inspector.Q("inspector-local-styles-foldout");
        
        private sealed class InspectorTab : Tab
        {
            public Func<VisualElement> FindContent { get; set; }
            
            public InspectorTab(string title) : base(title)
            {
            }
        }
    }
}