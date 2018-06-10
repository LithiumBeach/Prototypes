using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{

    [CreateAssetMenu(fileName = "SurfaceData", menuName = "PlanetExploder/SurfaceData")]
    [UnityEditor.CanEditMultipleObjects]
    public class SurfaceData : ScriptableObject
    {
        public EComposition m_SurfaceComposition;

        //minimum Surface mass for this composition type
        public float MinMass;
        //maximum Surface mass for this composition type
        public float MaxMass;

        //[Header("Compatible Atmosphere Compositions")]
        public EComposition[] CompatibleAtmosphereCompositions;

        internal bool IsAtmosphereCompatible(EComposition _composition)
        {
            for (int i = 0; i < CompatibleAtmosphereCompositions.Length; i++)
            {
                if (CompatibleAtmosphereCompositions[i] == _composition)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
