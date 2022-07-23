using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{ 
    public class Draggable : Manipulator
    {
        private Vector2 _targetStartPos;
        private Vector3 _pointerStartPos;
        private Vector2 _pointerDelta;
        private bool _tookCapture, _isDragging;

        private readonly VisualElement _shadowSpace = new VisualElement();
        private readonly VisualElement _dragHandle;
        private VisualElement DragHandle => _dragHandle ?? target;
        public string TargetClassName { get; set; }
        public float DragStartThreshold { get; set; }

        public Draggable(string targetClassName = null, VisualElement dragHandle = null)
        {
            TargetClassName = targetClassName;
            _dragHandle = dragHandle;
            DragStartThreshold = dragHandle == null ? 5 : 1;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            DragHandle.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            DragHandle.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            DragHandle.RegisterCallback<PointerUpEvent>(PointerUpHandler);
            DragHandle.RegisterCallback<MouseUpEvent>(MouseUpHandler);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            DragHandle.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            DragHandle.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
            DragHandle.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
            DragHandle.UnregisterCallback<MouseUpEvent>(MouseUpHandler);
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            CacheDragStartData(evt);
            ToggleShadowSpace(true);
            SwitchPositionSpace(true);
            DragHandle.CapturePointer(evt.pointerId);
            _tookCapture = true;
        }

        private void CacheDragStartData(PointerDownEvent evt)
        {
            _pointerStartPos = evt.position;
            _targetStartPos = target.layout.position;
            _pointerDelta = Vector2.zero;
        }

        private void PointerMoveHandler(PointerMoveEvent evt)
        {
            if (!target.HasPointerCapture(evt.pointerId))
                return;
            _pointerDelta = evt.position - _pointerStartPos;
            target.transform.position = GetNewTargetPosFromCursor();
        }
        
        private Vector2 GetNewTargetPosFromCursor()
        {
            var translateBackToStartPos = _targetStartPos;
            return _pointerDelta + translateBackToStartPos;
        }

        private void PointerUpHandler(PointerUpEvent evt)
        {
            OnPointerUp(evt, evt.pointerId);
        }

        private void MouseUpHandler(MouseUpEvent evt)
        {
            OnPointerUp(evt, PointerId.mousePointerId);
        }

        private void OnPointerUp(EventBase evt, int pointerId)
        {
            if (_tookCapture)
            {
                _tookCapture = false;
                SwitchPositionSpace(false);
                ToggleShadowSpace(false);
                target.ReleasePointer(pointerId);
                evt.StopImmediatePropagation();
            }
        }

        private void SwitchPositionSpace(bool dragStart)
        {
            if (dragStart)
            {
                target.BringToFront();
                target.style.position = Position.Absolute;
                target.transform.position = GetNewTargetPosFromCursor();
            }
            else
            {
                target.style.position = Position.Relative;
                target.transform.position =
                    target.layout.position - _shadowSpace.layout.position;
                target.transform.position = Vector3.zero;
                _shadowSpace.parent.Add(target);
                target.PlaceBehind(_shadowSpace);
            }
        }

        private void ToggleShadowSpace(bool enabled)
        {
            if (enabled)
            {
                _shadowSpace.style.width = target.layout.width;
                _shadowSpace.style.height = target.layout.height;
                target.parent.Add(_shadowSpace);
                _shadowSpace.PlaceBehind(target);
            }
            else
            {
                _shadowSpace.RemoveFromHierarchy();
            }
        }
    }
}