using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabDragAndDropManipulator : PointerManipulator
    {
        public const string TabDraggedClassName = "tab--dragged";
        public const string TabDraggedOutClassName = "tab--dragged-out";
        public float DragStartThreshold { get; set; } = 5;

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
            target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
            target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
        }

        private float _pointerStartPos;
        private float _targetStartPos;

        private float _lastPointerXPos;

        private bool _isDragging,_tookCapture;

        private VisualElement _tabGroup;
        private List<VisualElement> _allTabs;
        private VisualElement _lastSwappedTab;
        private readonly VisualElement _shadowSpace = new VisualElement();
        private float _pointerDelta;
        
        private void StartDrag()
        {
            ToggleFloatingMode(true);
            _isDragging = true;
        }

        private void InitVars(IPointerEvent evt)
        {
            _tabGroup = target.parent;
            _allTabs = _tabGroup.Children().Where(c => c is Tab).ToList();
            _pointerStartPos = _lastPointerXPos = evt.position.x;
            _targetStartPos = target.layout.position.x;
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            InitVars(evt);
            target.CapturePointer(evt.pointerId);
            _tookCapture = true;
        }

        private void PointerMoveHandler(PointerMoveEvent evt)
        {
            if (!target.HasPointerCapture(evt.pointerId))
                return;
            UpdatePointerDelta(evt);
            if (_isDragging)
            {
                target.transform.position = GetNewTargetPosFromCursor();

                if (TryGetNewHierarchyPosition(out var index))
                    MoveInHierarchy(_shadowSpace, index);
            }
            else if (Mathf.Abs(_pointerDelta) >= DragStartThreshold)
            {
                StartDrag();
            }
        }

        private void PointerUpHandler(PointerUpEvent evt)
        {
            if (_tookCapture && target.HasPointerCapture(evt.pointerId))
            {
                _tookCapture = false;
                target.ReleasePointer(evt.pointerId);
            }
        }

        private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
        {
            if (!_isDragging)
                return;
            ToggleFloatingMode(false);
            _isDragging = false;
        }

        private void UpdatePointerDelta(PointerMoveEvent evt)
        {
            _lastPointerXPos = evt.position.x;
            _pointerDelta = _lastPointerXPos - _pointerStartPos;
        }

        private Vector2 GetNewTargetPosFromCursor()
        {
            return new Vector2
            {
                x = Mathf.Clamp(_pointerDelta + _targetStartPos,
                    0, _tabGroup.layout.width - target.resolvedStyle.width),
                y = target.transform.position.y
            };
        }

        private static void MoveInHierarchy(VisualElement ve, int newIndex)
        {
            ve.parent.Insert(newIndex, ve);
        }


        //todo:
        // if i want to animate snapping into right space:
        // dont just set the transform to zero,
        // instead set it so that element stays in the last dragged position
        // then add animation class
        // then set pos to zero
        private void ToggleFloatingMode(bool enabled)
        {
            if (enabled)
            {
                //target.EnableInClassList(TabDraggedOutClassName, false);
                ToggleShadowSpace(true);
                SwitchPositionSpace(true);
                target.EnableInClassList(TabDraggedClassName, true);
            }
            else
            {
                target.EnableInClassList(TabDraggedClassName, false);
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
            var overlappingTabs = _allTabs.Where(OverlapsTarget);
            var closeTab = FindClosestTab(overlappingTabs);
            if (closeTab == null)
                return false;
            var swap = ShouldSwapPlacesWithTab(closeTab);
            if (swap)
                _lastSwappedTab = closeTab;
            index = swap ? closeTab.parent.IndexOf(closeTab) : -1;
            return swap;
        }

        private bool ShouldSwapPlacesWithTab(VisualElement tab)
        {
            var threshold = tab != _lastSwappedTab ? tab.worldBound.width / 3 : tab.worldBound.width / 1.5f;
            var (myRect, otherRect) = (target.worldBound, tab.worldBound);
            if (otherRect.center.x < myRect.center.x)
                return otherRect.xMax - myRect.xMin > threshold;
            return myRect.xMax - otherRect.xMin > threshold;
        }

        private bool OverlapsTarget(VisualElement tab)
        {
            return tab != target && target.worldBound.Overlaps(tab.worldBound);
        }

        private VisualElement FindClosestTab(IEnumerable<VisualElement> tabs)
        {
            var tabList = tabs.ToList();
            var bestDistanceSq = float.MaxValue;
            VisualElement closest = null;
            foreach (var tab in tabList)
            {
                var displacement =
                    RootSpaceOfTab(tab, tab.parent) - target.transform.position;
                var distanceSq = displacement.sqrMagnitude;
                if (distanceSq < bestDistanceSq)
                {
                    bestDistanceSq = distanceSq;
                    closest = tab;
                }
            }

            return closest;
        }

        private Vector3 RootSpaceOfTab(VisualElement tab, VisualElement tabGroup)
        {
            var tabWorldSpace = tab.parent.LocalToWorld(tab.layout.position);
            return tabGroup.WorldToLocal(tabWorldSpace);
        }
    }
}