using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    // TODO: Drag events as UITK events
    internal class Draggable : Manipulator
    {
        public event Action<PointerMoveEvent> Started;
        public event Action Ended;
        public event Action<Vector2> Dragging;

        private Vector3 _pointerStartPos;
        private bool _isDragging;
        public float DragStartThreshold { get; set; }

        public Draggable()
        {
            // Started += _ => { Debug.Log($"Started drag {target.name}"); };
            // Dragging += _ => { Debug.Log($"Dragging drag {target.name}"); };
            // Ended += () => { Debug.Log($"Ended drag {target.name}"); };
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
            target.RegisterCallback<MouseUpEvent>(MouseUpHandler);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
            target.UnregisterCallback<MouseUpEvent>(MouseUpHandler);
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            _isDragging = false;
            _pointerStartPos = evt.position;
            target.CapturePointer(evt.pointerId);
        }

        private void PointerMoveHandler(PointerMoveEvent evt)
        {
            if (!target.HasPointerCapture(evt.pointerId))
                return;
            var pointerDelta = evt.position - _pointerStartPos;
            if (!_isDragging && pointerDelta.magnitude >= DragStartThreshold)
            {
                Started?.Invoke(evt);
                _isDragging = true;
            }

            if (_isDragging)
            {
                Dragging?.Invoke(pointerDelta);
            }
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
            target.ReleasePointer(pointerId);
            evt.StopImmediatePropagation();
            if (_isDragging)
            {
                Ended?.Invoke();
                _isDragging = false;
            }
        }
    }
}