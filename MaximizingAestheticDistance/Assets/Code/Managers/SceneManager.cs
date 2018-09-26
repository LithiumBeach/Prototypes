using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    public class SceneManager : SingletonBehavior<SceneManager>
    {
        private DayNightCycleManager[] m_DayNightCycleManagers;
        public DayNightCycleManager[] DayNightCycleManagers
        {
            get{ return m_DayNightCycleManagers; }
        }

        protected override void OnAwake()
        {
            m_DayNightCycleManagers = GameObject.FindObjectsOfType<DayNightCycleManager>();
        }
    }
}