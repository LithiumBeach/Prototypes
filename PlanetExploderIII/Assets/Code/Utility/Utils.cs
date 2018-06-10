using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public static class Utils
    {

        public static readonly string c_MassUnitSymbol = "x 10^25kg";

        /// <summary>
        /// this could be a collections ISort implementation
        /// </summary>
        /// <param name="myList"></param>
        /// <param name="order">how many multiples of the length of the list do we want to pick two rand indices and shuffle them?</param>
        public static void Shuffle(this List<string> myList, int order=1)
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
        internal static int GetRandomIndexFromChanceSet(float[] set, float totalChance=1f)
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
