using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    public class GLTriangle : MonoBehaviour
    {
        static Material lineMaterial;
        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }

        // Will be called after all regular rendering is done
        public void OnRenderObject()
        {
            CreateLineMaterial();
            // Apply the line material
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.LoadIdentity();
            GL.MultMatrix(transform.localToWorldMatrix);
            GL.MultMatrix(Camera.main.worldToCameraMatrix);

            // Draw lines
            GL.Begin(GL.TRIANGLES);
            GL.Color(Color.blue);
            GL.Vertex3(0, 0, -.5f);
            //GL.End();
            //GL.Begin(GL.TRIANGLES);
            GL.Color(Color.green);
            GL.Vertex3(-.25f, .2f, .15f);
            //GL.End();
            //GL.Begin(GL.TRIANGLES);
            GL.Color(Color.red);
            GL.Vertex3(.5f, 1, -.5f);
            GL.End();
            GL.PopMatrix();
        }
    }
}