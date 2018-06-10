using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{

    [CreateAssetMenu(fileName = "AtmosphereData", menuName = "PlanetExploder/AtmosphereData")]
    public class AtmosphereData : ScriptableObject
    {
        public EComposition m_AtmosphereComposition;

        //minimum Atmosphere mass for this composition type
        public float MinMass;
        //maximum Atmosphere mass for this composition type
        public float MaxMass;
    }
}
