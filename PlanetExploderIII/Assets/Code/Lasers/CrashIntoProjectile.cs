using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    //behavior to manage a projectile crashing into a planet.
    public class CrashIntoProjectile : MonoBehaviour
    {
        private Vector3 m_CrashPos;
        private float m_Speed;
        private Vector3 m_Dir;

        public delegate void CrashIntoProjectileDelegate(CrashIntoProjectile proj);
        private CrashIntoProjectileDelegate m_OnCrashCallback;

        public void Initialize(Vector3 crashPoint, float speed, CrashIntoProjectileDelegate onCrash)
        {
            m_CrashPos = crashPoint;
            m_Speed = speed;
            m_OnCrashCallback = onCrash;

            m_Dir = (m_CrashPos - transform.position).normalized;
        }

        //make sure we don't pass the crash point.
        private bool b_ReachedDestination = false;
        void Update()
        {
            //stop integrating if we've reached our destination.
            if (b_ReachedDestination) { return; }

            Vector3 newDelta = m_Dir * m_Speed * Time.deltaTime;

            //check if the direction to the crash point is opposite the intended direction after a newDelta step (we will have passed the crash point)
            if (Vector3.Dot((m_CrashPos - (transform.position + newDelta)).normalized, m_Dir) < 0f)
            {
                transform.position = m_CrashPos;
                b_ReachedDestination = true;
                m_OnCrashCallback(this);
            }
            transform.position += newDelta;
        }
    }
}