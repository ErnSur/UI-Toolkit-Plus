using System;
using UnityEditor;
using UnityEngine;

namespace QuickEye.UxmlBridgeGen
{
    internal readonly struct OverrideFieldScope : IDisposable
    {
        private static readonly Color _OverrideMarginColor = new Color(1f / 255f, 153f / 255f, 235f / 255f, 0.75f);

        private readonly bool _hasOverride;
        private readonly float _labelWidth;

        public OverrideFieldScope(bool hasOverride)
        {
            _hasOverride = hasOverride;

            _labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 100;

            if (!_hasOverride)
                return;
            GUI.skin.textField.fontStyle = FontStyle.Bold;
            GUI.skin.GetStyle("ControlLabel").fontStyle = FontStyle.Bold;
        }

        public void Dispose()
        {
            EditorGUIUtility.labelWidth = _labelWidth;
            if (!_hasOverride)
                return;
            GUI.skin.textField.fontStyle = FontStyle.Normal;
            GUI.skin.GetStyle("ControlLabel").fontStyle = FontStyle.Normal;

            if (Event.current.type == EventType.Repaint)
            {
                var r = GUILayoutUtility.GetLastRect();
                r.width = 2;
                r.x = 0;
                EditorGUI.DrawRect(r, _OverrideMarginColor);
            }
        }
    }
}