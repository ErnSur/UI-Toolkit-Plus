using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class ActiveClassManipulator : Manipulator
    {
        public ActiveClassManipulator(string classNamePrefix)
        {
            ClassName = classNamePrefix + "--active";
        }

        public string ClassName { get; }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
            target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            target.RegisterCallback<PointerEnterEvent>(PointerEnterHandler);
            target.RegisterCallback<PointerLeaveEvent>(PointerLeaveHandler);
        }


        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
            target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            target.UnregisterCallback<PointerEnterEvent>(PointerEnterHandler);
            target.UnregisterCallback<PointerLeaveEvent>(PointerLeaveHandler);
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            target.EnableInClassList(ClassName, true);
        }

        private void PointerUpHandler(PointerUpEvent evt)
        {
            target.EnableInClassList(ClassName, false);
        }

        private void PointerEnterHandler(PointerEnterEvent evt)
        {
            if (target.HasPointerCapture(evt.pointerId))
                target.EnableInClassList(ClassName, true);
        }

        private void PointerLeaveHandler(PointerLeaveEvent evt)
        {
            target.EnableInClassList(ClassName, false);
        }
    }
}