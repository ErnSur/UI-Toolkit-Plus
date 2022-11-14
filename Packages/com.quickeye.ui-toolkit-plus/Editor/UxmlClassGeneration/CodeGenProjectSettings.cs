using UnityEditor;
using UnityEngine;

namespace QuickEye.UIToolkit.Editor
{
    [FilePath("ProjectSettings/UxmlCodeGenSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class CodeGenProjectSettings : ScriptableSingleton<CodeGenProjectSettings>
    {
        public static CodeGenSettings Instance => instance.settings;
        public CodeGenSettings settings;
        public void Save() => Save(true);

        private void OnEnable()
        {
            if (hideFlags.HasFlag(HideFlags.NotEditable))
            {
                hideFlags ^= HideFlags.NotEditable;
            }
        }
    }
}