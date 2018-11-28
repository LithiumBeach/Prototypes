using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dd
{
    public interface IInteractAxisHandler
    {
        void HandlePositive();
        void HandleNegative();
    }
}