using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace pe
{
    public class SolarSystemBehavior : MonoBehaviour
    {

        private SolarSystemInfo m_Info = null;
        private OrbitalSystemCenter m_Sun = null;
        public OrbitalSystemCenter Sun { get { return m_Sun; } }
        public Transform SunTrans { get { return m_Sun.transform; } }

        private LineRenderer m_SolarOrbit = null;

        private PlanetBehavior[] m_Planets;
        public PlanetBehavior[] Planets { get { return m_Planets; } }
        private LineRenderer[] m_PlanetOrbits;

        private float m_TotalRadius = 0f;
        public float Radius { get { return m_TotalRadius; } }

        //the solar system has its own seed, ALL THE PLANETS also have their own seeds already generated.
        //spawn the visuals, and use the seeds to deterministically gen their attributes (mass, composition, etc)
        public void GenerateSolarSystem(SolarSystemInfo _Info, Vector3 _Position)
        {
            m_Info = _Info;

            //move this solar system to generated origin
            transform.position = _Position;

            SpawnSun(_Position);

            #region Spawn Planets

            m_Planets = new PlanetBehavior[_Info.m_NumPlanets];
            m_PlanetOrbits = new LineRenderer[_Info.m_NumPlanets];

            System.Random l_RNG = new System.Random(_Info.Seed);

            float iterDistance = 4f;//distance from previous orbit -- initial =  to avoid being inside the sun to start with.
            for (int iPlanet = 0; iPlanet < _Info.m_NumPlanets; iPlanet++)
            {
                //decide where to put this solar system's center.
                iterDistance += l_RNG.NextFloatBetween(InstanceManager.Instance.m_MinMaxDistanceBetweenPlanets.x, InstanceManager.Instance.m_MinMaxDistanceBetweenPlanets.y);

                m_Planets[iPlanet] = SpawnPlanet(_Info.m_Planets[iPlanet],
                                                 Math.RandomPointOnSphere(l_RNG, iterDistance),
                                                 l_RNG.NextFloatBetween(_Info.m_MinMaxPlanetBaseMass.x, _Info.m_MinMaxPlanetBaseMass.y),
                                                 l_RNG.NextFloatBetween(_Info.m_MinMaxPlanetBaseEnergy.x, _Info.m_MinMaxPlanetBaseEnergy.y),
                                                 l_RNG);
                //m_Planets[iPlanet].R = iterDistance;
            }
            m_TotalRadius = iterDistance;

            #endregion
        }

        private void SpawnSun(Vector3 _Position)
        {
            //spawn sun in the center.
            m_Sun = Instantiate(InstanceManager.Instance.m_SunPrefab.gameObject, transform).GetComponent<OrbitalSystemCenter>();
            m_Sun.transform.localPosition = Vector3.zero;

            //spawn the solar orbit around the black hole (Vector3.zero)
            m_SolarOrbit = Instantiate(InstanceManager.Instance.m_GalaxyOrbitLineRenderer.gameObject, transform).GetComponent<LineRenderer>();

            //solar orbit origin is at position of wormhole, which is zero.
            m_SolarOrbit.transform.position = Vector3.zero;
            //we set the up vector so we can do our line point spawning in 2d.
            m_SolarOrbit.transform.up = Vector3.Cross(_Position.normalized, transform.right);

            //subdivide orbit circle into Line Renderer points
            Vector3[] newOrbitPositions = new Vector3[PEConstants.c_OrbitVertices];
            float iterTheta = 0;
            float orbitRadius = _Position.magnitude;
            for (int i = 0; i < PEConstants.c_OrbitVertices; i++)
            {
                iterTheta = ((float)i / (float)PEConstants.c_OrbitVertices) * Math.TWOPI;
                newOrbitPositions[i] = new Vector3(Mathf.Cos(iterTheta) * orbitRadius, 0f, Mathf.Sin(iterTheta) * orbitRadius);
            }

            //update Line Renderer with new positions
            m_SolarOrbit.positionCount = PEConstants.c_OrbitVertices;
            m_SolarOrbit.SetPositions(newOrbitPositions);
        }

        private PlanetBehavior SpawnPlanet(PlanetInfo planetInfo, Vector3 _Position, float baseMass, float baseEnergy, System.Random _rng)
        {
            PlanetBehavior newPlanet = Instantiate(InstanceManager.Instance.m_PlanetPrefab.gameObject, m_Sun.transform).GetComponent<PlanetBehavior>();
            newPlanet.transform.localPosition = _Position;
            newPlanet.name = InfoManager.Instance.GetPlanetName(planetInfo);

            //planet Radius major (orbit radius)
            newPlanet.R = _Position.magnitude;

            LineRenderer orbitLR = Instantiate(InstanceManager.Instance.m_SolarSystemOrbitLineRenderer.gameObject, m_Sun.transform).GetComponent<LineRenderer>();
            orbitLR.transform.localPosition = Vector3.zero;//solar orbit origin is at position of sun, which is at (local) zero.

            //we set the up vector so we can do our line point spawning in 2d.
            orbitLR.transform.up = Vector3.Cross(_Position.normalized, m_Sun.transform.right);

            //subdivide orbit circle into Line Renderer points
            Vector3[] newOrbitPositions = new Vector3[PEConstants.c_OrbitVertices];
            float iterTheta = 0;
            float orbitRadius = _Position.magnitude;
            for (int i = 0; i < PEConstants.c_OrbitVertices; i++)
            {
                iterTheta = ((float)i / (float)PEConstants.c_OrbitVertices) * Math.TWOPI;
                newOrbitPositions[i] = new Vector3(Mathf.Cos(iterTheta) * orbitRadius, 0f, Mathf.Sin(iterTheta) * orbitRadius);
            }

            //update Line Renderer with new positions
            orbitLR.positionCount = PEConstants.c_OrbitVertices;
            orbitLR.SetPositions(newOrbitPositions);

            newPlanet.Generate(_rng.Next(int.MinValue, int.MaxValue), baseMass, baseEnergy);

            return newPlanet;
        }
    }

}
