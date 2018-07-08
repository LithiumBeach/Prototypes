using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace dd
{
    [CreateAssetMenu(fileName = "DialogGraphData", menuName = "DialogDirector/Graph", order = 1)]
    public class DialogGraphData : ScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        public List<DialogNodeData> m_Nodes;

#if UNITY_EDITOR
        private System.Random m_Rand;

        [Button("Open Dialog Graph", ButtonSizes.Large)]
        public void OpenDialogGraph()
        {
            //open self, which will trigger DDAssetHandler
            AssetDatabase.OpenAsset(this.GetInstanceID());

            m_Rand = new System.Random();
        }
#endif

        public void Clear()
        {
            m_Nodes = new List<DialogNodeData>();
        }

        public int GetUniqueNodeID()
        {
            if (m_Rand == null)
            {
                m_Rand = new System.Random();
            }

            int newID = 0; //0 is reserved for none
            if (m_Nodes == null)
            {
                Clear();
            }
            while (newID == 0 || m_Nodes.Find((item)=>item.m_NodeID == newID) != null)//while the new ID == 0, or there is a rand id collision, continue generating a rand id
            {
                newID = m_Rand.Next();
            }
            return newID;
        }
    }
}