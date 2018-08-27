using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    public static class RandomUtility
    {
        /// <param name="n">number of samples</param>
        /// <param name="center">centerpoint of torus</param>
        /// <param name="R">major radius: the radius from the centerpoint of the torus to the centerpoint of the ring</param>
        /// <param name="r">minor radius: the radius of the ring</param>
        /// <returns>Vector3[] of random points within this torus</returns>
        public static Vector3 GetRandomlyDistributedPointInTheVolumeOfATorus(Vector3 center, float R, float r)
        {
            //random points on y-axis+ normal circle
            float randTheta = Random.Range(0.0f, MathUtility.TWOPI);
            float majorX = Mathf.Cos(randTheta) * R;
            float majorZ = Mathf.Sin(randTheta) * R;

            Vector3 centerPos2D = new Vector3(majorX, 0f, majorZ);

            //random points in a sphere
            //https://stackoverflow.com/questions/5408276/sampling-uniformly-distributed-random-points-inside-a-spherical-volume
            //float phi = Random.Range(0f, MathUtility.TWOPI);
            //float costheta = Random.Range(-1f, 1f);
            //float u = Random.Range(0f, 1f);

            //float theta = Mathf.Acos(costheta);
            //float _r = r * Mathf.Pow(u, 1 / 3);//cubic root

            //float x = _r * Mathf.Sin(theta) * Mathf.Cos(phi);
            //float y = _r * Mathf.Sin(theta) * Mathf.Sin(phi);
            //float z = _r * Mathf.Cos(theta);
            //return (new Vector3(x, y, z) + center) + centerPos2D;
            return GetRandomPointInVolumeOfASphere(center, r) + centerPos2D;
        }

        public static Vector3 GetRandomPointInVolumeOfASphere(Vector3 center, float r)
        {//random points in a sphere
            //https://stackoverflow.com/questions/5408276/sampling-uniformly-distributed-random-points-inside-a-spherical-volume
            float phi = Random.Range(0f, MathUtility.TWOPI);
            float costheta = Random.Range(-1f, 1f);
            float u = Random.Range(0f, 1f);

            float theta = Mathf.Acos(costheta);
            float _r = r * Mathf.Pow(u, 1 / 3);//cubic root

            float x = _r * Mathf.Sin(theta) * Mathf.Cos(phi);
            float y = _r * Mathf.Sin(theta) * Mathf.Sin(phi);
            float z = _r * Mathf.Cos(theta);
            return new Vector3(x, y, z) + center;
        }

        //http://mathworld.wolfram.com/SpherePointPicking.html
        public static Vector3 RandomPointOnSphere(this System.Random rng, float radius)
        {
            float u = rng.NextFloat();
            float v = rng.NextFloat();
            float theta = MathUtility.TWOPI * u;
            float phi = Mathf.Acos(2f * v - 1f);


            return new Vector3(radius * Mathf.Sign(theta) * Mathf.Cos(phi),
                                radius * Mathf.Sin(theta) * Mathf.Sin(phi),
                                radius * Mathf.Cos(theta));
        }

        public static double NextDoubleBetween(this System.Random rand, double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;
        }
        public static float NextFloatBetween(this System.Random rand, float min, float max)
        {
            return rand.NextFloat() * (max - min) + min;
        }

        /// <param name="totalChance">if not normalized.</param>
        /// <returns></returns>
        public static int GetRandomIndexFromChanceSet(float[] set, float totalChance = 1f)
        {
            float rand = UnityEngine.Random.Range(0f, totalChance);
            for (int i = 0; i < set.Length; i++)
            {
                if (rand < set[i])
                {
                    return i;
                }
            }
            Debug.LogError("Error! Utils.GetRandomIndexFromChanceSet didn't find a set! This should never happen.");
            return -1;
        }
    }
}