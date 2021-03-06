﻿using System;
using UnityEngine;

namespace dd
{
    [Serializable]
    public class DialogNodeData
    {
        [HideInInspector]
        [SerializeField]
        public int m_NodeID;
        [HideInInspector]
        [SerializeField]
        public int m_LocalizationID;
        [HideInInspector]
        [SerializeField]
        public Vector2 m_Position;
        [SerializeField]
        public int m_SpeakerIndex;

        [HideInInspector]
        public int m_ToNodeID;

        public DialogNodeData(int nodeID, int localizationID, Vector2 position, int speakerIndex, int toNodeID=0)
        {
            m_NodeID = nodeID;  m_LocalizationID = localizationID; m_Position = position;
            m_SpeakerIndex = speakerIndex; m_ToNodeID = toNodeID;
        }
    }
}