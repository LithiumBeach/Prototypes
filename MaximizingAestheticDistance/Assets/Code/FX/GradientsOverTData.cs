using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{

    [CreateAssetMenu(fileName = "GradientsOverT_", menuName = "GenerationDatas/GradientsOverT", order = 1)]
    public class GradientsOverTData : ScriptableObject
    {
        [Tooltip("Must have one defined at 0. Interpolate between these as DayNightCycle t goes from 0-1. Do not define 1.")]
        public List<GradientGroup> m_GradientGroups;
        #region stress garbage
        //FUCKING FUCK FUCK FUCK WHY DOESNT THIS WORK FOR FUCKS SAKE. what's great, i mean what's really just fantastic, is that this WORKS when unity compiles.
        //setdirty refresh they all do jack fucking shit until this open goddamn garbage fire compiles.
        //[Button("Sort", ButtonSizes.Medium)]
        //private void Editor_SortGradients()
        //{
        //    if (m_GradientGroups != null && m_GradientGroups.Count > 1)
        //    {
        //        m_GradientGroups = m_GradientGroups.OrderBy(item => item.m_T).ToList();
        //    }
        //}
        #endregion

        /// <summary>
        /// assumes m_GradientGroups is SORTED!
        /// </summary>
        /// <param name="t">DayNightCycleManager.m_T!</param>
        /// <returns></returns>
        public Gradient GetGradientAt(float t)
        {
            //if t==0
            //boost performance (especially at startup)
            if (t < Mathf.Epsilon)
            {
                return m_GradientGroups[0].m_Gradient;
            }
            Debug.Assert(m_GradientGroups.Count >= 2, "Error! Must have minimum of 2 gradients defined in " + name);
            GradientGroup a = null;
            GradientGroup b = null;

            //find active gradients based on t. a should be before t, b should be after t.
            for (int i = 0; i < m_GradientGroups.Count; i++)
            {
                //when we find the first 
                if (m_GradientGroups[i].m_T >= t)
                {
                    if (i > 0)
                    {
                        a = m_GradientGroups[i - 1];
                        b = m_GradientGroups[i];
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("not sure how you got here buddy. YOU SHOULD HAVE ONE OF YOUR GRADIENTS IN " + name + " be t=0!!");
                        a = m_GradientGroups[0];
                        b = m_GradientGroups[1];
                        break;
                    }
                }
            }
            //we found a and b fine, interpolate normally
            if (a != null)
            {
                return GradientHelper.Lerp(a.m_Gradient, b.m_Gradient, Mathf.InverseLerp(a.m_T, b.m_T, t));
            }
            //if we didn't find one that's greater than t, loop around to first gradient
            else
            {
                a = m_GradientGroups[m_GradientGroups.Count - 1];
                b = m_GradientGroups[0];
                return GradientHelper.Lerp(a.m_Gradient, b.m_Gradient, Mathf.InverseLerp(a.m_T, b.m_T + 1f, t < b.m_T ? t + 1f : t));
            }
        }


    }

    [System.Serializable]
    public class GradientGroup
    {
        public Gradient m_Gradient;

        [Range(0f, .99f)]
        [Tooltip("START TIME. end time is defined by the next gradient group, or 1.")]
        public float m_T;
    }
}