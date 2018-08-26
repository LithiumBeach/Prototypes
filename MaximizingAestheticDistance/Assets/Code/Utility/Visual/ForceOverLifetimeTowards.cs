using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ForceOverLifetimeTowards : MonoBehaviour
    {
        [System.Serializable]
        public struct Target
        {
            //leave null for no force towards effect.
            public Transform trans;
            [Range(0f, 1f)]
            public float percentAffected;
            public float magnitude;

            //set particle alpha to 0 if square distance within this radius of this target.
            public float consumeSqrRadius;
            public Target(Transform _trans, float _percentAffected, float _magnitude, float _consumeSqrRadius)
            {
                trans = _trans; percentAffected = _percentAffected; magnitude = _magnitude; consumeSqrRadius = _consumeSqrRadius;
            }
        }

        //this needs to be an array.
        public Target[] targets;
        public int[] targetIndicesToParticles;

        private ParticleSystem ps;
        private ParticleSystem.Particle[] p;

        public int TotalNumParticles;

        void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        public void Initialize(Target[] _targets, int _totalNumParticles, bool newSeed = true)
        {
            targets = _targets;
            TotalNumParticles = _totalNumParticles;
            targetIndicesToParticles = new int[TotalNumParticles];

            //find all chances
            float[] inOrderChances = new float[targets.Length];
            float totalChance = 0f;
            for (int i = 0; i < targets.Length; i++)
            {
                totalChance += targets[i].percentAffected;
                inOrderChances[i] = totalChance;
            }

            //every particle now knows which targets index it follows.
            for (int i = 0; i < TotalNumParticles; i++)
            {
                targetIndicesToParticles[i] = RandomUtility.GetRandomIndexFromChanceSet(inOrderChances, totalChance);
            }
        }

        private float m_IterSqrDistance = 0f;
        private float m_Timer = 0f;
        private static readonly float m_MinTimeToDestroy = 0.5f;//wait a second, since they spawn at the planet edge they get destroyed immediately if they target the planet.
        private void FixedUpdate()
        {
            if (!ps.isPlaying) { return; }

            m_Timer += Time.deltaTime;

            int particleCount = ps.particleCount;
            p = new ParticleSystem.Particle[particleCount];
            ps.GetParticles(p);

            //for all (not a lot) targets
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].magnitude += 1.0f;
            }

            Vector3 iterDir = Vector3.zero;
            for (int i = 0; i < particleCount; i++)
            {
                //only if the particle is still 'alive'. alpha sets to 0 so we don't resize that dirty array
                if (p[i].startColor.a > 0.01f)
                {
                    //get direction from iter particle to iter particle target
                    iterDir = (targets[targetIndicesToParticles[i]].trans.position - p[i].position).normalized;
                    //set velocity based on direction and iter magnitude
                    p[i].velocity += iterDir * targets[targetIndicesToParticles[i]].magnitude * Time.fixedDeltaTime;
                    //if we are able to kill the particle yet
                    if (m_Timer > m_MinTimeToDestroy)
                    {
                        m_IterSqrDistance = Vector3.SqrMagnitude(targets[targetIndicesToParticles[i]].trans.position - p[i].position);
                        //if we are within consumption range
                        if (m_IterSqrDistance < targets[targetIndicesToParticles[i]].consumeSqrRadius)
                        {
                            //'kill' particle
                            p[i].startColor = Color.clear;

                            // TODO: add mass to target HERE!!
                        }
                    }
                }
            }
            ps.SetParticles(p, particleCount);
        }
    }
}