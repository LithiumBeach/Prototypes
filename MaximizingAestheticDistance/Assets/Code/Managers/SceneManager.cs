using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    public class SceneManager : SingletonBehavior<SceneManager>
    {
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

        protected override void OnAwake()
        {
            //DayNightCycleManagers = FindObjectsOfType<DayNightCycleManager>();
            //Debug.Assert(DayNightCycleManagers.Length > 0);
        }
    }
}