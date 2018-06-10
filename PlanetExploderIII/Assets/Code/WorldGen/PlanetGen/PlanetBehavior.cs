using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    /// <summary>
    /// we don't need to save the initial data.
    /// adjust these values directly
    /// </summary>
    public class PlanetData
    {
        [ReadOnly]
        public PlanetLayer m_Core;
        [ReadOnly]
        public PlanetLayer m_Surface;
        [ReadOnly]
        public PlanetLayer m_Atmosphere;

    }

    public class PlanetBehavior : OrbitObject
    {
        //destroy me when finished.
        public ParticleSystem m_ExplosionPS;

        [Required]
        public ObjectRotator m_CoreGO;
        [Required]
        public ObjectRotator m_SurfaceGO;
        [Required]
        public ObjectRotator m_AtmosphereGO;

        [ReadOnly]
        [BoxGroup("Layers")]
        public PlanetLayer m_Core;
        [ReadOnly]
        [BoxGroup("Layers")]
        public PlanetLayer m_Surface;
        [ReadOnly]
        [BoxGroup("Layers")]
        public PlanetLayer m_Atmosphere;

        #region Core+Surface+Atmosphere sum getters
        [ShowInInspector]
        [BoxGroup("meta")]
        public float Mass
        {
            get { return m_Core.Mass + m_Surface.Mass + m_Atmosphere.Mass; }
        }
        [ShowInInspector]
        [BoxGroup("meta")]
        public float Energy
        {
            get { return m_Core.Energy + m_Surface.Energy + m_Atmosphere.Energy; }
        }
        [ShowInInspector]
        [BoxGroup("meta")]
        public float EquilibriumEnergy
        {
            get { return m_Core.EquilibriumEnergy + m_Surface.EquilibriumEnergy + m_Atmosphere.EquilibriumEnergy; }
        }
        [ShowInInspector]
        [BoxGroup("meta")]
        public float CombustionEnergyThreshold
        {
            get { return m_Core.CombustionEnergyThreshold + m_Surface.CombustionEnergyThreshold + m_Atmosphere.CombustionEnergyThreshold; }
        }
        [ShowInInspector]
        [BoxGroup("meta")]
        public bool IsAlive
        {
            get { return m_Core.IsAlive || m_Surface.IsAlive || m_Atmosphere.IsAlive; }
        }
        #endregion

        public bool b_IsExploding = false;

        //these do not need to sum to 1.
        private static readonly float c_CoreRadiusPctMin = .1f;
        private static readonly float c_CoreRadiusPctMax = .3f;
        private static readonly float c_SurfaceRadiusPctMin = .55f;
        private static readonly float c_SurfaceRadiusPctMax = .8f;
        private static readonly float c_AtmosphereRadiusPctMin = .1f;
        private static readonly float c_AtmosphereRadiusPctMax = .2f;

        //multiply this by energy to get rotational velocity
        private static readonly float c_EnergyToAngularVelocityMultiplier = 256;

        public void Generate(int seed, float baseMass, float baseEnergy)
        {
            System.Random rng = new System.Random(seed);

            #region Composition
            int coreIndex = rng.Next(0, InfoManager.Instance.FocusGalaxy.m_Cores.Length);
            m_Core.Composition = InfoManager.Instance.FocusGalaxy.m_Cores[coreIndex];

            int surfaceIndex = rng.Next(0, InfoManager.Instance.FocusGalaxy.m_Surfaces.Length);
            m_Surface.Composition = InfoManager.Instance.FocusGalaxy.m_Surfaces[surfaceIndex];

            int atmosphereIndex = rng.Next(0, InfoManager.Instance.FocusGalaxy.m_Atmospheres.Length);
            m_Atmosphere.Composition = InfoManager.Instance.FocusGalaxy.m_Atmospheres[atmosphereIndex];
            #endregion

            #region Matter State
            if (m_Core.Composition == EComposition.None)
            {
                m_Core.State = EMatterState.None;
            }
            else
            {
                m_Core.State = rng.Next(0, 2) == 1 ? EMatterState.Liquid : EMatterState.Solid;
            }

            if (m_Surface.Composition == EComposition.None)
            {
                m_Surface.State = EMatterState.None;
            }
            else
            {
                m_Surface.State = EMatterState.Solid;
            }

            if (m_Atmosphere.Composition == EComposition.None)
            {
                m_Atmosphere.State = EMatterState.None;
            }
            else
            {
                m_Atmosphere.State = EMatterState.Gas;
            }
            #endregion

            #region Mass
            //state influence for solids/liquids is 1. gas < 1. plasma >> 1. none = 0: Defined in MatterDatabase
            m_Core.Mass = m_Core.MassInitial = baseMass *
                          DBManager.Instance.MatterDB.CoreMatrix.MassMultiplier *
                          DBManager.Instance.MatterDB.m_CompositionMatrix[m_Core.Composition].matter.MassMultiplier *
                          DBManager.Instance.MatterDB.StateMatrix[m_Core.State].MassMultiplier;
            m_Surface.Mass = m_Surface.MassInitial = baseMass *
                          DBManager.Instance.MatterDB.SurfaceMatrix.MassMultiplier *
                          DBManager.Instance.MatterDB.m_CompositionMatrix[m_Surface.Composition].matter.MassMultiplier *
                          DBManager.Instance.MatterDB.StateMatrix[m_Surface.State].MassMultiplier;
            m_Atmosphere.Mass = m_Atmosphere.MassInitial = baseMass *
                          DBManager.Instance.MatterDB.AtmosphereMatrix.MassMultiplier *
                          DBManager.Instance.MatterDB.m_CompositionMatrix[m_Atmosphere.Composition].matter.MassMultiplier *
                          DBManager.Instance.MatterDB.StateMatrix[m_Atmosphere.State].MassMultiplier;
            #endregion

            #region Equilibrium Energy
            //state influence for solids/liquids is 1. gas < 1. plasma >> 1. none = 0: Defined in MatterDatabase
            m_Core.Energy = m_Core.EquilibriumEnergy = baseEnergy *
                          DBManager.Instance.MatterDB.CoreMatrix.EnergyMultiplier *
                          DBManager.Instance.MatterDB.m_CompositionMatrix[m_Core.Composition].matter.EnergyMultiplier *
                          DBManager.Instance.MatterDB.StateMatrix[m_Core.State].EnergyMultiplier;
            m_Surface.Energy = m_Surface.EquilibriumEnergy = baseEnergy *
                          DBManager.Instance.MatterDB.SurfaceMatrix.EnergyMultiplier *
                          DBManager.Instance.MatterDB.m_CompositionMatrix[m_Surface.Composition].matter.EnergyMultiplier *
                          DBManager.Instance.MatterDB.StateMatrix[m_Surface.State].EnergyMultiplier;
            m_Atmosphere.Energy = m_Atmosphere.EquilibriumEnergy = baseEnergy *
                          DBManager.Instance.MatterDB.AtmosphereMatrix.EnergyMultiplier *
                          DBManager.Instance.MatterDB.m_CompositionMatrix[m_Atmosphere.Composition].matter.EnergyMultiplier *
                          DBManager.Instance.MatterDB.StateMatrix[m_Atmosphere.State].EnergyMultiplier;
            #endregion

            //TODO: CombustionEnergyThreshold
            m_Core.CombustionEnergyThreshold = 100000;
            m_Surface.CombustionEnergyThreshold = 100000;
            m_Atmosphere.CombustionEnergyThreshold = 100000;

            #region radius -> transform.scale. mass is unrelated to initial radius.
            float rMinor = rng.NextFloatBetween(InfoManager.Instance.m_MinMaxPlanetSize.x, InfoManager.Instance.m_MinMaxPlanetSize.y);

            //divvy up the planet's r minor (radius of the planet) between the core, surface, and atmosphere
            if (m_Core.IsAlive)
            {
                m_Core.Radius = rng.NextFloatBetween(c_CoreRadiusPctMin, c_CoreRadiusPctMax) * rMinor; 
            }
            else
            {
                m_Core.Radius = 0;
            }
            if (m_Surface.IsAlive)
            {
                m_Surface.Radius = m_Core.Radius + rng.NextFloatBetween(c_SurfaceRadiusPctMin, c_SurfaceRadiusPctMax) * rMinor; 
            }
            else
            {
                m_Surface.Radius = 0;
            }
            if (m_Atmosphere.IsAlive)
            {
                m_Atmosphere.Radius = m_Surface.Radius + rng.NextFloatBetween(c_AtmosphereRadiusPctMin, c_AtmosphereRadiusPctMax) * rMinor; 
            }
            else
            {
                m_Atmosphere.Radius = 0;
            }
            #endregion

            #region Visual
            //scale = radius
            m_AtmosphereGO.transform.localScale = new Vector3(m_Atmosphere.Radius, m_Atmosphere.Radius, m_Atmosphere.Radius);
            m_SurfaceGO.transform.localScale = new Vector3(m_Surface.Radius, m_Surface.Radius, m_Surface.Radius);
            m_CoreGO.transform.localScale = new Vector3(m_Core.Radius, m_Core.Radius, m_Core.Radius);

            //rotational velocity directly related to energy
            m_AtmosphereGO.m_RotateSpeed = m_Atmosphere.Energy * c_EnergyToAngularVelocityMultiplier;
            m_SurfaceGO.m_RotateSpeed = m_Surface.Energy * c_EnergyToAngularVelocityMultiplier;
            m_CoreGO.m_RotateSpeed = m_Core.Energy * c_EnergyToAngularVelocityMultiplier;

            //turn off all dead layers
            if (!m_Core.IsAlive)
            {
                m_CoreGO.gameObject.SetActive(false);
            }
            if (!m_Surface.IsAlive)
            {
                m_SurfaceGO.gameObject.SetActive(false);
            }
            if (!m_Atmosphere.IsAlive)
            {
                m_AtmosphereGO.gameObject.SetActive(false);
            }
            #endregion
        }

        public override float r
        {
            get { return m_Atmosphere.Radius + m_Surface.Radius + m_Core.Radius; }
            set
            {
                Debug.LogError("you should not be setting the radius minor for a planet directly!! Extrapolate this value from atmos+surface+core.");
                base.r = value;
            }
        }


        #region Explode / Destroy
        public override void Explode()
        {
            m_ExplosionPS = Instantiate(m_ExplosionPS, transform);
            m_ExplosionPS.transform.localPosition = Vector3.zero;
            m_ExplosionPS.Play();

            //Handle effect gravity towards sun.
            //ForceOverLifetimeTowards psController = m_ExplosionPS.GetComponentInChildren<ForceOverLifetimeTowards>(true);
            //psController.targets.trans = InstanceManager.Instance.FocusSolarSystem.SunTrans.transform;
            //psController.targets.percentAffected = .5f;

            //GetComponent<MeshRenderer>().enabled = false;//hide planet
            b_IsExploding = true;
            StartCoroutine("DestroySelfAfterSeconds", m_ExplosionPS.main.duration + m_ExplosionPS.subEmitters.GetSubEmitterSystem(0).main.duration);
        }

        private IEnumerator DestroySelfAfterSeconds(float t)
        {
            yield return new WaitForSeconds(t);

            //STUB!!
            Destroy(gameObject);
            Debug.Log("Implement PlanetBehavior.OnDestroy()!");
        }

        #endregion
    }
}
