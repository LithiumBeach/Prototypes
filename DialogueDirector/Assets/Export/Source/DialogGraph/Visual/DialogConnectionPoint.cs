using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    public enum EConnectionPointType
    { In, Out }
    public class DialogConnectionPoint
    {
        private Rect m_DrawRect;//ONLY for drawing.
        public Rect m_Rect;//a little bigger than the draw rect. used for everything else
        private EConnectionPointType m_Type;
        public DialogNode m_Node;
        public GUIStyle m_Style;
        private GUIStyle m_NormalStyle;
        private GUIStyle m_PressedStyle;
        public GUIContent m_Content = new GUIContent(); //need this for the GUI.Box call... 

        public DialogConnectionPoint(DialogNode node, EConnectionPointType type, GUIStyle normalStyle, GUIStyle pressedStyle)
        {
            m_Node = node;
            m_Type = type;
            m_Style = normalStyle;
            m_NormalStyle = normalStyle;
            m_PressedStyle = pressedStyle;

            m_DrawRect = new Rect(0, 0, 12f, 24f);

            m_Rect.width = m_DrawRect.width   * 3f;
            m_Rect.height = m_DrawRect.height * 2f;
            m_Rect.center = m_DrawRect.center;
        }

        public void Draw()
        {
            m_DrawRect.y = m_Node.m_Rect.y + (m_Node.m_Rect.height * 0.5f) - m_DrawRect.height * 0.5f;

            switch (m_Type)
            {
                case EConnectionPointType.In:
                    m_DrawRect.x = m_Node.m_Rect.x - m_DrawRect.width + 8f;
                    break;

                case EConnectionPointType.Out:
                    m_DrawRect.x = m_Node.m_Rect.x + m_Node.m_Rect.width - 8f;
                    break;
            }

            m_Rect.position = m_DrawRect.position;
            m_Rect.center = m_DrawRect.center;

            GUI.Box(m_DrawRect, m_Content, m_Style);
        }

        public void SetGUIStyleActive(bool isPressed)
        {
            m_Style = isPressed ? m_PressedStyle : m_NormalStyle;
            GUI.changed = true;
        }
    }
}