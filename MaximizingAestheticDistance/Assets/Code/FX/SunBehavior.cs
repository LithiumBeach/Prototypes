using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    /// <summary>
    /// rotates about LOCAL X-Z plane. local up vector is the normal to it's orbit. this is important.
    /// </summary>
    public class SunBehavior : MonoBehaviour
    {
        public Gradient m_ColorOverDay;

        [Tooltip("orbit about this transform. if you're making a sun, for instance, you would want this to orbit about it'")]
        [Required]
        public Transform m_Parent = null;

    }
}