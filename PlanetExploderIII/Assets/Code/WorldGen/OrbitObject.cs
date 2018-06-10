using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public abstract class OrbitObject : MonoBehaviour
    {
        [HideInInspector]
        //radius of orbit
        public float R;

        [HideInInspector]
        //radius of object
        private float m_r;
        public virtual float r
        {
            get { return m_r; }
            set { m_r = value; }
        }

        public virtual void Explode()
        {
        }
    }
}
