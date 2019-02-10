using System.Collections.Generic;
using util;
using UnityEngine;

namespace lb
{
    public class EconomyUpdateManager : SingletonBehavior<EconomyUpdateManager>
    {//<int=Monobehavior.GetInstanceId(), gameobject>
        System.Collections.Generic.Dictionary<int, IEconomyUpdate> m_Updaters;

        protected override void OnAwake()
        {
        }

        protected override void OnUpdate(float dt)
        {
            IEnumerator<KeyValuePair <int, IEconomyUpdate>> enumerator = m_Updaters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.Update(Time.deltaTime);
            }
            enumerator.Reset();
        }

        internal void AddUpdater(IEconomyUpdate updater)
        {
            if (m_Updaters == null)
            {
                m_Updaters = new Dictionary<int, IEconomyUpdate>();
            }
            m_Updaters.Add(m_Updaters.Count, updater);
        }
    }
}