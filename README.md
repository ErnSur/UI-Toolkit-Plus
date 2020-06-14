# UI Toolkit Plus
[![license](https://img.shields.io/github/license/ToshikiImagawa/Variable-infinite-scroll?style=flat)](https://github.com/ToshikiImagawa/Variable-infinite-scroll/blob/master/LICENSE.md)
![](https://img.shields.io/static/v1?label=unity&message=2019.1%2B&color=blue&style=flat&logo=Unity)
### About
Library mostly made of of extension methods. Created to cut down the boilerplate code, that I wrote in every UI Toolkit related scripts.

### QAttribute Example

##### Standard VisualElement assigment
```csharp
public class ExampleEditorWindow 
{
    ObjectField objField;
    ListView listView;
    Label label;
    
    void OnEnable()
    {
      objField = rootVisualElement.Q<ObjectField>("objField");
      listView = rootVisualElement.Q<ListView>("listView");
      label = rootVisualElement.Q<Label>("label");
    }
}
```
##### QAttribute
```csharp
public class ExampleEditorWindow 
{
    [Q("objField")] ObjectField objField;
    [Q("listView")] ListView listView;
    [Q("label")] Label label;
    
    void OnEnable()
    {
      rootVisualElement.AssignQueryResults(this);
    }
}
```
`QAttribute` marks the class member as a query target, and the `AssignQueryResults` extension method assigns appropriate UQuery results to those members.
