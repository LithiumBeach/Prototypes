using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public class PlanetInfo :  DeterministicStructure
    {
        //0 = Sun, 1 = first Planet from sun, ... each planet exists in its own orbital around the sun.
        //this is my index in the solar system array, and probably unnecessary
        [HideInInspector]
        public int m_Orbital = -1;

        public float InitialMass;

        public float CurrentMass;
    }
}