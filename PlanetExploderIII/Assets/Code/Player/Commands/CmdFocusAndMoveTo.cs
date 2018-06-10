using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public class CmdFocusAndMoveTo : BaseCommand
    {
        public override bool Evaluate(string[] _params)
        {
            //if there are no params, we can't cd to anything
            if (_params.Length < 1) { return false; }

            if (InfoManager.Instance.FocusPlanet != null)
            {
                //if back (../)
                if (CommandStrings.CheckString(_params[0], CommandStrings.LSBack))
                {
                    InfoManager.Instance.m_FocusPlanetIndex = -1;
                    return true;
                }
            }
            //try to CD to a planet first
            if (InfoManager.Instance.FocusSolarSystem != null)
            {
                //if back (../)
                if (CommandStrings.CheckString(_params[0], CommandStrings.LSBack))
                {
                    InfoManager.Instance.m_FocusSolarSystemIndex = -1;
                    return true;
                }

                //planetNames should be 1:1 inorder with Planets
                string[] planetNames = InfoManager.Instance.GetAllPlanetNamesInSolarSystem(InfoManager.Instance.FocusSolarSystem);
                for (int i = 0; i < planetNames.Length; i++)
                {
                    if (planetNames[i].ToUpper() == _params[0])
                    {
                        //if we aren't already focused on this planet:
                        if (InfoManager.Instance.m_FocusPlanetIndex != i)
                        {
                            InfoManager.Instance.m_FocusPlanetIndex = i;
                            PlayerManager.Instance.MovePlayerShipTo(InstanceManager.Instance.FocusPlanet, OnPlayershipReachedDestination);
                            return true;
                        }
                        //otherwise do nothing.
                    }
                }
            }
            //if that fails, try to CD to a solar system
            if (InfoManager.Instance.FocusGalaxy != null)
            {
                //if back (../)
                if (CommandStrings.CheckString(_params[0], CommandStrings.LSBack))
                {
                    //cannot go up from just a focus galaxy!
                    return false;
                }

                //planetNames should be 1:1 inorder with Planets
                string[] ssNames = InfoManager.Instance.GetAllSolarSystemNamesInGalaxy(InfoManager.Instance.FocusGalaxy.m_GalaxyInfo);
                for (int i = 0; i < ssNames.Length; i++)
                {
                    if (ssNames[i].ToUpper() == _params[0])
                    {
                        //if we aren't already focused on this SS:
                        if (InfoManager.Instance.m_FocusSolarSystemIndex != i)
                        {
                            InfoManager.Instance.m_FocusSolarSystemIndex = i;
                            PlayerManager.Instance.MovePlayerShipTo(InstanceManager.Instance.FocusSolarSystem.transform.position, InstanceManager.Instance.FocusSolarSystem.Radius, OnPlayershipReachedDestination);
                            return true;
                        }
                        //otherwise do nothing.
                    }
                }
            }
            // TODO: if that fails, try to CD to the wormhole


            return false;
        }

        private void OnPlayershipReachedDestination()
        {
            if (InstanceManager.Instance.FocusPlanet != null)
            {
                InfoUI.Instance.Print(InstanceManager.Instance.FocusPlanet as PlanetBehavior); 
            }
        }

        public override void Initialize()
        {
            m_CommandStringsRef = CommandStrings.FocusAndMoveTo;
        }
    }
}
