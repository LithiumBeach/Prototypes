using patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public class BrainManager : SingletonBehavior<BrainManager>
    {
        

        //public bool ListenToCommand(CmdExplode _command)
        //{
        //
        //    return false;
        //}

        public bool ListenToString(string s)
        {
            return false;
        }
    }
}
