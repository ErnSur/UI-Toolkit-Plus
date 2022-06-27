using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class ActiveClassManipulator : Manipulator
    {
        public string ClassName { get; }

        public ActiveClassManipulator(string classNamePrefix)
        {
            ClassName = classNamePrefix+"--active";
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
            target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            target.EnableInClassList(ClassName,true);
        }

        private void PointerMoveHandler(PointerMoveEvent evt)
        {
            
        }

        private void PointerUpHandler(PointerUpEvent evt)
        {
            target.EnableInClassList(ClassName,false);

        }

        private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
        {
            target.EnableInClassList(ClassName,false);

        }
        
        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
            target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
        }
    }
}