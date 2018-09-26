using System;
using UnityEngine;

namespace util
{
    public class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
    {
        public Action OnAwakeComplete;

        private static T m_Instance;
        public static T Instance
        {
            get
            {
                //m_Instance = (T)FindObjectOfType(typeof(T));
                m_Instance = FindObjectOfType<T>();
                if (m_Instance == null)
                {
                    Debug.Log("Create me in SingletonBehaviorManager");
                }
                return m_Instance;
            }
        }

        protected SingletonBehavior()
        {

        }

        private void Awake()
        {
            //register self to manager manager?
            //Debug.Log(this.GetType().ToString() + "::OnAwake()");
            OnAwake();
            if (OnAwakeComplete != null)
            {
                OnAwakeComplete();
            }
        }
        protected virtual void OnAwake()
        {
        }

        //TODO: OnTimeSlicedAwake

        private void Update()
        {
            //register self to manager manager?
            OnUpdate(Time.deltaTime);
        }
        protected virtual void OnUpdate(float dt)
        {
        }
    }
}