using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace pe
{
    // table describing composition influence on mass, energy
    // for use with only quantifiable properties
    //[CreateAssetMenu(fileName = "Matter", menuName = "PlanetExploder/Matter")]
    public class MatterDatabase : SerializedScriptableObject
    {
        //composition -> Mass multiplier, Energy multiplier
        public Dictionary<EComposition, CompositionMatrix> m_CompositionMatrix;

        //matter state -> Mass multiplier, Energy multiplier
        public Dictionary<EMatterState, MatterMatrix> StateMatrix;

        //these could also be described by taking the generated base mass/energy, and dividing that up into 3 unequal parts (constant)
        //this would mean more control, but let's see what happens when we just multiply everything together.
        [BoxGroup("Planet Layers Base Values")]
        public MatterMatrix CoreMatrix;
        [BoxGroup("Planet Layers Base Values")]
        public MatterMatrix SurfaceMatrix;
        [BoxGroup("Planet Layers Base Values")]
        public MatterMatrix AtmosphereMatrix;

    }

    

    [Serializable]
    public class CompositionMatrix
    {
        public MatterMatrix matter;
        public Material material;
    }

    [Serializable]
    public class MatterMatrix
    {
        //
        public float MassMultiplier;
        public float EnergyMultiplier;
    }


    [Serializable]
    public enum EComposition
    {
        None,
        Hydrogen,
        Helium,
        Carbon,
        Nitrogen,
        Oxygen,
        Sand,
        Diamond,
        Rock,
        Ice,
        Iron,
        Ocean,
        Swamp,
        Terrestrial, //mix of water and rock/silicate/life/etc
    }
    [Serializable]
    public enum EMatterState
    {
        None,
        Solid,
        Liquid,
        Gas,
        Plasma
    }
}
