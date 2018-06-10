using System;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    [Serializable]
    public class ECompositionAndCoreData
    {
        public ECompositionAndCoreData(EComposition c, CoreData cd)
        {
            composition = c;
            coreData = cd;
        }
        public EComposition composition;
        public CoreData coreData;
    }
    //[CreateAssetMenu(fileName = "CoreDatabase", menuName = "PlanetExploder/CoreDatabase")]
    public class CoreDatabase : ScriptableObject
    {

        [SerializeField]
        public List<ECompositionAndCoreData> coreDBList;

        public Dictionary<EComposition, CoreData> CoreDB;

        public void PopulateDB(CoreData[] datas)
        {
            coreDBList = new List<ECompositionAndCoreData>();

            for (int i = 0; i < datas.Length; i++)
            {
                coreDBList.Add(new ECompositionAndCoreData(datas[i].m_CoreComposition, datas[i]));
            }
        }

        internal void Initialize()
        {
            CoreDB = new Dictionary<EComposition, CoreData>();
            for (int i = 0; i < coreDBList.Count; i++)
            {
                CoreDB.Add(coreDBList[i].composition, coreDBList[i].coreData);
            }
        }

        public void Clear()
        {
            CoreDB.Clear();
            CoreDB = new Dictionary<EComposition, CoreData>();
        }
    }
}
