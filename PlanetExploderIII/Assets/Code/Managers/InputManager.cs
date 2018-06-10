using patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace pe
{
    public class InputManager : SingletonBehavior<InputManager>
    {
        //<ctrl, alt, shift>
        public Action<bool, bool, bool> OnConsoleKeyDown;
        public Action<bool, bool, bool> OnConsoleKeyHeld;
        public Action<bool, bool, bool> OnConsoleKeyUp;

        //Log History --float is the positive or negative axis value
        public Action<bool, bool, bool, float> OnConsoleHistoryDown;

        public Action OnSubmitText;

        protected override void OnAwake()
        {
        }

        protected override void OnUpdate(float dt)
        {
            bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);


            if (OnConsoleKeyDown != null && Input.GetButtonDown("Console"))
            {
                OnConsoleKeyDown(ctrl, alt, shift);
            }
            if (OnConsoleKeyHeld != null &&  Input.GetButton("Console"))
            {
                OnConsoleKeyHeld(ctrl, alt, shift);
            }
            if (OnConsoleKeyUp != null && Input.GetButtonUp("Console"))
            {
                OnConsoleKeyUp(ctrl, alt, shift);
            }

            if (OnSubmitText != null && Input.GetButtonUp("Submit"))
            {
                OnSubmitText();
            }

            if (OnConsoleHistoryDown != null && Input.GetButtonDown("ConsoleHistory"))
            {
                OnConsoleHistoryDown(ctrl, alt, shift, Input.GetAxis("ConsoleHistory"));
            }
        }
    }
}