using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    public class GenTree : MonoBehaviour
    {
        [Required]
        public Transform TriangleTransform;
        [Required]
        public Material Mat;

        private void OnPostRender()
        {
            if (!Mat)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }
            //GL.PushMatrix();
            //Mat.SetPass(0);
            //GL.LoadOrtho();
            //GL.Color(Color.red);
            //GL.Begin(GL.TRIANGLES);
            //GL.Vertex3(TriangleTransform.position.x + 0.25F, TriangleTransform.position.y + 0.1351F, TriangleTransform.position.z + 0.0F);
            //GL.Vertex3(TriangleTransform.position.x + 0.25F, TriangleTransform.position.y + 0.3F, TriangleTransform.position.z + 0.0F);
            //GL.Vertex3(TriangleTransform.position.x + 0.5F, TriangleTransform.position.y + 0.3F, TriangleTransform.position.z + 0.0F);
            //GL.End();
            //GL.Color(Color.yellow);
            //GL.Begin(GL.TRIANGLES);
            ////GL.Vertex3(TriangleTransform.position + new Vector3(0.5F, 0.25F, -1));
            ////GL.Vertex3(TriangleTransform.position + new Vector3(0.5F, 0.1351F, -1));
            ////GL.Vertex3(TriangleTransform.position + new Vector3(0.1F, 0.25F, -1));
            ////GL.End();
            //GL.PopMatrix();

            GL.PushMatrix();
            Mat.SetPass(0);
            GL.LoadIdentity();
            GL.MultMatrix(Camera.main.worldToCameraMatrix);
            GL.Begin(GL.TRIANGLES);
            //GL.Color(Color.blue);
            GL.Vertex3(0, 0, -.5f);
            //GL.End();
            //GL.Begin(GL.TRIANGLES);
            //GL.Color(Color.green);
            GL.Vertex3(-.25f, .2f, .15f);
            //GL.End();
            //GL.Begin(GL.TRIANGLES);
            //GL.Color(Color.red);
            GL.Vertex3(.5f, 1, -.5f);
            GL.End();
            GL.PopMatrix();
        }
    }
}