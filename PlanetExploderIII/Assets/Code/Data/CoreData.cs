using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{

    [CreateAssetMenu(fileName = "CoreData", menuName = "PlanetExploder/CoreData")]
    public class CoreData : ScriptableObject
    {
        public EComposition m_CoreComposition;

        //minimum Core mass for this composition type
        public float MinMass;
        //maximum Core mass for this composition type
        public float MaxMass;

        public EComposition[] CompatibleSurfaceCompositions;

        internal bool IsSurfaceCompatible(EComposition _composition)
        {
            for (int i = 0; i < CompatibleSurfaceCompositions.Length; i++)
            {
                if (CompatibleSurfaceCompositions[i] == _composition)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
