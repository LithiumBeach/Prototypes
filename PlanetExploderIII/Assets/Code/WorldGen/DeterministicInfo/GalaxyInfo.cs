using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    [System.Serializable]
    public class GalaxyInfo : DeterministicStructure
    {
        [Tooltip("Lookup name of Galaxy")]
        public string m_GalaxyName;

        [Tooltip("Seeds for all solar systems in this galaxy. Length of this is the number of solar systems.")]
        [OnValueChanged("OnSolarSystemInfosChanged")]
        public SolarSystemInfo[] m_SolarSystemInfos;
        public void OnSolarSystemInfosChanged()
        {
            if (m_SolarSystemInfos != null)
            {
                for (int i = 0; i < m_SolarSystemInfos.Length; i++)
                {
                    if (m_SolarSystemInfos[i].Seed == -1)
                    {
                        m_SolarSystemInfos[i].GenerateSeed();
                    }
                }
            }
            //also check Galaxy seed
            if (Seed == -1)
            {
                GenerateSeed();
            }
        }


    }
}