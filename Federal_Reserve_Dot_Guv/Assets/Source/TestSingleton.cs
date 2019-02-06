using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace dd
{
    public class TestSingleton : util.SingletonBehavior<TestSingleton>
    {
        protected override void OnAwake()
        {
            Debug.Log("im awake.");
        }

        protected override void OnUpdate(float dt)
        {
            //floor
            int x = 1+(int)(Time.time * 4f) % 20;
            string o = "";
            for (int i = 0; i < x; ++i) { o += "."; }
            Debug.Log(o);
        }
    }
}