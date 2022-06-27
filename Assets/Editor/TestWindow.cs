using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TestWindow : EditorWindow
{
    [MenuItem("Test/open")]
    private static void open() => GetWindow<TestWindow>();

    [SerializeField]
    private VisualTreeAsset ass;

    private void CreateGUI()
    {
        rootVisualElement.Add(new TestView().CreateUI());
    }
}
