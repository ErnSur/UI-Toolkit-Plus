using System;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace QuickEye.UIToolkit.FieldExtensions
{
    public static class BaseFieldExtensions
    {
        public static bool RegisterValueChangedCallback(this IBaseField control,
            EventCallback<ChangeEvent<object>> callback)
        {
            if (!(control is CallbackEventHandler))
                return false;
            return control switch
            {
                INotifyValueChanged<bool> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<int> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<long> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<float> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<string> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Enum> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Object> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Vector2> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Vector2Int> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Vector3> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Vector3Int> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Vector4> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Rect> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<RectInt> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Bounds> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<BoundsInt> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Color> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<Gradient> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                INotifyValueChanged<AnimationCurve> n => n.RegisterValueChangedCallback(ev =>
                    callback?.Invoke(ChangeEvent<object>.GetPooled(ev.previousValue, ev.newValue))),
                _ => false
            };
        }
    }
}