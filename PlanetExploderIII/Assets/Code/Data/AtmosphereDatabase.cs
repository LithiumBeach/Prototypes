using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    [Serializable]
    public class ECompositionAndAtmosphereData
    {
        public ECompositionAndAtmosphereData(EComposition c, AtmosphereData cd)
        {
            composition = c;
            AtmosphereData = cd;
        }
        public EComposition composition;
        public AtmosphereData AtmosphereData;
    }

    //[CreateAssetMenu(fileName = "AtmosphereDatabase", menuName = "PlanetExploder/AtmosphereDatabase")]
    public class AtmosphereDatabase : ScriptableObject
    {

        [SerializeField]
        public List<ECompositionAndAtmosphereData> AtmosphereDBList;

        public Dictionary<EComposition, AtmosphereData> AtmosphereDB;

        public void PopulateDB(AtmosphereData[] datas)
        {
            AtmosphereDBList = new List<ECompositionAndAtmosphereData>();

            for (int i = 0; i < datas.Length; i++)
            {
                AtmosphereDBList.Add(new ECompositionAndAtmosphereData(datas[i].m_AtmosphereComposition, datas[i]));
            }
        }

        internal void Initialize()
        {
            AtmosphereDB = new Dictionary<EComposition, AtmosphereData>();
            for (int i = 0; i < AtmosphereDBList.Count; i++)
            {
                AtmosphereDB.Add(AtmosphereDBList[i].composition, AtmosphereDBList[i].AtmosphereData);
            }
        }

        public void Clear()
        {
            AtmosphereDB.Clear();
            AtmosphereDB = new Dictionary<EComposition, AtmosphereData>();
        }
    } 
}