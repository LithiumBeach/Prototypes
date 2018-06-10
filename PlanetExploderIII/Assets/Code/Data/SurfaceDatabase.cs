using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    [Serializable]
    public class ECompositionAndSurfaceData
    {
        public ECompositionAndSurfaceData(EComposition c, SurfaceData cd)
        {
            composition = c;
            SurfaceData = cd;
        }
        public EComposition composition;
        public SurfaceData SurfaceData;
    }
    //[CreateAssetMenu(fileName = "SurfaceDatabase", menuName = "PlanetExploder/SurfaceDatabase")]
    public class SurfaceDatabase : ScriptableObject
    {

        [SerializeField]
        public List<ECompositionAndSurfaceData> SurfaceDBList;

        public Dictionary<EComposition, SurfaceData> SurfaceDB;

        public void PopulateDB(SurfaceData[] datas)
        {
            SurfaceDBList = new List<ECompositionAndSurfaceData>();

            for (int i = 0; i < datas.Length; i++)
            {
                SurfaceDBList.Add(new ECompositionAndSurfaceData(datas[i].m_SurfaceComposition, datas[i]));
            }
        }

        internal void Initialize()
        {
            SurfaceDB = new Dictionary<EComposition, SurfaceData>();
            for (int i = 0; i < SurfaceDBList.Count; i++)
            {
                SurfaceDB.Add(SurfaceDBList[i].composition, SurfaceDBList[i].SurfaceData);
            }
        }

        public void Clear()
        {
            SurfaceDB.Clear();
            SurfaceDB = new Dictionary<EComposition, SurfaceData>();
        }
    }
}
