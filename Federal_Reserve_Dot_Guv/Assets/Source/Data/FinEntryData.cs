using Sirenix.Serialization;
using System;
using UnityEngine;
using util;

namespace lb
{
    //wrapper for value. this is the only place that stores m_Value.
    //also has a name. this is the only place that will store the actual string value m_Name.
    [System.Serializable]
    public class FinEntryData
    {
        //VALUE
        //floats go up to 1e+31. That's enough. fuck showing doubles in the inspector. it's fine this is a large enough number.
        //10^(10^3.8). big big big number: GNP of Unuted states: $19.61 trillion ($1.961e+12) (PPP) (2017)
        //https://www.quora.com/What-is-the-total-amount-of-money-in-the-world
        //all the, I think this is like M3?, currency, m3rstly liquid, but in all the world's economies, converted to USD: $1.2e+15.
        //floating point precision would be enough to, for every singular (PPP) dollar of money in the global economy, have another entire earth's worth of economies.
        //double will be useful later if you decide to factor in 𝑙𝑖𝑡𝑒𝑟𝑎𝑙𝑙𝑦 𝑎𝑙𝑙 𝑡ℎ𝑒 𝑎𝑡𝑜𝑚𝑠 𝑜𝑛 𝑒𝑎𝑟𝑡ℎ.
        public float m_Value;
        //NAME
        public string m_Name;
    }
}