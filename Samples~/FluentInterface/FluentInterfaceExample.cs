using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Example
{
    public class FluentInterfaceExample : EditorWindow
    {
        [SerializeField]
        private Model model;

        void OnEnable()
        {
            var root = new Column
            {
                new Label { bindingPath = "counter"},
                new Row
                {
                    new Button()
                        .Text("Counter")
                        .Action(() => model.counter++),
                    new Button()
                        .Text("Reset")
                        .Action(() => model.counter = 0),
                }
            };

            model = CreateInstance<Model>();
            root.Bind(new SerializedObject(model));

            rootVisualElement.Add(root);
        }

        [MenuItem("Window/UI Toolkit/" + nameof(FluentInterfaceExample))]
        static void Open() => GetWindow<FluentInterfaceExample>();
    }

    public class Model : ScriptableObject
    {
        public int counter = 4;
    }
}