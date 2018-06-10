using System;
using patterns;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

namespace pe
{
    public class InstanceManager : SingletonBehavior<InstanceManager>
    {
        [BoxGroup("Prefabs")]
        [Required]
        public SolarSystemBehavior m_SolarSystemPrefab = null;
        [BoxGroup("Prefabs")]
        [Required]
        public LineRenderer m_GalaxyOrbitLineRenderer = null;
        [BoxGroup("Prefabs")]
        [Required]
        public LineRenderer m_SolarSystemOrbitLineRenderer = null;
        [BoxGroup("Prefabs")]
        [Required]
        public OrbitalSystemCenter m_WormHolePrefab = null;
        [BoxGroup("Prefabs")]
        [Required]
        public OrbitalSystemCenter m_SunPrefab = null;
        [BoxGroup("Prefabs")]
        [Required]
        public PlanetBehavior m_PlanetPrefab = null;
        [BoxGroup("Prefabs")]
        [Required]
        public AsteroidBelt m_AsteroidBeltPrefab = null;


        [MinMaxSlider(50, 100, true)]
        [FoldoutGroup("Metrics", false)]
        public Vector2 m_MinMaxDistanceBetweenPlanets = new Vector2(4, 8);


        private SolarSystemBehavior[] m_InstancedSolarSystems;
        public SolarSystemBehavior[] InstancedSolarSystems { get { return m_InstancedSolarSystems; } }

        public SolarSystemBehavior FocusSolarSystem { get { return InfoManager.Instance.m_FocusSolarSystemIndex != -1 ? m_InstancedSolarSystems[InfoManager.Instance.m_FocusSolarSystemIndex] : null; } }
        public PlanetBehavior FocusPlanet { get { return InfoManager.Instance.m_FocusPlanetIndex != -1 ? FocusSolarSystem.Planets[InfoManager.Instance.m_FocusPlanetIndex] : null; } }

        private OrbitalSystemCenter m_InstancedWormhole;


        //called after InfoManager Awake, in script execution order settings.
        protected override void OnAwake()
        {
            //assert focus galaxy index
            Debug.Assert(InfoManager.Instance.m_FocusGalaxyIndex < InfoManager.Instance.m_UniverseData.Galaxies.Length &&
                         InfoManager.Instance.m_FocusGalaxyIndex >= 0);

            //we'll first try instantiating the entire galaxy, and all solar systems, THEN optimize.
            InstantiateGalaxy(InfoManager.Instance.FocusGalaxy);
        }

        private void InstantiateGalaxy(GalaxyInfoGroup galaxyInfo)
        {
            m_InstancedWormhole = Instantiate(m_WormHolePrefab).GetComponent<OrbitalSystemCenter>();

            //all galaxies spawn at V3.zero, as there is only ever one loaded at a time.
            m_InstancedWormhole.transform.position = Vector3.zero;

            int numSolarSystems = galaxyInfo.m_GalaxyInfo.m_SolarSystemInfos.Length;
            m_InstancedSolarSystems = new SolarSystemBehavior[numSolarSystems];

            System.Random l_RNG = new System.Random(galaxyInfo.m_GalaxyInfo.Seed);

            float iterDistance = 0f;//distance from previous orbit

            //simple for all solar system infos in galaxy info.
            // TODO: only instance SOME solar systems, not all of them. Or instantiate a visual representation that looks far away, but very optimized and only visual.
            for (int iSolarSystem = 0; iSolarSystem < numSolarSystems; iSolarSystem++)
            {
                //instantiate solar system childed to the wormhole
                SolarSystemBehavior ssb = Instantiate(m_SolarSystemPrefab, m_InstancedWormhole.transform).GetComponent<SolarSystemBehavior>();

                //decide where to put this solar system's center.
                iterDistance += l_RNG.NextFloatBetween(galaxyInfo.m_MinMaxDistanceBetweenSolarSystems.x, galaxyInfo.m_MinMaxDistanceBetweenSolarSystems.y);

                //let the SSB handle the generation.
                ssb.GenerateSolarSystem(InfoManager.Instance.FocusGalaxy.m_GalaxyInfo.m_SolarSystemInfos[iSolarSystem], //SS info
                                        Math.RandomPointOnSphere(l_RNG, iterDistance)); //SS origin

                //add new solar system to array
                m_InstancedSolarSystems[iSolarSystem] = ssb;
            }

        }

        //void Cleanup
    }

}