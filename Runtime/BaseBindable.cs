using System.Collections.Generic;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public abstract class BaseBindable<TValueType> : BindableElement, INotifyValueChanged<TValueType>
    {
        private TValueType m_Value;
        
        protected TValueType rawValue
        {
            get => m_Value;
            set => m_Value = value;
        }
    
        public virtual TValueType value
        {
            get => m_Value;
            set
            {
                if (EqualityComparer<TValueType>.Default.Equals(m_Value, value))
                    return;
                if (panel != null)
                {
                    using (ChangeEvent<TValueType> pooled = ChangeEvent<TValueType>.GetPooled(m_Value, value))
                    {
                        pooled.target = (IEventHandler)this;
                        SetValueWithoutNotify(value);
                        SendEvent((EventBase)pooled);
                    }
                }
                else
                    SetValueWithoutNotify(value);
            }
        }
    
        public virtual void SetValueWithoutNotify(TValueType newValue)
        {
            m_Value = newValue;
            MarkDirtyRepaint();
        }
    }
    
    public class BaseBindableTraits<TValueType, TValueUxmlAttributeType> : VisualElement.UxmlTraits
        where TValueUxmlAttributeType : TypedUxmlAttributeDescription<TValueType>, new()
    {
        private TValueUxmlAttributeType m_Value;
    
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((INotifyValueChanged<TValueType>) ve).SetValueWithoutNotify(this.m_Value.GetValueFromBag(bag, cc));
        }
    
        public BaseBindableTraits()
        {
            TValueUxmlAttributeType uxmlAttributeType = new TValueUxmlAttributeType();
            uxmlAttributeType.name = "value";
            this.m_Value = uxmlAttributeType;
        }
    }
}