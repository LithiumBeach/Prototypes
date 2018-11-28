using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    public class LeverController : MonoBehaviour, IInteractAxisHandler
    {
        //rotations defined along local x axis of m_StickT
        public Transform m_StickT;
        private float m_MinRotation = -80;
        private float m_MaxRotation = 80;
        private float m_T = .5f;

        private float m_StickTSpeed = 0.5f;

        private void Awake()
        {
            //t starts at what the t would be given this x rotation.
            m_T = 0.5f;//Mathf.InverseLerp(m_MinRotation, m_MaxRotation, m_StickT.rotation.eulerAngles.x);
            m_T = Mathf.Clamp01(m_T);
            UpdateStickRotation(m_T);
        }

        public void HandlePositive()
        {
            m_T += m_StickTSpeed * Time.deltaTime;
            m_T = Mathf.Clamp01(m_T);
            UpdateStickRotation(m_T);
            SceneManager.Instance.CurrentDayNightCycleManager.SetTimescale(m_T);
        }

        public void HandleNegative()
        {
            m_T -= m_StickTSpeed * Time.deltaTime;
            m_T = Mathf.Clamp01(m_T);
            UpdateStickRotation(m_T);
            SceneManager.Instance.CurrentDayNightCycleManager.SetTimescale(m_T);
        }

        private void UpdateStickRotation(float t)
        {
            m_StickT.localRotation = Quaternion.AngleAxis(Mathf.LerpAngle(m_MinRotation, m_MaxRotation, t), m_StickT.right);
        }

        private void Update()
        {
            float interactAxis = Input.GetAxis("InteractAxis");
            if (interactAxis > Mathf.Epsilon)
            {
                HandlePositive();
            }
            else if (interactAxis < -Mathf.Epsilon)
            {
                HandleNegative();
            }
        }
    }
}