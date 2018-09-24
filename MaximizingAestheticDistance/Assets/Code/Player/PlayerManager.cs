using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    public class PlayerManager : SingletonBehavior<PlayerManager>
    {
        [Required]
        public Transform m_Player;
    }
}