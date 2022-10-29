using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    // TODO: README entry
    // TODO: Animatable snap into place
    // TODO: Fix visuals on dragging tab in column tab group: create a shadow drag container that will mimic previous content container size
    public class Reorderable : Manipulator
    {
        public const string ReorderableClassName = "reorderable";
        public const string DraggedClassName = "dragged";
        private Vector2 _targetStartPos;
        private VisualElement _container;
        private VisualElement _lastSwappedElement;
        private List<VisualElement> _allReorderable;
        private readonly VisualElement _dragHandle;
        private readonly VisualElement _shadowSpace = new VisualElement();
        private readonly Draggable _draggable = new Draggable() { DragStartThreshold = 5 };

        private VisualElement[] originalChildOrder;

        public Reorderable(string targetClassName = null, VisualElement dragHandle = null)
        {
            TargetClassName = targetClassName;
            _dragHandle = dragHandle;
            DragStartThreshold = dragHandle == null ? 5 : 1;
            _draggable.Started += OnDragStart;
            _draggable.Dragging += OnDragging;
            _draggable.Ended += OnDragEnd;
        }

        public string TargetClassName { get; }
        public string TargetReorderableClassName => $"{TargetClassName}--{ReorderableClassName}";
        public string TargetDraggedClassName => $"{TargetReorderableClassName}-{DraggedClassName}";

        /// <summary>
        ///     How much does the pointer needs to drag the element before element is picked up.
        /// </summary>
        public float DragStartThreshold
        {
            get => _draggable.DragStartThreshold;
            set => _draggable.DragStartThreshold = value;
        }

        /// <summary>
        ///     When locked dragging only moves element along its parent flex direction axis.
        /// </summary>
        public bool LockDragToAxis { get; set; }

        public (float nextElement, float previousElement) SwapThresholdMod { get; set; } = (1 / 1.5f, 1 / 3f);
        private VisualElement DragHandle => _dragHandle ?? target;

        private bool IsColumnContainer
        {
            get
            {
                var dir = _container.resolvedStyle.flexDirection;
                return dir == FlexDirection.Column || dir == FlexDirection.ColumnReverse;
            }
        }

        public static bool IsReorderable(VisualElement ve) => ve.ClassListContains(ReorderableClassName);
        public static bool IsDragged(VisualElement ve) => ve.ClassListContains(DraggedClassName);

        protected override void RegisterCallbacksOnTarget()
        {
            DragHandle.AddManipulator(_draggable);
            target.EnableInClassList(TargetReorderableClassName, true);
            target.EnableInClassList(ReorderableClassName, true);

            if (target is Button b)
            {
                var clickable = b.clickable;
                b.RemoveManipulator(clickable);
                b.AddManipulator(clickable);
            }
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            DragHandle.RemoveManipulator(_draggable);
            target.EnableInClassList(TargetReorderableClassName, false);
            target.EnableInClassList(ReorderableClassName, false);
        }

        private void OnDragStart(IPointerEvent evt)
        {
            _container = target.parent.contentContainer;
            originalChildOrder = _container.Children().ToArray();
            _allReorderable = _container.Children().Where(IsReorderable).ToList();
            _targetStartPos = target.layout.position;
            ToggleDraggingMode(true);
        }

        private void OnDragging(Vector2 pointerDelta)
        {
            target.transform.position = GetNewTargetPosFromCursor(pointerDelta);

            if (TryGetNewHierarchyPosition(out var newIndex))
                MoveInHierarchy(_shadowSpace, newIndex);
        }

        private void OnDragEnd()
        {
            ToggleDraggingMode(false);
            var newChildOrder = _container.Children();
            if (originalChildOrder.SequenceEqual(newChildOrder))
                return;
            using (var orderChangedEvent = ChildOrderChangedEvent.GetPooled())
            {
                orderChangedEvent.target = target.parent;
                target.parent.SendEvent(orderChangedEvent);
            }
        }

        private Vector2 GetNewTargetPosFromCursor(Vector2 pointerDelta)
        {
            var translateBackToStartPos = _targetStartPos - target.layout.position;

            var newX = pointerDelta.x + translateBackToStartPos.x;
            var newY = pointerDelta.y + translateBackToStartPos.y;
            if (LockDragToAxis)
            {
                newX = Mathf.Clamp(newX,
                    0, _container.layout.width - target.resolvedStyle.width);
                newY = Mathf.Clamp(newY,
                    0, _container.layout.height - target.resolvedStyle.height);
            }

            return new Vector2
            {
                x = !LockDragToAxis ? newX : IsColumnContainer ? target.transform.position.x : newX,
                y = !LockDragToAxis ? newY : IsColumnContainer ? newY : target.transform.position.y
            };
        }

        private static void MoveInHierarchy(VisualElement ve, int newIndex)
        {
            ReorderableUtility.MoveReorderable(newIndex, ve);
        }

        private void ToggleDraggingMode(bool enabled)
        {
            if (enabled)
            {
                ToggleShadowSpace(true);
                SwitchPositionSpace(true);
                target.usageHints = UsageHints.DynamicTransform;
                target.EnableInClassList(TargetDraggedClassName, true);
            }
            else
            {
                target.usageHints = UsageHints.None;
                target.EnableInClassList(TargetDraggedClassName, false);
                SwitchPositionSpace(false);
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
                target.transform.position = GetNewTargetPosFromCursor(Vector2.zero);
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
                _shadowSpace.style.height = target.layout.height;
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
            var closestElement = ReorderableUtility.FindClosestElement(target, overlappingTabs);
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
            var (targetRect, otherRect) = (
                new AxisRectData(IsColumnContainer, target.worldBound),
                new AxisRectData(IsColumnContainer, element.worldBound));

            var threshold = otherRect.WidthOrHeight
                            * (element != _lastSwappedElement
                                ? SwapThresholdMod.previousElement
                                : SwapThresholdMod.nextElement);
            if (otherRect.CenterXOrY < targetRect.CenterXOrY)
                return otherRect.XMaxOrYMax - targetRect.XMinOrYMin > threshold;
            return targetRect.XMaxOrYMax - otherRect.XMinOrYMin > threshold;
        }

        private bool OverlapsTarget(VisualElement element)
        {
            return element != target && target.worldBound.Overlaps(element.worldBound);
        }
    }

    internal readonly struct AxisRectData
    {
        public readonly float WidthOrHeight;
        public readonly float XOrY;
        public float XMinOrYMin => XOrY;
        public readonly float XMaxOrYMax;
        public float CenterXOrY => XOrY + WidthOrHeight / 2f;

        public AxisRectData(bool isVertical, Rect rect)
        {
            WidthOrHeight = isVertical ? rect.height : rect.width;
            XOrY = isVertical ? rect.y : rect.x;
            XMaxOrYMax = isVertical ? rect.yMax : rect.xMax;
        }
    }
}