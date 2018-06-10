using patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace pe
{
    public class PlayerManager : SingletonBehavior<PlayerManager>
    {
        public PlayerShip m_PlayerShip;

        private BaseLaser m_FocusLaser;
        public BaseLaser FocusLaser
        {
            get
            {
                //introduce randomness here? yeah, probably.
                return m_FocusLaser;
            }
        }

        public List<BaseLaser> m_Lasers;

        protected override void OnAwake()
        {
            InitializeLasers();
        }

        private void InitializeLasers()
        {
            m_Lasers = new List<BaseLaser>();
            m_Lasers.Add(m_PlayerShip.transform.GetComponentInChildren<KineticLaser>(true));

            m_FocusLaser = m_Lasers[0];
        }

        public bool MovePlayerShipTo(Vector3 _pos, float _distanceFromObject = 0f, UnityAction _OnWarpFinished=null)
        {
            InfoUI.Instance.ClearLog();

            //temp
            m_PlayerShip.transform.LookAt(_pos, m_PlayerShip.transform.up);

            _pos += -m_PlayerShip.transform.forward * _distanceFromObject;

            m_PlayerShip.WarpTo(_pos, _OnWarpFinished);
            return true;
        }

        public bool MovePlayerShipTo(OrbitObject _o, UnityAction _OnWarpFinished=null)
        {
            Debug.Assert(_o != null);

            float radius = 0f;
            var planet = _o as PlanetBehavior;
            if (planet)
            {
                radius = planet.r * 2f;
            }

            return MovePlayerShipTo(_o.transform.position, radius, _OnWarpFinished);
        }


    }

}