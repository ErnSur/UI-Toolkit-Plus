using UnityEditor;
using UnityEngine;

namespace QuickEye.UxmlBridgeGen
{
    [FilePath("ProjectSettings/UxmlCodeGenSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class CodeGenProjectSettings : ScriptableSingleton<CodeGenProjectSettings>
    {
        public static CodeStyleRules CodeStyleRules => instance.codeStyleRules;
        public static string DefaultGenScriptDirectory => instance.defaultGenScriptDirectory;
        
        [SerializeField]
        internal CodeStyleRules codeStyleRules = new CodeStyleRules();

        [Tooltip("Relative to the project directory. Relative to the UXML file if it starts with './'. If the path is null or invalid, the script will be generated in the same directory as the UXML file.")]
        [SerializeField]
        [Path]
        internal string defaultGenScriptDirectory = "./";
        
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