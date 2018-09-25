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
        [OnValueChanged("Editor_OnSecondsPerCycleChanged")]
        private float m_SecondsPerCycle = 60f;
        private void Editor_OnSecondsPerCycleChanged()
        {
            //stub
        }

        private float m_T;
        //resets every m_SecondsPerCycle seconds
        private float m_CycleTimer;

        private void Awake()
        {
            ResetClock();
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
            for (int i = 0; i < m_Suns.Length; i++)
            {
                m_Suns[i].m_T = m_T;
            }
        }
    }
}