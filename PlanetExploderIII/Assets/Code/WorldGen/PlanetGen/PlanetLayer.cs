using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    [System.Serializable]
    public class PlanetLayer
    {
        public EComposition Composition;
        public EMatterState State = EMatterState.None;

        //intrinsically related to energy.
        public float Mass;
        public float MassInitial;

        public float Radius;
        public float RadiusInitial;

        //intrinsically related to mass
        public float Energy;//calculated from mass and composition
        //as energy increases from equilibrium to total combustion, it can be visually interpreted as angular velocity in the planet layer mesh,
        //but gameplay it ejects matter, reducing energy. some of this matter is ejected outwards and possibly sucked back into the planet, some of it could
        //reach escape velocity and get sucked into the sun, some of it could hurdle out into space

        //if energy is greater than this, but less than CombustionEnergyThreshold, the energy will "leak" out to equilibrium, ejecting some matter
        public float EquilibriumEnergy;

        //affected by mass. Total energy input required to explode.
        public float CombustionEnergyThreshold;

        public bool IsAlive
        {
            //if Composition == EComposition.None, the Mass should always be 0!!!
            get { return (Mass > 0f && Energy < CombustionEnergyThreshold && Composition != EComposition.None); }
        }
    }
}
