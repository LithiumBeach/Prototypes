using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math
{
    public const float TWOPI = Mathf.PI * 2f;

    /// <param name="n">number of samples</param>
    /// <param name="center">centerpoint of torus</param>
    /// <param name="R">major radius: the radius from the centerpoint of the torus to the centerpoint of the ring</param>
    /// <param name="r">minor radius: the radius of the ring</param>
    /// <returns>Vector3[] of random points within this torus</returns>
    public static Vector3 GetRandomlyDistributedPointInTheVolumeOfATorus(Vector3 center, float R, float r)
    {
        //random points on y-axis+ normal circle
        float randTheta = Random.Range(0.0f, TWOPI);
        float majorX = Mathf.Cos(randTheta) * R;
        float majorZ = Mathf.Sin(randTheta) * R;

        Vector3 centerPos2D = new Vector3(majorX, 0f, majorZ);

        //random points in a sphere
        //https://stackoverflow.com/questions/5408276/sampling-uniformly-distributed-random-points-inside-a-spherical-volume
        float phi = Random.Range(0f, TWOPI);
        float costheta = Random.Range(-1f, 1f);
        float u = Random.Range(0f, 1f);

        float theta = Mathf.Acos(costheta);
        float _r = r * Mathf.Pow(u, 1 / 3);//cubic root

        float x = _r * Mathf.Sin(theta) * Mathf.Cos(phi);
        float y = _r * Mathf.Sin(theta) * Mathf.Sin(phi);
        float z = _r * Mathf.Cos(theta);
        return (new Vector3(x, y, z)) + centerPos2D + center;
    }

    //http://mathworld.wolfram.com/SpherePointPicking.html
    public static Vector3 RandomPointOnSphere(this System.Random rng, float radius)
    {
        float u = rng.NextFloat();
        float v = rng.NextFloat();
        float theta = Math.TWOPI * u;
        float phi = Mathf.Acos(2f * v - 1f);


        return new Vector3( radius * Mathf.Sign(theta) * Mathf.Cos(phi),
                            radius * Mathf.Sin(theta) * Mathf.Sin(phi),
                            radius * Mathf.Cos(theta));
    }

    public static float NextFloat(this System.Random rng)
    {
        return (float)rng.NextDouble();
    }

}
