using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    public class SceneManager : SingletonBehavior<SceneManager>
    {
        [SerializeField]
        private List<DayNightCycleManager> m_DayNightCycleManagers = new List<DayNightCycleManager>();
        public List<DayNightCycleManager> DayNightCycleManagers
        {
            get
            {
                if (m_DayNightCycleManagers == null)
                {
                    m_DayNightCycleManagers = new List<dd.DayNightCycleManager>(FindObjectsOfType<DayNightCycleManager>());
                }
                return m_DayNightCycleManagers;
            }
        }

        private DayNightCycleManager m_CurrentDayNightCycleManager;
        public DayNightCycleManager CurrentDayNightCycleManager
        {
            get { return m_CurrentDayNightCycleManager; }
        }

        protected override void OnAwake()
        {
            m_CurrentDayNightCycleManager = DayNightCycleManagers[0];
            //DayNightCycleManagers = FindObjectsOfType<DayNightCycleManager>();
            //Debug.Assert(DayNightCycleManagers.Length > 0);
        }
    }
}