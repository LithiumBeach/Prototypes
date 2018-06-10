using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public class CmdExplode : BaseCommand
    {
        public override bool Evaluate(string[] _params)
        {
            if (_params.Length != 0)
            {
                return false;
            }
            
            if (InfoManager.Instance.m_FocusPlanetIndex < 0)
            {
                return false;
            }

            PlayerManager.Instance.FocusLaser.FireAt(InstanceManager.Instance.FocusPlanet);

            //update info
            InfoUI.Instance.Print(InstanceManager.Instance.FocusPlanet);

            return true;
        }

        public override void Initialize()
        {
            m_CommandStringsRef = CommandStrings.Explode;
        }
    }
}
