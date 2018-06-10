using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace metaballs
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class MetaballsManager : MonoBehaviour
    {
        public Rigidbody[] m_Spheres;
        public Material material;

        private Camera m_Camera;

        // Postprocess the image
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

            material.SetFloat("_FOV", m_Camera.fieldOfView);
            material.SetInt("_MBCount", positionArray.Count);

            material.SetVectorArray("_MBPositions", positionArray);
            material.SetFloatArray("_MBRadii", radiiArray);
            //material.SetFloat("_ScreenResolutionX", Screen.currentResolution.width);
            //material.SetFloat("_ScreenResolutionY", Screen.currentResolution.height);
            material.SetVector("_CameraPos", m_Camera.transform.position);
            material.SetVector("_CameraDir", m_Camera.transform.forward);

            Graphics.Blit(source, destination, material);
        }

    }
}