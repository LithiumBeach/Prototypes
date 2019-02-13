using UnityEngine;
using util;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;

namespace lb
{
    public class TimelinePopulateEditorUtility : MonoBehaviour
    {
        public RectTransform m_MinorNotchPrefab; //TODO: NotchBehavior (for displaying text at a notch? maybe not)
        public RectTransform m_MajorNotchPrefab; //TODO: NotchBehavior (for displaying text at a notch? maybe not)
        public RectTransform m_NotchRoot;
        public int m_NotchCount;
        private List<RectTransform> m_Notches;

        public int m_EveryNthMakeMajor = 12;


        [Button("Generate and Place Notches\nPopulate Timeline Component Notch Array", ButtonSizes.Large)]
        private void GenerateNotches()
        {
            if (m_Notches == null)
            {
                m_Notches = new List<RectTransform>();
            }

            for (int i = 0; i < m_Notches.Count; i++)
            {
                if (m_Notches[i] != null)
                {
                    Editor.DestroyImmediate(m_Notches[i].gameObject);
                }
                else// (m_Notches[i] == null)
                {
                    m_Notches.Remove(m_Notches[i]);
                    i--;
                }
            }
            m_Notches.Clear();

            int screenWidth = Screen.width;
            float halfStep = (1.0f / (float)m_NotchCount) *.5f;
            for (int i = 0; i < m_NotchCount; i++)
            {
                RectTransform newNotch;
                Vector2 sizeDelta;
                if (i % m_EveryNthMakeMajor == 0)
                {
                    newNotch = Editor.Instantiate(m_MajorNotchPrefab.gameObject).GetComponent<RectTransform>();
                    sizeDelta = m_MajorNotchPrefab.sizeDelta;
                }
                else
                {
                    newNotch = Editor.Instantiate(m_MinorNotchPrefab.gameObject).GetComponent<RectTransform>();
                    sizeDelta = m_MinorNotchPrefab.sizeDelta;
                }
                m_Notches.Add(newNotch);

                newNotch.SetParent(m_NotchRoot);
                newNotch.anchorMin = new Vector2(halfStep + i * (1.0f / (float)m_NotchCount), 0);
                newNotch.anchorMax = new Vector2(halfStep + i * (1.0f / (float)m_NotchCount), 1);
                newNotch.localScale = Vector3.one;
                newNotch.sizeDelta = sizeDelta;// new Vector2(m_NotchPrefab.sizeDelta.x, m_NotchPrefab.sizeDelta.y);
                //newNotch.localPosition = Vector2.zero; //Vector3((-screenWidth / 2) + (i * (screenWidth / m_NotchCount)), 0);
                newNotch.anchoredPosition = Vector2.zero;
            }
        }

    }
}