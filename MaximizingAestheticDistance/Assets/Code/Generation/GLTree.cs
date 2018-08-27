﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    public class GLTree : MonoBehaviour
    {

        static Material lineMaterial;

        private List<GLTriangle> m_Triangles;

        public void Awake()
        {
            m_Triangles = new List<GLTriangle>();
            GenerateTrianglesInSphere(new Vector3(0, 3, 0), 50f, 100);
        }

        private void GenerateTrianglesInSphere(Vector3 position, float r, int numTriangles)
        {
            for (int triangleIndex = 0; triangleIndex < numTriangles; triangleIndex++)
            {
                //Vector3 position = RandomUtility.GetRandomPointInVolumeOfASphere(new Vector3(0, 0, 0), 2f);
                Vector3 randLocalPosition = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(.1f, r);
                m_Triangles.Add(MakeTriangle(randLocalPosition + position, Vector3.forward, Vector3.up, UnityEngine.Random.Range(.6f, 1.2f)));
            }
        }

        public GLTriangle MakeTriangle(Vector3 position, Vector3 forward, Vector3 up, float radius)
        {
            //since backface culling is off, the direction of right (vs left) shouldn't matter.
            GLTriangle newTri = new GLTriangle();
            newTri.m_A = position + up * radius;
            float randAngleB = UnityEngine.Random.Range(15f, 165f);
            newTri.m_B = position + Quaternion.AngleAxis(randAngleB, forward) * (up * radius);
            float randAngleC = UnityEngine.Random.Range(randAngleB, 345f);
            newTri.m_C = position + Quaternion.AngleAxis(randAngleC, forward * Mathf.Rad2Deg) * (up * radius);
            //Vector3 right = Vector3.Cross(forward, up);


            return newTri;
        }


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
            GL.MultMatrix(Camera.main.worldToCameraMatrix);

            //Begin GL Draws

            //GLTriangle tri = new GLTriangle();
            //tri.A = new Vector3(0, 1, 0);
            //tri.B = new Vector3(0, 0, 0);
            //tri.C = new Vector3(1, 0, 0);
            //tri.Draw();

            for (int i = 0; i < m_Triangles.Count; i++)
            {
                m_Triangles[i].Draw();
            }

            //GenerateTrianglesInSphere(new Vector3(0, 3, 0), 4f, 10);

            //End GL Draws

            GL.PopMatrix();
        }
    }
}