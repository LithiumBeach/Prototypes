using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    [System.Serializable]
    public class SolarSystemInfo : DeterministicStructure
    {
        //0 = Wormhole, 1 = first SS from wormhole, ... each solar system exists in its own orbital around the wormhole.
        //this is my index in the Galaxy array, and probably unnecessary
        [HideInInspector]
        public int m_Orbital = -1;

        [Range(0, 16)]
        public int m_NumPlanets = 0;

        [Tooltip("Min/Max Random planet BASE mass value, before Matter multipliers are factored.")]
        [MinMaxSlider(0f, 5f, true)]
        public Vector2 m_MinMaxPlanetBaseMass = new Vector2(.75f, 1.25f);
        [Tooltip("Min/Max Random planet BASE energy value, before Matter multipliers are factored.")]
        [MinMaxSlider(0f, 5f, true)]
        public Vector2 m_MinMaxPlanetBaseEnergy = new Vector2(.75f, 1.25f);

        [HideInInspector]
        public PlanetInfo[] m_Planets;

        public float Mass
        {
            get
            {
                float accum = 0f;
                for (int i = 0; i < m_Planets.Length; i++)
                {
                    accum += m_Planets[i].CurrentMass;
                }
                return accum;
            }
        }
    }
}