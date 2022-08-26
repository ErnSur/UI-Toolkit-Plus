using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor
{
    public partial class FileLocationPanel : VisualElement
    {
        private const string UxmlPath = "QuickEye/FileLocationPanel/FileLocationPanel";
        private const string FolderPanelTitle = "Select path";

        private static readonly Stack<string> _PreviousDirectories = new Stack<string>(new[] { "Assets/" });

        public static IReadOnlyCollection<string> PreviousDirectories => _PreviousDirectories;

        public event Action AddClicked;
        public event Action CancelClicked;

        public string Directory { get => pathField.value; set => pathField.value = value; }
        public string FileName { get => nameField.value; set => nameField.value = value; }

        public string FullPath => Path.Combine(Directory, FileName);

        public FileLocationPanel()
        {
            LoadVisualTree();
            RegisterEventHandlers();
        }

        private void LoadVisualTree()
        {
            var tree = Resources.Load<VisualTreeAsset>(UxmlPath);
            tree.CloneTree(this);
            AssignQueryResults(this);
        }

        private void RegisterEventHandlers()
        {
            addButton.AddAction(() => AddClicked?.Invoke());
            cancelButton.AddAction(() => CancelClicked?.Invoke());
            pathButton.AddAction(TrySetPathFromFolderPanel);
        }

        private void TrySetPathFromFolderPanel()
        {
            if (OpenProjectFolderPanel(null, true, out var path))
            {
                _PreviousDirectories.Push(path);
                pathField.value = _PreviousDirectories.Peek();
            }
        }

        private static bool OpenProjectFolderPanel(string directory, bool projectRelative, out string path)
        {
            path = EditorUtility.OpenFolderPanel(FolderPanelTitle, directory ?? Application.dataPath, "Assets");

            if (string.IsNullOrEmpty(path))
                return false;

            if (!projectRelative)
                return true;

            if (path.StartsWith(Application.dataPath))
            {
                path = $"Assets{path.Substring(Application.dataPath.Length)}";
                return true;
            }

            EditorUtility.DisplayDialog("Bad path",
                $"Default saving folder location MUST starts with:{Application.dataPath}",
                "OK");

            return false;
        }

        public new class UxmlFactory : UxmlFactory<FileLocationPanel, UxmlTraits> { }
    }
}