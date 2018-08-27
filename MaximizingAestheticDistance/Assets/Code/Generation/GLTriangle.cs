using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    public class GLTriangle : MonoBehaviour
    {
        public Vector3 m_A;
        public Color m_AColor;
        public Vector3 m_B;
        public Color m_BColor;
        public Vector3 m_C;
        public Color m_CColor;

        public void Draw()
        {
            // Draw lines
            GL.Begin(GL.TRIANGLES);
            GL.Color(m_AColor);
            GL.Vertex3(m_A.x, m_A.y, m_A.z);
            GL.Color(m_BColor);
            GL.Vertex3(m_B.x, m_B.y, m_B.z);
            GL.Color(m_CColor);
            GL.Vertex3(m_C.x, m_C.y, m_C.z);
            GL.End();
        }
    }
}