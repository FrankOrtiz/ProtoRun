using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace CreativeSpore.SuperTilemapEditor
{

    public partial class TilemapEditor
    {
        private uint m_colorMask = 0xFFFFFFFFu; // [R|G|B|A]
        private bool m_toggleColorMaskAll = true;

        private void OnInspectorGUI_Color()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 120;
            EditorGUILayout.PrefixLabel("Channels");
            EditorGUILayout.BeginHorizontal(GUILayout.Width(180f));
            EditorGUIUtility.labelWidth = 1;
            EditorGUIUtility.fieldWidth = 1;
            m_toggleColorMaskAll = EditorUtils.DoToggleIconButton("", m_toggleColorMaskAll, new GUIContent("All"));
            EditorGUI.BeginChangeCheck();
            bool toggleColorMaskR = EditorUtils.DoToggleIconButton("", !m_toggleColorMaskAll && ((m_colorMask & 0xFFu) != 0), new GUIContent("R"));
            bool toggleColorMaskG = EditorUtils.DoToggleIconButton("", !m_toggleColorMaskAll && ((m_colorMask & 0xFF00u) != 0), new GUIContent("G"));
            bool toggleColorMaskB = EditorUtils.DoToggleIconButton("", !m_toggleColorMaskAll && ((m_colorMask & 0xFF0000u) != 0), new GUIContent("B"));
            bool toggleColorMaskA = EditorUtils.DoToggleIconButton("", !m_toggleColorMaskAll && ((m_colorMask & 0xFF000000u) != 0), new GUIContent("A"));
            m_toggleColorMaskAll &= !EditorGUI.EndChangeCheck();
            EditorGUIUtility.labelWidth = 0;
            EditorGUIUtility.fieldWidth = 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            if (m_toggleColorMaskAll)
            {
                m_colorMask = 0xFFFFFFFFu;
            }
            else
            {
                m_colorMask = 0;
                if (toggleColorMaskR) m_colorMask |= 0xFFu << 24;
                if (toggleColorMaskG) m_colorMask |= 0xFFu << 16;
                if (toggleColorMaskB) m_colorMask |= 0xFFu << 8;
                if (toggleColorMaskA) m_colorMask |= 0xFFu;
            }           
        }
    }
}