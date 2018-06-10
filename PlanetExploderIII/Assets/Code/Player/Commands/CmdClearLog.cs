using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public class CmdClearLog : BaseCommand
    {
        public override bool Evaluate(string[] _params)
        {
            PlayerConsoleUI.Instance.ClearLog();
            return true;
        }

        public override void Initialize()
        {
            m_CommandStringsRef = CommandStrings.ClearLog;
        }
    }
}
