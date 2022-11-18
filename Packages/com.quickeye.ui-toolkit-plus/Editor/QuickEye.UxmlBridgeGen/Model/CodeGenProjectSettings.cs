using UnityEditor;
using UnityEngine;

namespace QuickEye.UxmlBridgeGen
{
    [FilePath("ProjectSettings/UxmlCodeGenSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class CodeGenProjectSettings : ScriptableSingleton<CodeGenProjectSettings>
    {
        public static CodeStyleRules CodeStyleRules => instance.codeStyleRules;
        [SerializeField]
        private CodeStyleRules codeStyleRules = new CodeStyleRules();
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