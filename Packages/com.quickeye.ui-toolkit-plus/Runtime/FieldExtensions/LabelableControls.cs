using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.FieldExtensions
{
    public class FoldoutPlus : Foldout, ILabelable
    {
        public string label
        {
            get => text;
            set => text = value;
        }
    }
    
    public class ButtonPlus : Button, ILabelable
    {
        public string label
        {
            get => text;
            set => text = value;
        }
    }
}