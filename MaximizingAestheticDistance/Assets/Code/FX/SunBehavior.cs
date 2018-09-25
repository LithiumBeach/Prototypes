using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    /// <summary>
    /// rotates about PARENT's X-Z plane. LOCAL up vector is the normal to it's orbit about PARENT. this is important and non-obvious.
    /// </summary>
    public class SunBehavior : MonoBehaviour
    {
        public Gradient m_ColorOverDay;

        //[Required]
       public Light m_DirectionalLight = null;

        [HideInInspector]
        public float m_TimeScale = 1.0f;

        [Tooltip("how fast will the sun rotate at timescale=1?")]
        [Range(0f, 64f)]
        public float m_BaseOrbitSpeed = 1f;

        [Tooltip("orbit about this transform. if you're making a sun, for instance, you would want this to orbit about it'")]
        //[Sirenix.OdinInspector.OnInspectorGUI("OnInspectorGUI_m_Parent")]
        public Transform m_Parent = null;
        private void OnInspectorGUI_m_Parent()
        {
            if (m_Parent == null)
                m_Parent = PlayerManager.Instance.m_Player;
        }

        //let unity handle the rotation to simplify our code.
        private Transform m_LocalParent;
        //need an axis to rotate about, it will be our local up.
        private Vector3 m_RotationAxis;

        private void Start()
        {
            //this should always do nothing, but just in case something goes wrong.
            OnInspectorGUI_m_Parent();

            //store our up vector to orbit m_Parent about.
            m_RotationAxis = transform.up;

            //create local parent to follow the position of m_Parent
            m_LocalParent = new GameObject().GetComponent<Transform>();
            m_LocalParent.localPosition = Vector3.zero;
            m_LocalParent.name = transform.name + "_root";

            //finally, set our parent to the local parent, which we will rotate to take advantage of local space to rotate this sunbehavior
            transform.SetParent(m_LocalParent);
        }

        private void Update()
        {
            //only update position of parent
            m_LocalParent.position = m_Parent.position;
            m_LocalParent.transform.Rotate(m_RotationAxis, m_TimeScale * m_BaseOrbitSpeed * Time.deltaTime);

            if (m_DirectionalLight != null)
            {
                //maintain the directional light facing the world origin. it should be childed to this sun.
                m_DirectionalLight.transform.LookAt(Vector3.zero, Vector3.up);
            }
        }

    }
}