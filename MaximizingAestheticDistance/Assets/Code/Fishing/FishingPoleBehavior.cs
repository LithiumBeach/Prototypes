using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using util;

namespace dd
{
    public class FishingPoleBehavior : MonoBehaviour
    {
        private enum EState
        {
            HoldingNormally,
            SwingingBack,
            Casting,
            ReelingIn
        }
        private EState m_State;
        [Required]
        public Transform m_RodBase;

        //pre-casting
        private float m_SwingBackTimer;
        public float m_SwingBackTime;
        [Range(-90f, 0f)]
        [Tooltip("final swing back angle of the handle. rest of the rod will update automatically")]
        public float m_SwingBackRootAngleFinal = -30f;

        //[Button("Do the thing", ButtonSizes.Medium)]
        //public void DoTheThing()
        //{
        //    Transform armatureTransform = transform.Find("Armature");

        //    for (int i = 0; i < armatureTransform.childCount-1; i++)
        //    {
        //        HingeJoint iterHinge = armatureTransform.GetChild(i).GetComponent<HingeJoint>();
        //        iterHinge.connectedBody = armatureTransform.GetChild(i+1).GetComponent<Rigidbody>();
        //    }
        //}

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSwingBack();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                m_SwingBackTimer += Time.deltaTime;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                if (m_SwingBackTimer < m_SwingBackTime)
                {
                    OnCancelCast();
                }
                else
                {
                    OnRelease();
                }
            }

            UpdateRod();
        }

        private void UpdateRod()
        {
            switch (m_State)
            {
                case EState.HoldingNormally:
                    m_RodBase.rotation = Quaternion.identity;
                    break;
                case EState.SwingingBack:
                    if ((m_SwingBackTimer / m_SwingBackTime) > 1f)
                    {
                        m_SwingBackTimer = m_SwingBackTime;
                    }

                    //slerp to correct swing back rotation. defined by m_SwingBackRootAngleFinal over m_SwingBackTimer (t).
                    //rotate the base
                    Quaternion rodBaseRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(m_SwingBackRootAngleFinal, 0f, 0f), Mathf.Clamp01(m_SwingBackTimer / m_SwingBackTime));
                    m_RodBase.transform.rotation = rodBaseRotation;
                break;
                case EState.Casting:
                break;
                case EState.ReelingIn:
                    break;
                default:
                break;
            }
        }

        private void OnSwingBack()
        {
            m_SwingBackTimer = 0f;
            m_State = EState.SwingingBack;
        }

        private void OnRelease()
        {
            m_State = EState.Casting;
        }

        private void OnHitWater()
        {
            m_State = EState.ReelingIn;
        }

        private void OnCancelCast()
        {
            m_State = EState.HoldingNormally;
        }
    }
}