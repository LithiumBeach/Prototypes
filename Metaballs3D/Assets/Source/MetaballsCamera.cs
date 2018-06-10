using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace metaballs
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class MetaballsCamera : SceneViewFilter
    {
        [Required]
        public Material m_EffectMaterial;

        public Rigidbody[] m_Spheres;

        #region Camera
        private Camera m_Camera;
        private Camera cam
        {
            get
            {
                if (m_Camera == null)
                {
                    m_Camera = GetComponent<Camera>();
                }
                return m_Camera;
            }
        }
        #endregion

        //temp
        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            List<Vector4> positionArray = new List<Vector4>();
            List<float> radiiArray = new List<float>();

            for (int i = 0; i < m_Spheres.Length; i++)
            {
                positionArray.Add(m_Spheres[i].transform.position);
                radiiArray.Add(m_Spheres[i].transform.localScale.x);//temp
            }

            if (m_Camera == null) { m_Camera = GetComponent<Camera>(); }

            m_EffectMaterial.SetFloat("_FOV", m_Camera.fieldOfView);
            m_EffectMaterial.SetInt("_MBCount", positionArray.Count);

            m_EffectMaterial.SetVectorArray("_MBPositions", positionArray);
            m_EffectMaterial.SetFloatArray("_MBRadii", radiiArray);

            m_EffectMaterial.SetInt("MAX_MARCHING_STEPS", 255);
            m_EffectMaterial.SetFloat("MIN_DIST", 0f);
            m_EffectMaterial.SetFloat("MAX_DIST", 100f);
            m_EffectMaterial.SetFloat("EPSILON", 0.0001f);


            m_EffectMaterial.SetMatrix("_CameraInvViewMatrix", cam.cameraToWorldMatrix);
            m_EffectMaterial.SetVector("_CameraWorldPos", cam.transform.position);
            m_EffectMaterial.SetMatrix("_FrustumCorners", Utils.GetFrustumCorners(cam));


            //we can't use Graphics.Blit because we are creating the quad to render our metaballs with GetFrustumCorners(cam).
            //we need a custom Blit function to tell our shader how to interpret this.
            //Graphics.Blit(source, destination, m_EffectMaterial, 0); // use given effect shader as image effect

            CustomGraphicsBlit(source, destination, m_EffectMaterial, 0); // Replace Graphics.Blit with CustomGraphicsBlit

        }

        /// \brief Custom version of Graphics.Blit that encodes frustum corner indices into the input vertices.
        /// 
        /// In a shader you can expect the following frustum corner index information to get passed to the z coordinate:
        /// Top Left vertex:     z=0, u=0, v=0
        /// Top Right vertex:    z=1, u=1, v=0
        /// Bottom Right vertex: z=2, u=1, v=1
        /// Bottom Left vertex:  z=3, u=1, v=0
        /// 
        /// \warning You may need to account for flipped UVs on DirectX machines due to differing UV semantics
        ///          between OpenGL and DirectX.  Use the shader define UNITY_UV_STARTS_AT_TOP to account for this.
        static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNum)
        {
            RenderTexture.active = dest;

            fxMaterial.SetTexture("_MainTex", source);

            GL.PushMatrix();
            GL.LoadOrtho(); // Note: z value of vertices don't make a difference because we are using ortho projection

            fxMaterial.SetPass(passNum);

            GL.Begin(GL.QUADS);

            // Here, GL.MultitexCoord2(0, x, y) assigns the value (x, y) to the TEXCOORD0 slot in the shader.
            // GL.Vertex3(x,y,z) queues up a vertex at position (x, y, z) to be drawn.  Note that we are storing
            // our own custom frustum information in the z coordinate.
            GL.MultiTexCoord2(0, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 3.0f); // BL

            GL.MultiTexCoord2(0, 1.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 2.0f); // BR

            GL.MultiTexCoord2(0, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f); // TR

            GL.MultiTexCoord2(0, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f); // TL

            GL.End();
            GL.PopMatrix();
        }
    }
}