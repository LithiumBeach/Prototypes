using UnityEngine;
using UnityEngine.Events;

namespace pe
{
    [System.Serializable]
    public class LaserEnergy
    {
        public enum EType
        {
            stub
        }
        public EType m_EType;

        public float m_Energy;
    }


    public class PlayerShip : MonoBehaviour
    {
        [Header("Component Refs")]
        public Camera m_Camera;

        [Header("Warp")]
        public float m_WarpTime;
        public AnimationCurve m_WarpDriveCurve;

        private Vector3 m_WarpStartPos;
        private Vector3 m_WarpEndPos;
        private bool b_IsWarping=false;
        private float m_WarpT;

        public UnityEvent OnWarpFinished;

        private void Awake()
        {
            OnWarpFinished = new UnityEvent();
        }

        public void WarpTo(Vector3 _pos, UnityAction _OnWarpFinished=null)
        {
            if (_OnWarpFinished != null)
            {
                OnWarpFinished.AddListener(_OnWarpFinished);
            }

            m_WarpStartPos = transform.position;
            m_WarpEndPos = _pos;

            m_WarpT = 0;
            b_IsWarping = true;
        }

        private void Update()
        {
            if (b_IsWarping)
            {
                m_WarpT += Time.deltaTime;
                float t = m_WarpT / m_WarpTime;
                transform.position = Vector3.Lerp(m_WarpStartPos, m_WarpEndPos, m_WarpDriveCurve.Evaluate(t));

                if (t >= 1f)
                {
                    b_IsWarping = false;

                    //invoke callbacks
                    if (OnWarpFinished != null)
                    {
                        OnWarpFinished.Invoke();

                        //this is a consumable command
                        OnWarpFinished.RemoveAllListeners();
                    }
                }
            }
        }
    }
}
