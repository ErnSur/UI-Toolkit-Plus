using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace QuickEye.UIToolkit.Editor
{
    [InitializeOnLoad]
    internal static class PersistentPostHeaderManager
    {
        public static event Action<UnityEditor.Editor> EditorCreated;
        private static readonly Dictionary<UnityEditor.Editor, List<PostHeaderDrawer>> _ActiveDrawers = new();

        static PersistentPostHeaderManager()
        {
            UnityEditor.Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }
        
        public static void RegisterPostHeaderDrawer(PostHeaderDrawer drawer)
        {
            if (_ActiveDrawers.TryGetValue(drawer.Editor, out var drawers))
            {
                drawers.Add(drawer);
                return;
            }

            _ActiveDrawers[drawer.Editor] = new List<PostHeaderDrawer> { drawer };
        }

        private static void OnPostHeaderGUI(UnityEditor.Editor editor)
        {
            if (!_ActiveDrawers.ContainsKey(editor))
            {
                _ActiveDrawers.Add(editor, new List<PostHeaderDrawer>());
                EditorCreated?.Invoke(editor);
            }

            foreach (var drawer in _ActiveDrawers[editor])
            {
                drawer.OnGUI();
            }

            TryClearCache();
        }

        private static void TryClearCache()
        {
            foreach (var kvp in _ActiveDrawers.Where(kvp => kvp.Key == null).ToArray())
            {
                _ActiveDrawers.Remove(kvp.Key);
            }
        }
    }
}