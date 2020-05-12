using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class FileLocationPanel : VisualElement
    {
        public const string _uxmlPath = "QuickEye/FileLocationPanel/FileLocationPanel";
        public const string _ussPath = _uxmlPath;
        private const string _folderPanelTitle = "Select path";

        private static readonly Stack<string> _previousDirectories = new Stack<string>(new[] { "Assets/" });

        public static IReadOnlyCollection<string> PreviousDirectories => _previousDirectories;

        public event Action AddClicked;
        public event Action CancelClicked;

        [UQuery("add-button")]
        private Button _addButton;

        [UQuery("cancel-button")]
        private Button _cancelButton;

        [UQuery("path-button")]
        private Button _pathButton;

        [UQuery("path-field")]
        private TextField _pathField;

        [UQuery]
        private FileNameField _nameField;

        public string Directory { get => _pathField.value; set => _pathField.value = value; }
        public string FileName { get => _nameField.value; set => _nameField.value = value; }

        public string FullPath => Path.Combine(Directory, FileName);

        public FileLocationPanel()
        {
            LoadVisualTree();
            RegisterEventHandlers();
        }

        private void LoadVisualTree()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(_ussPath));
            var tree = Resources.Load<VisualTreeAsset>(_uxmlPath);
            tree.CloneTree(this);
            this.AssignQueryableMembers(this);
        }

        private void RegisterEventHandlers()
        {
            _addButton.clicked += () => AddClicked?.Invoke();
            _cancelButton.clicked += () => CancelClicked?.Invoke();
            _pathButton.clicked += TrySetPathFromFolderPanel;
        }

        private void TrySetPathFromFolderPanel()
        {
            if (OpenProjectFolderPanel(null, true, out var path))
            {
                _previousDirectories.Push(path);
                _pathField.value = _previousDirectories.Peek();
            }
        }

        private static bool OpenProjectFolderPanel(string directory, bool projectRelative, out string path)
        {
            path = EditorUtility.OpenFolderPanel(_folderPanelTitle, directory ?? Application.dataPath, "Assets");

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