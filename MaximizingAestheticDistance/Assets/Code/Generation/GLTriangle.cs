using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    //so these generated trees, for the moment, do not need to have a TRS matrix
    //(and thereby a Transform component, and so an instantiated GameObject in the end).
    //they really only need a translation, since generating them is building them up in random
    //rotations already (and 'scale' is handled in their scriptableobjects)
    //since they are, again, at least for now, static props.
    public class GLTriangle //: MonoBehaviour
    {
        public Vector3 m_A;
        public Color m_AColor;
        public Vector3 m_B;
        public Color m_BColor;
        public Vector3 m_C;
        public Color m_CColor;

        public void Draw(Vector3 position)
        {
            // Draw lines
            GL.Begin(GL.TRIANGLES);
            GL.Color(m_AColor);
            GL.Vertex3(m_A.x + position.x, m_A.y + position.y, m_A.z + position.z);
            GL.Color(m_BColor);
            GL.Vertex3(m_B.x + position.x, m_B.y + position.y, m_B.z + position.z);
            GL.Color(m_CColor);
            GL.Vertex3(m_C.x + +position.x, m_C.y + position.y, m_C.z + position.z);
            GL.End();
        }
    }
}