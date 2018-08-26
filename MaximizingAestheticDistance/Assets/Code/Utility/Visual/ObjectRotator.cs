using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    [SerializeField]
    public class ObjectRotator : MonoBehaviour
    {
        public Transform m_Object = null;

        public bool b_SetVarsThroughCode = false;

        [HideIf("b_SetVarsThroughCode")]
        public bool b_ManualRotateSpeed = false;

        [MinMaxSlider(0f, 100f, true)]
        [HideIf("b_ManualRotateSpeed")]
        [HideIf("b_SetVarsThroughCode")]
        public Vector2 m_MinMaxRotateSpeed;

        [ShowIf("b_ManualRotateSpeed")]
        [HideIf("b_SetVarsThroughCode")]
        public float m_RotateSpeed;

        [HideIf("b_SetVarsThroughCode")]
        public bool b_ManualRotateAxis = false;
        [ShowIf("b_ManualRotateAxis")]
        [HideIf("b_SetVarsThroughCode")]
        public Vector3 m_RotateAxis;

        void Awake()
        {
            if (m_Object == null)
            {
                m_Object = transform;
            }

            if (b_SetVarsThroughCode)
            {
                //Don't do anything here
                return;
            }

            if (!b_ManualRotateSpeed)
            {
                m_RotateSpeed = UnityEngine.Random.Range(m_MinMaxRotateSpeed.x, m_MinMaxRotateSpeed.y);
            }
            if (!b_ManualRotateAxis)
            {
                m_RotateAxis = UnityEngine.Random.onUnitSphere;
            }
            else
            {
                m_RotateAxis.Normalize();
            }
        }

        private void Update()
        {
            m_Object.Rotate(m_RotateAxis * (m_RotateSpeed * Time.deltaTime));
        }
    }
}