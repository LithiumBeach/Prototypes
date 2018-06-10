using patterns;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace pe
{
    public class FileToList : SingletonBehavior<FileToList>
    {
        public TextAsset m_PlanetNamesRaw;
        private string[] m_PlanetNames;
        [HideInInspector]
        public List<string> m_PlanetNamesManaged;

        public TextAsset m_SolarSystemNamesRaw;
        private string[] m_SolarSystemNames;
        [HideInInspector]
        public List<string> m_SolarSystemNamesManaged;

        protected override void OnAwake()
        {
            Initialize();
        }


        // TODO: avoid duplicates!
        public string GetRandomPlanetName(int seed)
        {
            System.Random rng = new System.Random(seed);
            return m_PlanetNamesManaged[rng.Next(0, m_PlanetNamesManaged.Count)];
        }
        // TODO: avoid duplicates!
        public string GetRandomSolarSystemName(int seed)
        {
            System.Random rng = new System.Random(seed);
            return m_SolarSystemNamesManaged[rng.Next(0, m_SolarSystemNamesManaged.Count)];
        }


        public void Initialize()
        {
            m_PlanetNames = Load(m_PlanetNamesRaw, ref m_PlanetNamesManaged);
            Debug.Assert(m_PlanetNames != null);

            m_SolarSystemNames = Load(m_SolarSystemNamesRaw, ref m_SolarSystemNamesManaged);
            Debug.Assert(m_SolarSystemNames != null);
        }

        private string[] Load(TextAsset _asset, ref List<string> namesManaged)
        {
            string[] r = _asset.text.Split('\n', '\r');

            namesManaged = new List<string>(r);

            for (int i = 0; i < namesManaged.Count; i++)
            {
                if (namesManaged[i] == "" || namesManaged[i] == "\n")
                {
                    namesManaged.RemoveAt(i);
                }
            }

            //can't call shuffle: it breaks determinism.
            //namesManaged.Shuffle(12);

            return r;
        }
    }
}
