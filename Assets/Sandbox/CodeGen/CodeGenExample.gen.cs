//
// AUTO GENERATED BY TOOL
//
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

namespace SampleAsmdef
{
    partial class CodeGenExample
    {
        private Label title;
        private VisualElement menu;
        private Button confirmButton;
    
        private void AssignQueryResults(VisualElement root)
        {
            title = root.Q<Label>("title");
            menu = root.Q<VisualElement>("menu");
            confirmButton = root.Q<Button>("confirm-button");
        }
    }
}