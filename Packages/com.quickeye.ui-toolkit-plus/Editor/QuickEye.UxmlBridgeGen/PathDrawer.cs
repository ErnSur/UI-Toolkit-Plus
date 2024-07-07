namespace QuickEye.UxmlBridgeGen
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(PathAttribute))]
    internal class PathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            // split rect horizontally into two: field and button
            const int buttonWidth = 70;
            var fieldRect = position;
            fieldRect.width -= buttonWidth+2;
            var buttonRect = position;
            buttonRect.x += fieldRect.width+2;
            buttonRect.width = buttonWidth;
            
            var newValue = EditorGUI.DelayedTextField(fieldRect, label, property.stringValue);
            property.stringValue = GetProjectOrDotRelativePath(newValue);
            if (GUI.Button(buttonRect,"Browse"))
            {
                string newPath = EditorUtility.OpenFolderPanel("Browse for folder", "Assets", "");
                newPath = GetProjectOrDotRelativePath(newPath);
                if (newPath.Length != 0)
                {
                    property.stringValue = newPath;
                }
            }
            EditorGUI.EndProperty();
        }

        private string GetProjectOrDotRelativePath(string path)
        {
            if (path.StartsWith(Application.dataPath))
            {
                return "Assets" + path.Substring(Application.dataPath.Length);
            }
            if (path.StartsWith("Assets") || path.StartsWith(".") || path.Length == 0)
            {
                return path;
            }
            
            // show warning help box
            if (path.Length != 0)
            {
                Debug.LogError($"Path must be inside the project's Assets folder. {path}");
            }
            return null;
        }
        
        private static bool IsPathValid(string path)
        {
            return path.StartsWith("Assets/") || path.StartsWith("./");
        }
    }
}