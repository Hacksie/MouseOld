using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.Input
{
    public class DesktopInputController : IInputController
    {

        public void SetMobileInput(UI.MobileInputUIPresenter mobileInputUI)
        {
            // do nothing;
        }

        public bool ShowMobileInput()
        {
            return false;
        }


        public Vector2 GetMovementAxis()
        {
            return new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
        }

        public void ResetInput()
        {
            UnityEngine.Input.ResetInputAxes();
        }

        public bool StartButtonUp()
        {
            return UnityEngine.Input.GetButtonUp("Start");
        }

        public bool SelectButtonUp()
        {
            return UnityEngine.Input.GetButtonUp("Select");
        }

        public bool InteractButtonUp()
        {
            return UnityEngine.Input.GetButtonUp("Interact");
        }

        public bool OverloadButtonUp()
        {
            return UnityEngine.Input.GetButtonUp("Overload");
        }

        public bool HackButtonUp()
        {
            return UnityEngine.Input.GetButtonUp("Hack");
        }

        public bool KeycardButtonUp()
        {
            return UnityEngine.Input.GetButtonUp("Keycard");
        }

        public bool BugButtonUp()
        {
            return UnityEngine.Input.GetButtonUp("Bug");
        }
    }

}