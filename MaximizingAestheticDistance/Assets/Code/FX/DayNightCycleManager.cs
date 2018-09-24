using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    /// <summary>
    /// there should be one of these per scene.
    /// manages light direction blending
    /// keeps track of time and manages timescale
    /// </summary>
    public class DayNightCycleManager : MonoBehaviour
    {
        public SunBehavior[] m_Suns;

        [SerializeField]
        [Range(0f, 32f)]
        private float m_TimeScale = 1.0f;
        public float TimeScale
        {
            get { return m_TimeScale; }
            set
            {
                m_TimeScale = value;
                for (int i = 0; i < m_Suns.Length; i++)
                {
                    m_Suns[i].m_TimeScale = value;
                }
            }
        }


        private void Awake()
        {
            
        }

        private void Update()
        {
#if UNITY_EDITOR
            //TODO: temp for editor
            TimeScale = m_TimeScale;
#endif
        }
    }
}