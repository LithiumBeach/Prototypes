using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    [System.Serializable]
    public class DeterministicStructure
    {
        [PropertyOrder(-1)]
        [Button("Generate Seed", ButtonSizes.Small)]
        public void GenerateSeed()
        {
            //eventually, these values will have to be validated to avoid duplicates (unlikely, so for now we'll leave it like this)
            //another benefit of duplicate validation: these Seeds can be used as unique IDs
            Seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

        [ReadOnly]
        public int Seed = -1;

        //TODO: do I want to put an abstract Generate() class here? or does someone else do the actual object instancing?
    }
}