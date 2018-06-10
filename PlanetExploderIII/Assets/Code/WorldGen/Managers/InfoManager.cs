using patterns;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace pe
{
    public class InfoManager : SingletonBehavior<InfoManager>
    {
        public UniverseData m_UniverseData;

        [MinMaxSlider(0f, 5f, true)]
        [FoldoutGroup("Galaxy")]
        public Vector2 m_MinMaxPlanetSize = new Vector2(.5f, 3f);

        public int m_FocusGalaxyIndex = 0; //TODO: save/load
        public GalaxyInfoGroup FocusGalaxy
        {
            get { return m_FocusGalaxyIndex >= 0 ? m_UniverseData.Galaxies[m_FocusGalaxyIndex] : null; }
        }

        public int m_FocusSolarSystemIndex = -1;
        public SolarSystemInfo FocusSolarSystem
        {
            get { return m_FocusSolarSystemIndex >= 0 ? FocusGalaxy.m_GalaxyInfo.m_SolarSystemInfos[m_FocusSolarSystemIndex] : null; }
        }

        public int m_FocusPlanetIndex = -1;
        public PlanetInfo FocusPlanet
        {
            get { return m_FocusPlanetIndex >= 0 ? FocusSolarSystem.m_Planets[m_FocusPlanetIndex] : null; }
        }

        protected override void OnAwake()
        {
            InitializeSolarSystems();
        }

        //Galaxy seeds are set in UniverseData
        //Solar System seeds are set in UniverseData
        public void InitializeSolarSystems()
        {
            //for all galaxies
            for (int iGalaxy = 0; iGalaxy < m_UniverseData.NumGalaxies; iGalaxy++)
            {
                //for all solar systems
                for (int iSolarSystem = 0; iSolarSystem < m_UniverseData.Galaxies[iGalaxy].m_GalaxyInfo.m_SolarSystemInfos.Length; iSolarSystem++)
                {
                    int numPlanets = m_UniverseData.Galaxies[iGalaxy].m_GalaxyInfo.m_SolarSystemInfos[iSolarSystem].m_NumPlanets;
                    //initialize planets array
                    m_UniverseData.Galaxies[iGalaxy].m_GalaxyInfo.m_SolarSystemInfos[iSolarSystem].m_Planets = new PlanetInfo[numPlanets];

                    //generate random planet seed from solar system's seed (set in data)
                    System.Random rng = new System.Random(m_UniverseData.Galaxies[iGalaxy].m_GalaxyInfo.m_SolarSystemInfos[iSolarSystem].Seed);

                    //for all planets
                    for (int iPlanet = 0; iPlanet < numPlanets; iPlanet++)
                    {
                        //add new to planet array
                        m_UniverseData.Galaxies[iGalaxy].m_GalaxyInfo.m_SolarSystemInfos[iSolarSystem].m_Planets[iPlanet] = new PlanetInfo();
                        m_UniverseData.Galaxies[iGalaxy].m_GalaxyInfo.m_SolarSystemInfos[iSolarSystem].m_Planets[iPlanet].Seed = rng.Next(int.MinValue, int.MaxValue);
                    }
                }
            }
        }

        public string[] GetAllSolarSystemNamesInGalaxy(GalaxyInfo galaxy)
        {
            string[] names = new string[galaxy.m_SolarSystemInfos.Length];
            for (int i = 0; i < galaxy.m_SolarSystemInfos.Length; i++)
            {
                //deterministic: names will always return the same for a given seed.
                names[i] = FileToList.Instance.GetRandomSolarSystemName(galaxy.m_SolarSystemInfos[i].Seed);
            }
            return names;
        }
        internal float[] GetAllSolarSystemSizesInGalaxy(GalaxyInfo galaxy)
        {
            float[] sizes = new float[galaxy.m_SolarSystemInfos.Length];
            for (int i = 0; i < galaxy.m_SolarSystemInfos.Length; i++)
            {
                //deterministic: names will always return the same for a given seed.
                sizes[i] = galaxy.m_SolarSystemInfos[i].Mass;
            }
            return sizes;
        }

        public string[] GetAllPlanetNamesInSolarSystem(SolarSystemInfo ss)
        {
            string[] names = new string[ss.m_Planets.Length];
            for (int i = 0; i < ss.m_Planets.Length; i++)
            {
                //deterministic: names will always return the same for a given seed.
                names[i] = FileToList.Instance.GetRandomPlanetName(ss.m_Planets[i].Seed);
            }
            return names;
        }

        public string GetPlanetName(PlanetInfo pi)
        {
            return FileToList.Instance.GetRandomPlanetName(pi.Seed);
        }

        internal float[] GetAllPlanetMassesInSolarSystem(SolarSystemInfo ss)
        {
            float[] sizes = new float[ss.m_Planets.Length];
            for (int i = 0; i < ss.m_Planets.Length; i++)
            {
                //deterministic: names will always return the same for a given seed.
                sizes[i] = ss.m_Planets[i].CurrentMass;
            }
            return sizes;
        }

    }

}