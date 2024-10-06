# Custom UI Builder
Tabs instead of foldouts in the inspector

How to inject my logic into the builder?
- Create menu item "UI Builder Plus"
  - it uses reflection to open menu builder and cache its reference in a scriptable object
  - on each assembly reload we inject our elements int this window
- 

# Source generator
- for a class with [UxmlHierarchy(string path)] attribute, generate a partial cs class that creates the same UI hierarchy

```csharp
[UxmlHierarchy("Assets/MyUxmlFile.uxml")]
public partial class UxmlFileName : VisualElement
{
    public UxmlFileName()
    {
        CreateHierarchy();
    }
}
```

```csharp
partial class UxmlFileName : VisualElement
{
    //TODO: how to make field protected?
    private VisualElement title-label;
    private VisualElement playButton;
    private VisualElement settings-button;
    private VisualElement quit-button;

    private void CreateHierarchy()
    {
        title-label = new Label("Title")
        {
            style = { fontSize = 35, unityTextAlign = TextAnchor.UpperCenter, marginTop = 36, marginBottom = 59 }
        };

        play-button = new Button("Play");
        settings-button = new Button("Settings");
        quit-button = new Button("Quit");

        Add(title-label);
        Add(new VisualElement
        {
            play-button,
            settings-button,
            quit-button
        });
    }
}
```

```uxml
<ui:UXML xmlns:ui="UnityEngine.UIElements" xsi="http://www.w3.org/2001/XMLSchema-instance">
    <ui:VisualElement>
        <ui:Label text="Title" display-tooltip-when-elided="true" name="title-label" style="font-size: 35px; -unity-text-align: upper-center; margin-top: 36px; margin-bottom: 59px;" />
        <ui:VisualElement>
            <ui:Button text="Play" display-tooltip-when-elided="true" name="_play-button" />
            <ui:Button text="Settings" display-tooltip-when-elided="true" name="settings-button" />
            <ui:Button text="Quit" display-tooltip-when-elided="true" name="quit-button" />
        </ui:VisualElement>
    </ui:VisualElement>
</ui:UXML>
```

### How to trigger Source generator for UXML files
Add the "uxml" file extension to Project Settings > Editor > C# Project Generation > Additional extensions to include

### Challanges
- how to handle uxml serialized asset references and other custom serialization?
- how to load stylesheets?
  - [LoadUss(identifier:"folder/fileName",LoadMethod.Resources)]
  - [LoadUss(identifier:"Assets/folder/fileName.uss",LoadMethod.AssetDatabase)]
  - [LoadUss(identifier:"labelName",LoadMethod.Addressables)]
  - [UxmlHierarchy("Assets/MyUxmlFile.uxml",defaultUssLoadMethod:LoadMethod.AssetDatabase)]
    - at this point I could also load the uxml and not bother with recreating the hierarchy
- UXML element names do not always correspond to the same class name?
  - On the other hand this package already validated and accounted for it, right?
- UXML attributes are not always the same as the class field names
  - could ii somehow use factory classes for that?

