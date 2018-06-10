using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{

    public class OrbitalSystemCenter : MonoBehaviour
    {
        [Tooltip("start mass")]
        [BoxGroup("Gameplay Data")]
        [ReadOnly]
        public float InitialMass = -1;
        [Tooltip("When the total mass reaches this, the wormhole will open to the next galaxy.")]
        [BoxGroup("Gameplay Data")]
        [ReadOnly]
        public float ActivationMass = -1;

        [Tooltip("Set this to the scale of the outer radius!")]
        public float Radius;

        [HideInInspector]
        //increased by planet matter flying into it
        public float CurrentMass = -1;


    }
}