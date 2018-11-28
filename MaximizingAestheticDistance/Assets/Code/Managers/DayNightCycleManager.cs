using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
using System;

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
        [Range(.1f, 300)]
        [Tooltip("real seconds between t=0 and t=1 (full day-night cycle)")]
        private float m_SecondsPerCycle = 60f;

        [SerializeField]
        private float m_MinSecondsPerCycle = 0.1f;
        [SerializeField]
        private float m_MaxSecondsPerCycle = 60.0f;

        /// <param name="normalizedT">will be clamped to 0,1 range. 0 = m_MinSecondsPerCycle, 1 = m_MaxSecondsPerCycle</param>
        public void SetTimescale(float normalizedT)
        {
            m_SecondsPerCycle = Mathf.Lerp(m_MinSecondsPerCycle, m_MaxSecondsPerCycle, Mathf.Clamp01(normalizedT));
        }

        private float m_T;
        public float m_GlobalT { get { return m_T; } }

        //resets every m_SecondsPerCycle seconds
        private float m_CycleTimer;

        private GLTree[] m_Trees;

        private void Awake()
        {

            //@TODO: this is a garbage solution to a garbage problem.
            SceneManager.Instance.DayNightCycleManagers.Add(this);
            ResetClock();
            m_Trees = FindObjectsOfType<GLTree>();
        }

        private void ResetClock()
        {
            m_T = 0f;
            m_CycleTimer = 0f;
        }

        private void Update()
        {
            UpdateT();
        }

        private void UpdateT()
        {
            m_CycleTimer += Time.deltaTime;
            if (m_CycleTimer > m_SecondsPerCycle)
            {
                //loop daynight cycles. t0 == t1.
                ResetClock();
            }
            m_T = Mathf.Lerp(0f, 1f, m_CycleTimer / m_SecondsPerCycle);
            //Debug.Log(m_T);
        }
    }
}