using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    public static class Utils
    {
        //naturally, this does fuck-all.
        ////https://answers.unity.com/questions/458207/copy-a-component-at-runtime.html
        //public static Component CopyComponent(Component original, GameObject destination)
        //{
        //    System.Type type = original.GetType();
        //    Component copy = destination.AddComponent(type);
        //    // Copied fields can be restricted with BindingFlags
        //    System.Reflection.FieldInfo[] fields = type.GetFields();
        //    foreach (System.Reflection.FieldInfo field in fields)
        //    {
        //        field.SetValue(copy, field.GetValue(original));
        //    }
        //    return copy;
        //}


        /// <summary>
        /// this could be a collections ISort implementation
        /// </summary>
        /// <param name="myList"></param>
        /// <param name="order">how many multiples of the length of the list do we want to pick two rand indices and shuffle them?</param>
        public static void Shuffle(this List<string> myList, int order = 1)
        {
            order = Mathf.Max(1, order);
            int listLen = myList.Count;
            int len = myList.Count * order;
            int a = 0;
            int b = 0;
            string tmp = "";
            for (int i = 0; i < len; i++)
            {
                //I don't care that it's possible for a and b to be the same.
                a = UnityEngine.Random.Range(0, listLen);
                b = UnityEngine.Random.Range(0, listLen);

                tmp = myList[a];
                myList[a] = myList[b];
                myList[b] = tmp;
            }
        }

        public static float NextFloat(this System.Random rng)
        {
            return (float)rng.NextDouble();
        }

        internal static void DrawSquare2D(float tileSize, Vector2 pos, Color color)
        {
            Debug.DrawLine(new Vector3(pos.x - tileSize, pos.y - tileSize), new Vector3(pos.x - tileSize, pos.y + tileSize), color);
            Debug.DrawLine(new Vector3(pos.x - tileSize, pos.y + tileSize), new Vector3(pos.x + tileSize, pos.y + tileSize), color);
            Debug.DrawLine(new Vector3(pos.x + tileSize, pos.y + tileSize), new Vector3(pos.x + tileSize, pos.y - tileSize), color);
            Debug.DrawLine(new Vector3(pos.x + tileSize, pos.y - tileSize), new Vector3(pos.x - tileSize, pos.y - tileSize), color);
        }

        public static void DrawCirlce2D(float r, Vector2 pos, Color color, int resolution = 32)
        {
            float theta = 0f;
            float nextTheta = 0f;
            for (int i = 0; i < resolution; i++)
            {
                theta = MathUtility.TWOPI * (float)i / (float)resolution;
                nextTheta = MathUtility.TWOPI * (float)(i + 1) / (float)resolution;

                Debug.DrawLine(new Vector3(pos.x + Mathf.Cos(theta), pos.y + Mathf.Sin(theta), 0), new Vector3(pos.x + Mathf.Cos(nextTheta), pos.y + Mathf.Sin(nextTheta), 0), color);
            }
        }

        //return the 4 frustum corner rays in CAMERA SPACE, defining the bounds of the frustum. 1 ray per row.
        //matrix indices (rows):
        //TOP LEFT CORNER       = 0
        //TOP RIGHT CORNER      = 1
        //BOTTOM RIGHT CORNER   = 2
        //BOTTOM LEFT CORNER    = 3
        //http://flafla2.github.io/2016/10/01/raymarching.html
        public static Matrix4x4 GetFrustumCorners(Camera cam)
        {
            float fov = cam.fieldOfView;
            float halfFov = cam.fieldOfView * .5f;
            float tanFov = Mathf.Tan(halfFov * Mathf.Deg2Rad);
            float aspect = cam.aspect;

            //frustum corners
            Matrix4x4 corners = Matrix4x4.identity;

            //similar to half width and half height of a rect.
            Vector3 dirRight = Vector3.right * tanFov * aspect;
            Vector3 dirUp = Vector3.up * tanFov;

            //find corners.
            Vector3 topLeft = (-Vector3.forward - dirRight + dirUp);
            Vector3 topRight = (-Vector3.forward + dirRight + dirUp);
            Vector3 bottomRight = (-Vector3.forward + dirRight - dirUp);
            Vector3 bottomLeft = (-Vector3.forward - dirRight - dirUp);

            //set corners in matrix
            corners.SetRow(0, topLeft);
            corners.SetRow(1, topRight);
            corners.SetRow(2, bottomRight);
            corners.SetRow(3, bottomLeft);

            return corners;
        }
    }
}
