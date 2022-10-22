# UI Toolkit Plus
[![license](https://img.shields.io/github/license/ToshikiImagawa/Variable-infinite-scroll?style=flat)](https://github.com/ToshikiImagawa/Variable-infinite-scroll/blob/master/LICENSE.md)
![](https://img.shields.io/static/v1?label=unity&message=2021.3%2B&color=blue&style=flat&logo=Unity)
[![openupm](https://img.shields.io/npm/v/com.quickeye.ui-toolkit-plus?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.quickeye.ui-toolkit-plus/)

### About
Reusable features for UI Toolkit runtime and editor.


### UXML C# Script generation

You can generate partial C# class from UXML file. 

Given the following UXML:
```xml
<ui:VisualElement>
    <ui:Label name="title" />
</ui:VisualElement>
<ui:VisualElement name="menu">
    <ui:Button name="confirm-button" />
</ui:VisualElement>
```
Tool generates C# script:
```csharp
partial class UXMLFileName
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
```

- [Detailed documentation about code generation](Documentation~/UxmlCodeGeneration.md)  
- [`QAttribute` approach to query assignment](Documentation~/QAttribute.md)

### Style Sheet Exporter
> Open from context menu: __Window/UI Toolkit/Style Sheet Exporter__  
> Available with Unity 2022.1 and newer

Ever wanted to see how Unity's built-in controls are styled?  
With this tool, you can! Export style sheet assets used by Unity Editor to USS files.

![Style Sheet Exporter Window](Documentation~/style-sheet-exporter.png)

### `ReorderableManipulator`

![](Documentation~/reorderable.gif)

### `TabGroup`, `Tab` and `TabDropdown`

![](Documentation~/tabs.gif)
