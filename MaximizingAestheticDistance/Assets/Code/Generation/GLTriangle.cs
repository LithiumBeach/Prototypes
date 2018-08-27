using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    public class GLTriangle : MonoBehaviour
    {
        public Vector3 m_A;
        public Vector3 m_B;
        public Vector3 m_C;

        public void Draw()
        {
            // Draw lines
            GL.Begin(GL.TRIANGLES);
            GL.Color(Color.red);
            GL.Vertex3(m_A.x, m_A.y, m_A.z);
            GL.Color(Color.green);
            GL.Vertex3(m_B.x, m_B.y, m_B.z);
            GL.Color(Color.blue);
            GL.Vertex3(m_C.x, m_C.y, m_C.z);
            GL.End();
        }
    }
}