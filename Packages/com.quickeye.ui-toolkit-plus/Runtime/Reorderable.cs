using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    //TODO: If container has flex direction: column, make it work in y axis.
    // TODO: Is PointerCaptureOutEvent needed?
    // make it work with drag handle being different visualelement
    // try to use it in real scenario
    public class Reorderable : Manipulator
    {
        public const string ReorderableClassName = "reorderable";
        public const string DraggedClassName = ReorderableClassName + "--dragged";

        private List<VisualElement> _allReorderable;

        private readonly VisualElement _shadowSpace = new VisualElement();
        private VisualElement _container;
        private VisualElement _lastSwappedElement;
        private VisualElement _dragHandle;

        private bool _isDragging, _tookCapture;
        private float _pointerDelta;
        private float _pointerStartPos;
        private float _targetStartPos;

        public float DragStartThreshold { get; set; }
        private VisualElement DragHandle => _dragHandle ?? target;
        public Reorderable(VisualElement dragHandle = null)
        {
            _dragHandle = dragHandle;
            DragStartThreshold = dragHandle == null ? 5 : 1;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.EnableInClassList(ReorderableClassName, true);
            DragHandle.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            DragHandle.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            DragHandle.RegisterCallback<PointerUpEvent>(PointerUpHandler);
            DragHandle.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
            DragHandle.RegisterCallback<MouseUpEvent>(MouseUpHandler);

            if (target is Button b)
            {
                var clickable = b.clickable;
                b.RemoveManipulator(clickable);
                b.AddManipulator(clickable);
            }
        }
        
        protected override void UnregisterCallbacksFromTarget()
        {
            target.EnableInClassList(ReorderableClassName, false);
            DragHandle.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            DragHandle.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
            DragHandle.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
            DragHandle.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
            DragHandle.UnregisterCallback<MouseUpEvent>(MouseUpHandler);
        }

        private static bool IsReorderable(VisualElement ve)
        {
            return ve.ClassListContains(ReorderableClassName);
        }

        private void StartDrag()
        {
            ToggleDraggingMode(true);
            _isDragging = true;
        }

        private void SetupData(IPointerEvent evt)
        {
            _container = target.parent;
            _allReorderable = _container.Children().Where(IsReorderable).ToList();
            _pointerStartPos = evt.position.x;
            _targetStartPos = target.layout.position.x;
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            SetupData(evt);
            DragHandle.CapturePointer(evt.pointerId);
            _tookCapture = true;
        }

        private void PointerMoveHandler(PointerMoveEvent evt)
        {
            if (!DragHandle.HasPointerCapture(evt.pointerId) || !_tookCapture)
                return;
            UpdatePointerDelta(evt);
            if (_isDragging)
            {
                target.transform.position = GetNewTargetPosFromCursor();

                if (TryGetNewHierarchyPosition(out var newIndex))
                    MoveInHierarchy(_shadowSpace, newIndex);
            }
            else if (Mathf.Abs(_pointerDelta) >= DragStartThreshold)
            {
                StartDrag();
            }
        }

        private void PointerUpHandler(PointerUpEvent evt)
        {
            OnPointerUp(evt,evt.pointerId);
        }

        private void MouseUpHandler(MouseUpEvent evt)
        {
            OnPointerUp(evt, PointerId.mousePointerId);
        }
        
        private void OnPointerUp(EventBase evt, int pointerId)
        {
            if (_tookCapture && DragHandle.HasPointerCapture(pointerId))
            {
                _tookCapture = false;
                DragHandle.ReleasePointer(pointerId);
            }

            if (_isDragging)
            {
                evt.StopImmediatePropagation();
            }
        }
        
        private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
        {
            if (!_isDragging)
                return;
            ToggleDraggingMode(false);
            _isDragging = false;
        }

        private void UpdatePointerDelta(IPointerEvent evt)
        {
            _pointerDelta = evt.position.x - _pointerStartPos;
        }

        private Vector2 GetNewTargetPosFromCursor()
        {
            return new Vector2
            {
                x = Mathf.Clamp(_pointerDelta + _targetStartPos,
                    0, _container.layout.width - target.resolvedStyle.width),
                y = target.transform.position.y
            };
        }

        private static void MoveInHierarchy(VisualElement ve, int newIndex)
        {
            var container = ve.parent;
            var nonReorderableSiblings = container.Children()
                .Select((e, i) => (index: i, element: e))
                .Where(t => !IsReorderable(t.element))
                .Where(t => t.element != ve)
                .ToArray();

            container.Insert(newIndex, ve);
            foreach (var (index, tab) in nonReorderableSiblings)
                container.Insert(index, tab);
        }


        //todo:
        // if i want to animate snapping into right space:
        // dont just set the transform to zero,
        // instead set it so that element stays in the last dragged position
        // then add animation class
        // then set pos to zero
        private void ToggleDraggingMode(bool enabled)
        {
            if (enabled)
            {
                //target.EnableInClassList(TabDraggedOutClassName, false);
                ToggleShadowSpace(true);
                SwitchPositionSpace(true);
                target.EnableInClassList(DraggedClassName, true);
            }
            else
            {
                target.EnableInClassList(DraggedClassName, false);
                SwitchPositionSpace(false);
                //target.EnableInClassList(TabDraggedOutClassName, true);
                target.PlaceBehind(_shadowSpace);
                ToggleShadowSpace(false);
            }
        }

        private void SwitchPositionSpace(bool absolutePosition)
        {
            if (absolutePosition)
            {
                target.BringToFront();
                target.style.position = Position.Absolute;
                target.transform.position = GetNewTargetPosFromCursor();
            }
            else
            {
                target.style.position = Position.Relative;
                target.transform.position = Vector3.zero;
            }
        }

        private void ToggleShadowSpace(bool enabled)
        {
            if (enabled)
            {
                _shadowSpace.style.width = target.layout.width;
                target.parent.Add(_shadowSpace);
                _shadowSpace.PlaceBehind(target);
            }
            else
            {
                _shadowSpace.RemoveFromHierarchy();
            }
        }

        private bool TryGetNewHierarchyPosition(out int index)
        {
            index = -1;
            var overlappingTabs = _allReorderable.Where(OverlapsTarget).ToArray();
            var closestElement = FindClosestElement(overlappingTabs);
            if (closestElement == null)
                return false;
            var swap = ShouldSwapPlacesWith(closestElement);
            if (swap)
                _lastSwappedElement = closestElement;
            index = swap ? closestElement.parent.IndexOf(closestElement) : -1;
            return swap;
        }

        private bool ShouldSwapPlacesWith(VisualElement element)
        {
            var threshold = element != _lastSwappedElement
                ? element.worldBound.width / 3
                : element.worldBound.width / 1.5f;
            var (myRect, otherRect) = (target.worldBound, element.worldBound);
            if (otherRect.center.x < myRect.center.x)
                return otherRect.xMax - myRect.xMin > threshold;
            return myRect.xMax - otherRect.xMin > threshold;
        }

        private bool OverlapsTarget(VisualElement element)
        {
            return element != target && target.worldBound.Overlaps(element.worldBound);
        }

        private VisualElement FindClosestElement(VisualElement[] elements)
        {
            var bestDistanceSq = float.MaxValue;
            VisualElement closest = null;
            foreach (var element in elements)
            {
                var displacement =
                    RootSpaceOfElement(element) - target.transform.position;
                var distanceSq = displacement.sqrMagnitude;
                if (distanceSq < bestDistanceSq)
                {
                    bestDistanceSq = distanceSq;
                    closest = element;
                }
            }

            return closest;
        }

        private static Vector3 RootSpaceOfElement(VisualElement element)
        {
            var tabWorldSpace = element.parent.LocalToWorld(element.layout.position);
            return element.parent.WorldToLocal(tabWorldSpace);
        }
    }
}