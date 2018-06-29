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
        public Rect m_Rect;
        private EConnectionPointType m_Type;
        public DialogNode m_Node;
        public GUIStyle m_Style;
        public Action<DialogConnectionPoint> OnClickConnectionPoint;

        public DialogConnectionPoint(DialogNode node, EConnectionPointType type, GUIStyle style, Action<DialogConnectionPoint> _OnClickConnectionPoint)
        {
            m_Node = node;
            m_Type = type;
            m_Style = style;
            OnClickConnectionPoint = _OnClickConnectionPoint;
            m_Rect = new Rect(0, 0, 10f, 20f);
        }

        public void Draw()
        {
            m_Rect.y = m_Node.m_Rect.y + (m_Node.m_Rect.height * 0.5f) - m_Rect.height * 0.5f;

            switch (m_Type)
            {
                case EConnectionPointType.In:
                    m_Rect.x = m_Node.m_Rect.x - m_Rect.width + 8f;
                    break;

                case EConnectionPointType.Out:
                    m_Rect.x = m_Node.m_Rect.x + m_Node.m_Rect.width - 8f;
                    break;
            }

            if (GUI.Button(m_Rect, "", m_Style))
            {
                if (OnClickConnectionPoint != null)
                {
                    OnClickConnectionPoint(this);
                }
            }
        }
    }
}