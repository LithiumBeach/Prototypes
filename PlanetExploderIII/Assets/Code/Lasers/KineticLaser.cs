using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// it's basically just a big rock.
/// </summary>

namespace pe
{
    public class KineticLaser : BaseLaser
    {
        public float LaserMass = .5f;

        public override bool FireAt(PlanetBehavior _planet)
        {
            if (_planet == null || !_planet.IsAlive)
            {
                return false;
            }

            PlanetLayer _atmosphere = _planet.m_Atmosphere;
            PlanetLayer _surface = _planet.m_Surface;
            PlanetLayer _core = _planet.m_Core;

            float surfaceMassDeltaInitialImpact = 0f;
            float surfaceMassDeltaPostImpact = 0f;

            float surfaceEnergyDeltaInitialImpact = 0f;

            if (_surface.IsAlive)
            {
                switch (_surface.Composition)
                {
                    case EComposition.Carbon:
                        surfaceMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 0.9f) / (Mathf.Sqrt(_surface.Mass))));
                        surfaceMassDeltaInitialImpact = LaserMass;

                        surfaceEnergyDeltaInitialImpact = PEConstants.c_C2 * surfaceMassDeltaPostImpact;
                        break;
                    case EComposition.Sand:
                        surfaceMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 0.75f) / (Mathf.Sqrt(_surface.Mass))));
                        surfaceMassDeltaInitialImpact = LaserMass;

                        surfaceEnergyDeltaInitialImpact = PEConstants.c_C2 * surfaceMassDeltaPostImpact;
                        break;
                    case EComposition.Diamond:
                        surfaceMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 0.5f) / (Mathf.Sqrt(_surface.Mass))));
                        surfaceMassDeltaInitialImpact = LaserMass;

                        surfaceEnergyDeltaInitialImpact = PEConstants.c_C2 * surfaceMassDeltaPostImpact;
                        break;
                    case EComposition.Rock:
                        surfaceMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 1f) / (Mathf.Sqrt(_surface.Mass))));
                        surfaceMassDeltaInitialImpact = LaserMass;

                        surfaceEnergyDeltaInitialImpact = PEConstants.c_C2 * surfaceMassDeltaPostImpact;
                        break;
                    case EComposition.Ice:
                        surfaceMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 1.5f) / (Mathf.Sqrt(_surface.Mass))));
                        surfaceMassDeltaInitialImpact = LaserMass;

                        surfaceEnergyDeltaInitialImpact = PEConstants.c_C2 * surfaceMassDeltaPostImpact;
                        break;
                    case EComposition.Iron:
                        surfaceMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 1f) / (Mathf.Sqrt(_surface.Mass))));
                        surfaceMassDeltaInitialImpact = LaserMass;

                        surfaceEnergyDeltaInitialImpact = PEConstants.c_C2 * surfaceMassDeltaPostImpact;
                        break;
                    case EComposition.Ocean:
                        surfaceMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 0.1f) / (Mathf.Sqrt(_surface.Mass))));
                        surfaceMassDeltaInitialImpact = LaserMass;

                        surfaceEnergyDeltaInitialImpact = PEConstants.c_C2 * surfaceMassDeltaPostImpact;
                        break;
                    case EComposition.Swamp:
                        surfaceMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 3f) / (Mathf.Sqrt(_surface.Mass))));
                        surfaceMassDeltaInitialImpact = LaserMass;

                        surfaceEnergyDeltaInitialImpact = PEConstants.c_C2 * surfaceMassDeltaPostImpact;
                        break;
                    case EComposition.Terrestrial:
                        surfaceMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 1.2f) / (Mathf.Sqrt(_surface.Mass))));
                        surfaceMassDeltaInitialImpact = LaserMass;

                        surfaceEnergyDeltaInitialImpact = PEConstants.c_C2 * surfaceMassDeltaPostImpact;
                        break;
                    default:
                        surfaceMassDeltaPostImpact = 0f;
                        surfaceMassDeltaInitialImpact = 0f;
                        surfaceEnergyDeltaInitialImpact = LaserMass * .1f;
                        break;
                }
                //modify the surface directly
                _surface.Mass += surfaceMassDeltaInitialImpact;
                _surface.Mass -= surfaceMassDeltaPostImpact;
                _surface.Energy += surfaceEnergyDeltaInitialImpact;
            }

            float coreMassDeltaInitialImpact = 0f;
            float coreMassDeltaPostImpact = 0f;

            float coreEnergyDeltaInitialImpact = 0f;

            if (_core.IsAlive)
            {
                //these values are what is leftover from the blast on the surface.
                //The only way the kinetic lazer can affect the core is if the surface is destroyed already (or rather, will be destroyed).
                float remainingMass = 0f;
                float remainingEnergy = 0f;
                //take remainder surface mass.
                if (_surface.Mass <= 0f)
                {
                    remainingMass = -_surface.Mass;
                    _surface.Mass = 0f;
                }
                if (_surface.Energy >= _surface.CombustionEnergyThreshold)
                {
                    remainingEnergy = surfaceEnergyDeltaInitialImpact;
                }

                //if the surface is not alive (the above two ifs were false)
                if (!_surface.IsAlive)
                {
                    switch (_core.Composition)
                    {
                        case EComposition.Carbon:
                            coreMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 0.9f) / (Mathf.Sqrt(_core.Mass))));
                            coreMassDeltaInitialImpact = LaserMass;

                            coreEnergyDeltaInitialImpact = PEConstants.c_C2 * coreMassDeltaPostImpact;
                            break;
                        case EComposition.Sand:
                            coreMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 0.75f) / (Mathf.Sqrt(_core.Mass))));
                            coreMassDeltaInitialImpact = LaserMass;

                            coreEnergyDeltaInitialImpact = PEConstants.c_C2 * coreMassDeltaPostImpact;
                            break;
                        case EComposition.Diamond:
                            coreMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 0.5f) / (Mathf.Sqrt(_core.Mass))));
                            coreMassDeltaInitialImpact = LaserMass;

                            coreEnergyDeltaInitialImpact = PEConstants.c_C2 * coreMassDeltaPostImpact;
                            break;
                        case EComposition.Rock:
                            coreMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 1f) / (Mathf.Sqrt(_core.Mass))));
                            coreMassDeltaInitialImpact = LaserMass;

                            coreEnergyDeltaInitialImpact = PEConstants.c_C2 * coreMassDeltaPostImpact;
                            break;
                        case EComposition.Ice:
                            coreMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 1.5f) / (Mathf.Sqrt(_core.Mass))));
                            coreMassDeltaInitialImpact = LaserMass;

                            coreEnergyDeltaInitialImpact = PEConstants.c_C2 * coreMassDeltaPostImpact;
                            break;
                        case EComposition.Iron:
                            coreMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 1f) / (Mathf.Sqrt(_core.Mass))));
                            coreMassDeltaInitialImpact = LaserMass;

                            coreEnergyDeltaInitialImpact = PEConstants.c_C2 * coreMassDeltaPostImpact;
                            break;
                        case EComposition.Ocean:
                            coreMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 0.1f) / (Mathf.Sqrt(_core.Mass))));
                            coreMassDeltaInitialImpact = LaserMass;

                            coreEnergyDeltaInitialImpact = PEConstants.c_C2 * coreMassDeltaPostImpact;
                            break;
                        case EComposition.Swamp:
                            coreMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 3f) / (Mathf.Sqrt(_core.Mass))));
                            coreMassDeltaInitialImpact = LaserMass;

                            coreEnergyDeltaInitialImpact = PEConstants.c_C2 * coreMassDeltaPostImpact;
                            break;
                        case EComposition.Terrestrial:
                            coreMassDeltaPostImpact = LaserMass * (1 + ((PEConstants.c_C2 * 1.2f) / (Mathf.Sqrt(_core.Mass))));
                            coreMassDeltaInitialImpact = LaserMass;

                            coreEnergyDeltaInitialImpact = PEConstants.c_C2 * coreMassDeltaPostImpact;
                            break;
                        default:
                            coreMassDeltaPostImpact = 0f;
                            coreMassDeltaInitialImpact = 0f;
                            coreEnergyDeltaInitialImpact = LaserMass * .1f;
                            break;
                    }
                    //modify the core
                    _core.Mass += coreMassDeltaInitialImpact;
                    _core.Mass -= coreMassDeltaPostImpact;
                    _core.Energy += coreEnergyDeltaInitialImpact;

                }
                else
                {
                    //add the remainder values to the core.
                    _core.Mass -= remainingMass;
                    _core.Energy += remainingEnergy;
                }
            }

            //for now, just kill the atmosphere if the core and surface have been killed
            if (_atmosphere.IsAlive && !_core.IsAlive && !_surface.IsAlive)
            {
                _atmosphere.Mass = 0f;
                _atmosphere.Energy = 0f;
            }

            //finally, tell the surface and/or core to explode if they should.
            if (!_surface.IsAlive && !_core.IsAlive)
            {
                _atmosphere.Mass = 0f;
                _surface.Mass = 0f;
                _core.Mass = 0f;
                _atmosphere.Energy = 0f;
                _surface.Energy = 0f;
                _core.Energy = 0f;
                _planet.Explode();
            }

            #region notes
            //Mass: on hit, increase by mass of lazer (IMMEDIATE). Calculate an amount of mass to eject into space based on current mass 
            //(generally significantly larger than the mass of the lazer). over time.

            //Energy: on hit, increase by ke of lazer. Lose that energy over time, until equilibrium is reached.

            //all planets have an equilibrium energy based on it's mass (and composition)

            //all planets have a CRITICAL energy based on it's mass (and composition). this value is quite large. if a lazer is powerful enough to surpass this, the planet explodes immediately.

            //reducing surface's mass to 0 will carry over the remainder of the blow
            //reaching the surface's critical energy will carry over *all* of the energy.
            //you can display data for total energy of the planet by summing the core, surface, and atmosphere Es.
            #endregion

            //TEMP::

            CrashIntoProjectile projectile = Instantiate(ProjectilePrefab, transform).GetComponent<CrashIntoProjectile>();
            projectile.transform.localPosition = new Vector3(1f, 0, 0);
            Vector3 crashpos = _planet.transform.position + (transform.position - _planet.transform.position).normalized * _planet.r;
            projectile.Initialize(_planet.transform.position, 3f, OnCrashHit);

            //::ENDTEMP

            return false;
        }
        private void OnCrashHit(CrashIntoProjectile proj)
        {
            ForceOverLifetimeTowards effectForce = Instantiate(ImpactEffectPrefab).GetComponentInChildren<ForceOverLifetimeTowards>(true);
            effectForce.transform.position = proj.transform.position;
            effectForce.transform.forward = (transform.position - proj.transform.position).normalized;

            effectForce.Initialize(new ForceOverLifetimeTowards.Target[]
                                    {
                                        new ForceOverLifetimeTowards.Target(InstanceManager.Instance.FocusSolarSystem.SunTrans.transform, .5f, 25f,
                                                                            InstanceManager.Instance.FocusSolarSystem.Sun.Radius * InstanceManager.Instance.FocusSolarSystem.Sun.Radius), //*.75 to go in a little bit before destroying
                                        new ForceOverLifetimeTowards.Target(InstanceManager.Instance.FocusPlanet.transform, .5f, 10f,
                                                                            InstanceManager.Instance.FocusPlanet.r * InstanceManager.Instance.FocusPlanet.r * .75f) //*.75 to go in a little bit before destroying
                                    },
                                    100);

            Destroy(proj.gameObject);
        }
    }
}
