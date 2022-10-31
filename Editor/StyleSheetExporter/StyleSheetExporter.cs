using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UssExporter
{
    internal partial class StyleSheetExporter : EditorWindow
    {
        [MenuItem("Window/UI Toolkit/Style Sheet Exporter")]
        public static void OpenWindow() => GetWindow<StyleSheetExporter>();

        [SerializeField]
        private VisualTreeAsset template;

        [SerializeField]
        private string searchText;

        private string[] _paths;

        private List<TreeViewItemData<string>> _treeRoots;

        private void OnEnable()
        {
            _paths = StyleSheetExporterDatastore.GetStyleSheetPaths();
            _treeRoots = CreateTreeViewItems();
            titleContent = EditorGUIUtility.IconContent("Project");
            titleContent.text = "Style Sheet Exporter";
        }

        private List<TreeViewItemData<string>> CreateTreeViewItems()
        {
            var id = 1;
            var root = new TreeViewItemData<string>(0, "root");
            foreach (var path in _paths)
            {
                id++;
                CreateTreeViewItem(root, ref id, path);
            }

            return root.children as List<TreeViewItemData<string>>;
        }

        private static void CreateTreeViewItem(TreeViewItemData<string> root, ref int id, string path)
        {
            foreach (var pathElement in path.Split('/'))
            {
                var existingItem = root.children.FirstOrDefault(i => i.data == pathElement);
                if (existingItem.id != 0)
                {
                    root = existingItem;
                    continue;
                }

                ((List<TreeViewItemData<string>>)root.children).Add(root =
                    new TreeViewItemData<string>(id++, pathElement));
            }
        }

        private static IEnumerable<TreeViewItemData<string>> CreateSearchResultItems(string searchText, string[] paths)
        {
            int id = 0;
            foreach (var path in paths.Where(p => p.ToLower().Contains(searchText.ToLower())))
            {
                yield return new TreeViewItemData<string>(id++, path);
            }
        }

        private void CreateGUI()
        {
            template.CloneTree(rootVisualElement);
            AssignQueryResults(rootVisualElement);
            treeView.fixedItemHeight = 16;
            treeView.makeItem = () =>
            {
                var root = new VisualElement();
                root.style.flexDirection = FlexDirection.Row;
                root.Add(new VisualElement()
                {
                    name = "item-icon",
                    style = { width = 16, height = 16 }
                });
                var l = new Label();
                l.AddToClassList("item-label");
                root.Add(l);
                return root;
            };
            treeView.bindItem = (element, i) =>
            {
                element.EnableInClassList("item--expanded", treeView.IsExpanded(treeView.GetIdForIndex(i)));
                var label = element.Q<Label>();
                label.text = treeView.GetItemDataForIndex<string>(i);
                var icon = element.Q("item-icon");
                SetIconClass(icon, !Path.HasExtension(label.text));
            };


            treeView.SetRootItems(_treeRoots);
            treeView.itemsChosen += objects =>
            {
                var path = objects.FirstOrDefault() as string;
                if (string.IsNullOrEmpty(path) || !Path.HasExtension(path))
                    return;
                StyleSheetExporterDatastore.ExportToFile(path);
            };

            searchField.RegisterValueChangedCallback(e =>
            {
                searchText = e.newValue;
                var items = string.IsNullOrWhiteSpace(searchText)
                    ? _treeRoots
                    : CreateSearchResultItems(searchText, _paths).ToList();

                treeView.SetRootItems(items);
                treeView.Rebuild();
            });
            searchField.value = searchText;
        }

        private static void SetIconClass(VisualElement icon, bool isFolder)
        {
            icon.ClearClassList();
            var className = isFolder ? "folder-icon" : "uss-icon";
            className += EditorGUIUtility.isProSkin ? "--dark" : "--light";
            icon.AddToClassList(className);
        }
    }
}