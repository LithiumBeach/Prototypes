﻿
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lb
{
    //for localization or... if symbols ever change 
    //[CreateAssetMenu(fileName = "FedSymbols", menuName = "Data/Fed Symbol Definitions")]
    [System.Serializable]
    public class FedSymbols// : ScriptableObject //FedConstants?
    {
        //dollar symbol
        public string dollar = "$";

        //money precision -- not really a symbol.
        //defs
        public enum EValueStrFormatType
        {
            DIGITS2,
            DIGITS3,
        }
        //TODO: make 3 digit integer component limit (might just be in that handy set func in NumTicker)
        private static readonly string[] c_ValueStrFormatDefinitions = {
            "{0:N2}",
            "{0:N3}"
        };
        [SerializeField]
        //shown in inspector
        private EValueStrFormatType m_FormatType;
        //call from string.Format(x.GetValueStrFormat())
        public string GetValueStrFormat() { return c_ValueStrFormatDefinitions[(int)m_FormatType]; }

        //a definition for K, M, B, T, ... might have to be generated sensically (K^5 or something)
        public static readonly string[] c_FactorShorthands = {
            "K", //1,000
            "M", //million
            "B", //billion
            "T", //trillion
            "Q" //these are going to have to be localized at some point, and constant throughout the game.
        };
        public static readonly string c_FactorOverflowUnit = "K^";

        //utility
        public static string ExtractDigitsOnly(string source)
        {
            //https://stackoverflow.com/questions/8108313/best-way-to-convert-ienumerablechar-to-string
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.Length == 0)
                return source;

            char[] buffer = new char[source.Length];
            int bufferIndex = 0;

            for (int sourceIndex = 0; sourceIndex < source.Length; sourceIndex++)
            {
                char c = source[sourceIndex];

                if (char.IsDigit(c))
                    buffer[bufferIndex++] = c;
            }
            return new string(buffer, 0, bufferIndex);
        }

    }
}