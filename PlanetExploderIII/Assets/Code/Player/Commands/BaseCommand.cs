using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public abstract class BaseCommand
    {
        protected bool m_Paused = false;
        public bool Pause { get { return m_Paused; } set { m_Paused = value; } }

        /// <summary>
        /// set m_CommandStringsRef to something
        /// </summary>
        public abstract void Initialize();

        public string[] m_CommandStringsRef=null;

        /// <summary>
        /// Override with command functionality, handle parameters yourself.
        /// </summary>
        /// <param name="_params">call String.Split('~delimeter char~') before this, 
        /// isolate params into elements of array</param>
        public abstract bool Evaluate(string[] _params);

        /// <summary>
        /// Is the parameter string a valid call to this command?
        /// Call this on all commands to find a match
        /// Assumes m_CommandStringsRef has been set to something..
        /// </summary>
        public virtual bool CheckCommandString(string _cmd)
        {
            if (m_Paused) return false;

            _cmd = _cmd.ToUpper();

            //iterate through Command Strings valid entries, look for match with param
            for (int i = 0; i < m_CommandStringsRef.Length; i++)
            {
                if (m_CommandStringsRef[i] == _cmd)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
