using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    public class GradientFunctors : SingletonBehavior<GradientFunctors>
    {
        public Gradient[] m_Gradients;
        public int m_GradientIndex = 0;

        public Color GetGradientAt(Vector3 position)
        {
            return m_Gradients[m_GradientIndex].Evaluate(Mathf.Clamp01(Mathf.InverseLerp(1, 8, position.y)));
        }
    }
}