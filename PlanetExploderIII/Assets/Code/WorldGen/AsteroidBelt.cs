using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public class AsteroidBelt : OrbitObject
    {
        public static int s_Count = 0;

        private int m_AsteroidCount = 0;
        public int m_NumAsteroids = 0;

        public GameObject m_AsteroidPrefab;
        [HideInInspector]
        public List<GameObject> m_Asteroids;

        private void Awake()
        {
            transform.name = "asteroid_belt";
            Spawn();
        }


        public void Spawn()
        {
            s_Count++;

            m_Asteroids = new List<GameObject>();

            for (int i = 0; i < m_NumAsteroids; i++)
            {
                SpawnRandomAsteroid(R, r);
            }
        }

        public void SpawnRandomAsteroid(float R, float r)
        {
            //None of this should be deterministic, doesn't really matter for now as you can't interact with asteroid belts

            Vector3 newPos = Math.GetRandomlyDistributedPointInTheVolumeOfATorus(transform.position, R, r);
            GameObject newAsteroid = Instantiate(m_AsteroidPrefab.gameObject, transform);
            newAsteroid.transform.position = newPos;
            newAsteroid.transform.name = "asteroid" + m_AsteroidCount++.ToString();
            float radius = UnityEngine.Random.Range(.05f, .5f);
            newAsteroid.transform.localScale = new Vector3(radius, radius, radius);
            m_Asteroids.Add(newAsteroid);
        }
    }
}
