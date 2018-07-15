using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace dd
{
    public class DialogGraphData : ScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        public List<DialogNodeData> m_Nodes;

        [SerializeField]
        [OnValueChanged("OnNodeSpeakerInfosChanged")]
        private List<string> m_Actors;
        public List<string> Actors
        {
            get
            {
                if (m_Actors == null)
                {
                    m_Actors = new List<string>();
                }
                return m_Actors;
            }
        }

        public void OnNodeSpeakerInfosChanged()
        {
            for (int i = 0; i < Actors.Count; i++)
            {
                //"Actor 1"..."Actor n"
                if (m_Actors[i] == "")
                {
                    m_Actors[i] = "Actor " + (i + 1).ToString();
                }
            }
        }
        public List<string> GetActorStrings()
        {
            List<string> strings = new List<string>();
            for (int i = 0; i < Actors.Count; i++)
            {
                if (m_Actors[i] == "")
                {
                    m_Actors[i] = "Actor " + (i + 1).ToString();
                }
                strings.Add(m_Actors[i]);
            }
            return strings;
        }


#if UNITY_EDITOR
        private System.Random m_Rand;

        [Button("Open Dialog Graph", ButtonSizes.Large)]
        public void OpenDialogGraph()
        {
            //open self, which will trigger DDAssetHandler
            AssetDatabase.OpenAsset(this.GetInstanceID());

            m_Rand = new System.Random();
        }

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
#endif
    }
}