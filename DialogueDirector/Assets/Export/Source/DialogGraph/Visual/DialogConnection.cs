using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace dd
{
    public class DialogConnection
    {
        public DialogConnectionPoint m_InPoint;
        public DialogConnectionPoint m_OutPoint;
        public Action<DialogConnection> OnClickRemoveConnection;

        public DialogConnection(DialogConnectionPoint inPoint, DialogConnectionPoint outPoint, Action<DialogConnection> _OnClickRemoveConnection)
        {
            m_InPoint = inPoint;
            m_OutPoint = outPoint;
            OnClickRemoveConnection = _OnClickRemoveConnection;
        }

        public void Draw()
        {
            Handles.DrawBezier(
                m_InPoint.m_Rect.center,
                m_OutPoint.m_Rect.center,
                m_InPoint.m_Rect.center + Vector2.left * 50f,
                m_OutPoint.m_Rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            Vector3[] curvePoints = Handles.MakeBezierPoints(
                m_InPoint.m_Rect.center,
                m_OutPoint.m_Rect.center,
                m_InPoint.m_Rect.center + Vector2.left * 50f,
                m_OutPoint.m_Rect.center - Vector2.left * 50f, 15);

            #region Draw Arrow on Line
            Vector3 arrowCenterPoint = curvePoints[5];
            Vector3 arrowDirection = (curvePoints[4] - arrowCenterPoint).normalized;
            Vector3 behindArrowPoint = arrowCenterPoint - arrowDirection * 12f;
            Vector3 perpendicular = new Vector3(arrowDirection.y, -arrowDirection.x, 0f);//still normalized

            Handles.DrawLine(arrowCenterPoint, behindArrowPoint + perpendicular * 8f);
            Handles.DrawLine(arrowCenterPoint, behindArrowPoint - perpendicular * 8f);
            #endregion

            //delete on click
            if (Handles.Button((m_InPoint.m_Rect.center + m_OutPoint.m_Rect.center) * 0.5f, Quaternion.identity, 4, 12, Handles.RectangleHandleCap))
            {
                if (OnClickRemoveConnection != null)
                {
                    OnClickRemoveConnection(this);
                }
            }
        }
    }
}