using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    [SerializeField]
    public enum EEnergyType
    {
        Kinetic,
        Electric,
        Magnetic,
        Gravitational,
    }

    public abstract class BaseLaser : MonoBehaviour
    {
        public EEnergyType m_Type;

        [Tooltip("play this on explode (fire lazer) command")]
        public ParticleSystem BeamPrefab;

        [Tooltip("play this on impact with outermost layer.")]
        public ParticleSystem ImpactEffectPrefab;

        [Tooltip("fire this at the planet, with BeamPrefab childed to it. can leave null.")]
        public GameObject ProjectilePrefab;

        /// <summary>
        /// returns if the planet exploded or not
        /// </summary>
        /// <param name="_planet"></param>
        /// <returns></returns>
        public abstract bool FireAt(PlanetBehavior _planet);
        //public abstract bool FireAt(PlanetLayer _layer);
    }
}
