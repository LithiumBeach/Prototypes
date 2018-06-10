using Sirenix.OdinInspector;
using UnityEngine;

namespace pe
{
    //[CreateAssetMenu(fileName = "UniverseData", menuName = "PlanetExploder/UniverseData")]
    public class UniverseData : ScriptableObject
    {
        [Tooltip("Top -> Bottom : First -> Last levels")]
        public GalaxyInfoGroup[] Galaxies;

        public int NumGalaxies
        {
            get { return Galaxies != null ? Galaxies.Length : 0; }
        }
    }

    [System.Serializable]
    public class GalaxyInfoGroup
    {
        [MinMaxSlider(50, 100, true)]
        [FoldoutGroup("Galaxy", false)]
        public Vector2 m_MinMaxDistanceBetweenSolarSystems = new Vector2(50, 100);

        [FoldoutGroup("Galaxy")]
        public GalaxyInfo m_GalaxyInfo;
        [FoldoutGroup("Galaxy")]
        public EComposition[] m_Cores;
        [FoldoutGroup("Galaxy")]
        public EComposition[] m_Surfaces;
        [FoldoutGroup("Galaxy")]
        public EComposition[] m_Atmospheres;
    }
}